using System.Collections.Generic;

namespace HandyCrab.Common.Entitys
{
    public interface IReadOnlyBarrier
    {
        string Id { get; }

        string UserId { get; }

        string Title { get; }

        double Longitude { get; }

        double Latitude { get; }

        string Picture { get; }

        string Description { get; }

        string Postcode { get; }

        List<Solution> Solutions { get; }

        int Upvotes { get; }

        int Downvotes { get; }

        Vote Vote { get; }
    }
}