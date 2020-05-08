using System;
using System.Windows.Input;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Common.Interfaces
{
    public interface IAddOrModifyBarrierViewModel : IImagePickerViewModel
    {
        /// <summary>
        /// Occurs when adding/modifying succeeds
        /// </summary>
        event EventHandler<IReadOnlyBarrier> OnSuccess;

        /// <summary>
        /// Gets or sets the modified barrier identifier.
        /// If null a new barrier will be created
        /// </summary>
        string ModifiedBarrierId { get; set; }

        /// <summary>
        /// Gets or sets the barrier title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the barriers description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the barriers postcode.
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the initial solution text.
        /// Might be null.
        /// </summary>
        string InitialSolutionText { get; set; }

        /// <summary>
        /// Gets the add or modify barrier command.
        /// </summary>
        ICommand AddOrModifyBarrierCommand { get; }
    }
}