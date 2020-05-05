using System;
using System.Collections.Generic;
using System.Windows.Input;
using JetBrains.Annotations;
using Xamarin.Forms;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Common.Interfaces
{
    public interface IBarrierViewModel : IViewModel
    {
        /// <summary>
        /// This event fires when a solution was successfully added.
        /// </summary>
        event EventHandler AddSolutionSucceeded;

        /// <summary>
        /// This event fires when a barrier or solution was successfully voted.
        /// </summary>
        event EventHandler VoteSucceeded;


        /// <summary>
        /// Gets or sets the Id of the Barrier to be displayed
        /// </summary>
        string BarrierId { get; set; }

        /// <summary>
        /// Gets the Image of the barrier
        /// </summary>
        [NotNull]
        ImageSource Image { get; }

        /// <summary>
        /// Gets the barrier title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        double Longitude { get; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        double Latitude { get; }

        /// <summary>
        /// Gets the barriers description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the barriers postcode.
        /// </summary>
        string Postcode { get; }

        /// <summary>
        /// Gets the Value of the users´ vote for this barrier.
        /// </summary>
        Vote UserVote { get; }

        /// <summary>
        /// Gets the number of upvotes minus the number of downvotes.
        /// </summary>
        int TotalVotes { get; }

        /// <summary>
        /// Gets the barriers solutions.
        /// </summary>
        IEnumerable<Solution> Solutions { get; }

        /// <summary>
        /// Gets or Sets the content of a new solution.
        /// </summary>

        string NewSolutionText { get; set; }

        /// <summary>
        /// Gets the upvote command.
        /// </summary>
        ICommand UpVoteCommand { get; }

        /// <summary>
        /// Gets the downvote command.
        /// </summary>
        ICommand DownVoteCommand { get; }

        /// <summary>
        /// Gets the addsolution command.
        /// </summary>
        ICommand AddSolutionCommand { get; }
    }
}
