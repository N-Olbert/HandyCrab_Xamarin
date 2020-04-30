using System.Collections.Generic;
using JetBrains.Annotations;
using Xamarin.Forms;
using HandyCrab.Common.Entitys;

namespace HandyCrab.Common.Interfaces
{
    public interface IBarrierViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the Barrier to be displayed
        /// </summary>
        string BarrierId { get; set; }

        /// <summary>
        /// Gets the Image of the barrier
        /// </summary>
        [NotNull]
        Image Image { get; }

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
        /// Gets the barriers solutions.
        /// </summary>
        IEnumerable<Solution> Solutions { get; }
    }
}
