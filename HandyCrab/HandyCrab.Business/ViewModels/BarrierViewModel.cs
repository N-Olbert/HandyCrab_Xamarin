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
        int totalVotes;
        private IEnumerable<Solution> solutions;
        private ICommand upVoteCommand;
        private ICommand downVoteCommand;

        public event EventHandler VoteSucceeded;

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
                        TotalVotes = barrierToShow.Upvotes - barrierToShow.Downvotes;
                        Solutions = barrierToShow.Solutions;
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

        public int TotalVotes
        {
            get => this.totalVotes;
            set
            {
                SetProperty(ref this.totalVotes, value);
            }
        }

        private void UpVoteAction(string id)
        {
            if (this.userVote == Vote.Up)
            {
                voteBarrier(Vote.None);
            } else
            {
                voteBarrier(Vote.Up);
            }
        }

        private void DownVoteAction(string id)
        {
            if (this.userVote == Vote.Down)
            {
                voteBarrier(Vote.None);
            }
            else
            {
                voteBarrier(Vote.Down);
            }
        }

        private async void voteBarrier(Vote vote)
        {
            var client = Factory.Get<IBarrierClient>();
            var task = await client.VoteBarrierAsync(this.BarrierId, vote);
            if (task.IsSucceeded())
            {
                if (UserVote == Vote.Down && vote == Vote.Up)
                {
                    TotalVotes += 2;
                } 
                else if ((UserVote == Vote.Down && vote == Vote.None) || (UserVote == Vote.None && vote == Vote.Up))
                {
                    TotalVotes += 1;
                } 
                else if ((UserVote == Vote.Up && vote == Vote.None) || (UserVote == Vote.None && vote == Vote.Down))
                {
                    TotalVotes -= 1;
                } 
                else if (UserVote == Vote.Up && vote == Vote.Down)
                {
                    TotalVotes -= 2;
                }
                UserVote = vote;
                VoteSucceeded?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
