using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services;
using HandyCrab.Common.Entitys;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;
using Xamarin.Forms;
using Barrier = HandyCrab.Common.Entitys.Barrier;

namespace HandyCrab.Business.ViewModels
{
    class BarrierViewModel : BaseViewModel, IBarrierViewModel
    {
        private string barrierId;
        private ImageSource image;
        private string title;
        private double longitude;
        private double latitude;
        private string description;
        private string postcode;
        private Vote userVote;
        int upVotes;
        int downVotes;
        private IEnumerable<Solution> solutions;
        private string newSolutionText;
        private ICommand upVoteCommand;
        private ICommand downVoteCommand;
        private ICommand addSolutionCommand;

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
                        Postcode = barrierToShow.Postcode;
                        UserVote = barrierToShow.Vote;
                        this.upVotes = barrierToShow.Upvotes;
                        this.downVotes = barrierToShow.Downvotes;
                        RaisePropertyChanged(nameof(TotalVotes));
                        Solutions = barrierToShow.Solutions.OrderByDescending(x => x.Upvotes - x.Downvotes);
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
            get => this.image;
            set
            {
                SetProperty(ref this.image, value);
            }
        }

        public string Title
        {
            get => this.title;
            set
            {
                SetProperty(ref this.title, value);
            }
        }

        public double Longitude
        {
            get => this.longitude;
            set
            {
                SetProperty(ref this.longitude, value);
            }
        }

        public double Latitude
        {
            get => this.latitude;
            set
            {
                SetProperty(ref this.latitude, value);
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                SetProperty(ref this.description, value);
            }
        }

        public string Postcode
        {
            get => this.postcode;
            set
            {
                SetProperty(ref this.postcode, value);
            }
        }

        public IEnumerable<Solution> Solutions
        {
            get => this.solutions;
            set
            {
                SetProperty(ref this.solutions, value);
            }
        }

        public BarrierViewModel()
        {
            this.upVoteCommand = new Command<string>(UpVoteAction);
            this.downVoteCommand = new Command<string>(DownVoteAction);
            this.addSolutionCommand = new Command(AddSolutionAction);
        }

        public ICommand UpVoteCommand => this.upVoteCommand;

        public ICommand DownVoteCommand => this.downVoteCommand;

        public Vote UserVote
        {
            get => this.userVote;
            set
            {
                SetProperty(ref this.userVote, value);
            }
        }

        public int TotalVotes => this.upVotes - this.downVotes;

        public string NewSolutionText 
        { 
            get => this.newSolutionText;
            set
            {
                SetProperty(ref this.newSolutionText, value);
            }
        }

        public ICommand AddSolutionCommand => this.addSolutionCommand;

        private void UpVoteAction(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                voteBarrier(Vote.Up);
            } else
            {
                voteSolution(Vote.Up, id);
            }
        }

        private void DownVoteAction(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                voteBarrier(Vote.Down);
            }
            else
            {
                voteSolution(Vote.Down, id);
            }
        }

        private async void voteBarrier(Vote vote)
        {
            vote = this.UserVote == vote ? Vote.None : vote;
            var client = Factory.Get<IBarrierClient>();
            var task = await client.VoteBarrierAsync(this.BarrierId, vote);
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
                RaisePropertyChanged("TotalVotes");
                UserVote = vote;
                saveBarrier();
                VoteSucceeded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                RaiseOnError(task);
            }
        }

        private async void voteSolution(Vote vote, String id)
        {
            var client = Factory.Get<IBarrierClient>();
            var task = await client.VoteSolutionAsync(id, vote);
            if(task.IsSucceeded())
            {
                var solutions = Solutions.ToList();
                for (int i = 0; i < solutions.Count; i++)
                {
                    if(solutions[i].Id == id)
                    {
                        vote = solutions[i].Vote == vote ? Vote.None : vote;
                        if (vote == Vote.Up)
                        {
                            solutions[i].Upvotes += 1;
                        } else if (vote == Vote.Down)
                        {
                            solutions[i].Downvotes += 1;
                        }
                        if (solutions[i].Vote == Vote.Up)
                        {
                            solutions[i].Upvotes -= 1;
                        } else if (solutions[i].Vote == Vote.Down)
                        {
                            solutions[i].Downvotes -= 1;
                        }
                        solutions[i].Vote = vote;
                        Solutions = solutions;
                        saveBarrier();
                        VoteSucceeded?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }
            } else
            {
                RaiseOnError(task);
            }
        }

        private async void AddSolutionAction()
        {
            var client = Factory.Get<IBarrierClient>();
            var solution = new Solution();
            solution.Text = this.newSolutionText;
            var barrier = await client.AddBarrierSolutionAsync(this.barrierId, solution);
            if (barrier.IsSucceeded())
            {
                Solutions = barrier.Value.Solutions;
                saveBarrier();
                AddSolutionSucceeded?.Invoke(this, EventArgs.Empty);
            } else
            {
                RaiseOnError(barrier);
            }
        }

        private void saveBarrier()
        {
            var storageService = Factory.Get<IInternalRuntimeDataStorageService>();
            var barriers = storageService.GetValue<IEnumerable<Barrier>>(StorageSlot.BarrierSearchResults);
            var barrierList = barriers.ToList<Barrier>();
            for (int i = 0; i < barrierList.Count; i++)
            {
                if (barrierList.ElementAt<Barrier>(i).Id == BarrierId)
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
