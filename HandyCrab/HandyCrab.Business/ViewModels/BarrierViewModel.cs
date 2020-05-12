using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using Xamarin.Forms;
using Barrier = HandyCrab.Common.Entitys.Barrier;

namespace HandyCrab.Business.ViewModels
{
    internal class BarrierViewModel : BaseViewModel, IBarrierViewModel
    {
        private string barrierId;
        private ImageSource image;
        private string title;
        private double longitude;
        private double latitude;
        private string description;
        private string author;
        private string postcode;
        private Vote userVote;
        private int upVotes;
        private int downVotes;
        private IEnumerable<Solution> solutions;
        private string newSolutionText;

        public event EventHandler VoteSucceeded;
        public event EventHandler AddSolutionSucceeded;

        public string BarrierId 
        { 
            get => this.barrierId; 
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
                    var barriers = storageService.GetValue<IEnumerable<Barrier>>(StorageSlot.BarrierSearchResults);
                    var barrierToShow = barriers?.FirstOrDefault(x => x != null && x.Id == value);
                    if (barrierToShow != null)
                    {
                        SetProperty(ref this.barrierId, value);
                        Title = barrierToShow.Title;
                        Longitude = barrierToShow.Longitude;
                        Latitude = barrierToShow.Latitude;
                        Description = barrierToShow.Description;
                        Author = barrierToShow.UserName;
                        Postcode = barrierToShow.Postcode;
                        UserVote = barrierToShow.Vote;
                        this.upVotes = barrierToShow.Upvotes;
                        this.downVotes = barrierToShow.Downvotes;
                        RaisePropertyChanged(nameof(TotalVotes));
                        Solutions = barrierToShow.Solutions?.OrderByDescending(x => x.Upvotes - x.Downvotes);
                        var imageSource = new UriImageSource();
                        if (!string.IsNullOrEmpty(barrierToShow.Picture))
                        {
                            imageSource.Uri = new Uri(barrierToShow.Picture);
                        }

                        Image = imageSource;
                    }
                    else
                    {
                        RaiseOnError(new InvalidOperationException("Requested barrier does not exist."));
                    }
                }
                else
                {
                    Title = string.Empty;
                    Longitude = 0;
                    Latitude = 0;
                    Description = string.Empty;
                    Postcode = string.Empty;
                    Solutions = null;
                    Image = new StreamImageSource();
                }
            }
        }

        public ImageSource Image
        {
            get => this.image ?? new StreamImageSource();
            set => SetProperty(ref this.image, value);
        }

        public string Title
        {
            get => this.title;
            set => SetProperty(ref this.title, value);
        }

        public double Longitude
        {
            get => this.longitude;
            set => SetProperty(ref this.longitude, value);
        }

        public double Latitude
        {
            get => this.latitude;
            set => SetProperty(ref this.latitude, value);
        }

        public string Description
        {
            get => this.description;
            set => SetProperty(ref this.description, value);
        }

        public string Postcode
        {
            get => this.postcode;
            set => SetProperty(ref this.postcode, value);
        }

        public IEnumerable<Solution> Solutions
        {
            get => this.solutions;
            set => SetProperty(ref this.solutions, value);
        }

        public BarrierViewModel()
        {
            UpVoteCommand = new Command<string>(UpVoteAction);
            DownVoteCommand = new Command<string>(DownVoteAction);
            AddSolutionCommand = new Command(AddSolutionAction);
        }

        public ICommand UpVoteCommand { get; }

        public ICommand DownVoteCommand { get; }

        public Vote UserVote
        {
            get => this.userVote;
            set => SetProperty(ref this.userVote, value);
        }

        public int TotalVotes => this.upVotes - this.downVotes;

        public string NewSolutionText 
        { 
            get => this.newSolutionText;
            set => SetProperty(ref this.newSolutionText, value);
        }

        public ICommand AddSolutionCommand { get; }

        public string Author
        {
            get => this.author;
            set => SetProperty(ref this.author, value);
        }

        private async void UpVoteAction(string id)
        {
            //async void is ok (event handler)
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    await VoteBarrier(Vote.Up);
                }
                else
                {
                    await VoteSolution(Vote.Up, id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RaiseOnError(e);
            }
        }

        private async void DownVoteAction(string id)
        {
            //async void is ok (event handler)
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    await VoteBarrier(Vote.Down);
                }
                else
                {
                    await VoteSolution(Vote.Down, id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RaiseOnError(e);
            }
        }

        private async Task VoteBarrier(Vote vote)
        {
            vote = UserVote == vote ? Vote.None : vote;
            var client = Factory.Get<IBarrierClient>();
            var task = await client.VoteBarrierAsync(BarrierId, vote);
            if (task.IsSucceeded())
            {
                if (vote == Vote.Up)
                {
                    this.upVotes += 1;
                }
                else if (vote == Vote.Down)
                {
                    this.downVotes += 1;
                }

                if (UserVote == Vote.Up)
                {
                    this.upVotes -= 1;
                }
                else if (UserVote == Vote.Down)
                {
                    this.downVotes -= 1;
                }

                RaisePropertyChanged(nameof(TotalVotes));
                UserVote = vote;
                SaveBarrier();
                VoteSucceeded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                RaiseOnError(task);
            }
        }

        private async Task VoteSolution(Vote vote, string id)
        {
            var client = Factory.Get<IBarrierClient>();
            var task = await client.VoteSolutionAsync(id, vote);
            if (task.IsSucceeded())
            {
                var currentSolutions = Solutions?.ToList();
                if (currentSolutions != null)
                {
                    for (int i = 0; i < currentSolutions.Count; i++)
                    {
                        if (currentSolutions[i] != null && currentSolutions[i].Id == id)
                        {
                            vote = currentSolutions[i].Vote == vote ? Vote.None : vote;
                            if (vote == Vote.Up)
                            {
                                currentSolutions[i].Upvotes += 1;
                            }
                            else if (vote == Vote.Down)
                            {
                                currentSolutions[i].Downvotes += 1;
                            }

                            if (currentSolutions[i].Vote == Vote.Up)
                            {
                                currentSolutions[i].Upvotes -= 1;
                            }
                            else if (currentSolutions[i].Vote == Vote.Down)
                            {
                                currentSolutions[i].Downvotes -= 1;
                            }

                            currentSolutions[i].Vote = vote;
                            Solutions = currentSolutions;
                            SaveBarrier();
                            VoteSucceeded?.Invoke(this, EventArgs.Empty);
                            break;
                        }
                    }
                }
            }
            else
            {
                RaiseOnError(task);
            }
        }

        private async void AddSolutionAction()
        {
            //async void is ok (event handler)
            try
            {
                var client = Factory.Get<IBarrierClient>();
                var solution = new Solution();
                solution.Text = this.newSolutionText;
                var barrier = await client.AddBarrierSolutionAsync(this.barrierId, solution);
                if (barrier.IsSucceeded())
                {
                    Solutions = barrier.Value.Solutions?.OrderByDescending(x => x.Upvotes - x.Downvotes);
                    SaveBarrier();
                    AddSolutionSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    RaiseOnError(barrier);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RaiseOnError(e);
            }
        }

        private void SaveBarrier()
        {
            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var barriers = storageService.GetValue<IEnumerable<Barrier>>(StorageSlot.BarrierSearchResults);
            var barrierList = barriers.ToList();
            for (int i = 0; i < barrierList.Count; i++)
            {
                if (barrierList.ElementAt(i).Id == BarrierId)
                {
                    barrierList[i].Solutions = Solutions.ToList();
                    barrierList[i].Upvotes = this.upVotes;
                    barrierList[i].Downvotes = this.downVotes;
                    barrierList[i].Vote = UserVote;
                    break;
                }
            }

            storageService.StoreValue(StorageSlot.BarrierSearchResults, barrierList);
        }
    }
}
