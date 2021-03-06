﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using JetBrains.Annotations;

namespace HandyCrab.Business.Services
{
    /// <summary>
    /// Interface for any barrier client.
    /// </summary>
    internal interface IBarrierClient
    {
        /// <summary>
        /// Gets the barriers.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="radius">The search radius.</param>
        /// <returns>IEnumberable of returned barriers (might be empty).</returns>
        [NotNull] 
        Task<Failable<IEnumerable<Barrier>>> GetBarriersAsync(double longitude, double latitude, float radius);

        /// <summary>
        /// Gets the barriers.
        /// </summary>
        /// <param name="postCode">The post code to search.</param>
        /// <returns>IEnumberable of returned barriers (might be empty).</returns>
        [NotNull] 
        Task<Failable<IEnumerable<Barrier>>> GetBarriersAsync(string postCode);

        /// <summary>
        /// Gets the barriers of the current user.
        /// </summary>
        /// <returns>IEnumberable of returned barriers (might be empty).</returns>
        [NotNull] 
        Task<Failable<IEnumerable<Barrier>>> GetBarriersOfCurrentUserAsync();

        /// <summary>
        /// Gets the barrier by its id.
        /// </summary>
        /// <param name="id">The identifier of the id.</param>
        /// <returns>The barrier which corresponds to the id.</returns>
        [NotNull] 
        Task<Failable<Barrier>> GetBarrierAsync(string id);

        /// <summary>
        /// Adds a new barrier.
        /// </summary>
        /// <param name="barrierToAdd">The base data of the barrier to add.</param>
        /// <param name="base64Picture">Optional: The modified picture as base64-string.</param>
        /// <param name="barrierSolution">Optional: A solution for the barrier.</param>
        /// <returns>The newly created barrier.</returns>
        [NotNull] 
        Task<Failable<Barrier>> AddBarrierAsync(Barrier barrierToAdd, string base64Picture, Solution barrierSolution);

        /// <summary>
        /// Modifies an exiting barrier.
        /// </summary>
        /// <param name="barrierId">The identifier of the barrier.</param>
        /// <param name="title">Optional: The modified title.</param>
        /// <param name="base64Picture">Optional: The modified picture as base64-string.</param>
        /// <param name="description">Optional: The modified description.</param>
        /// <returns>The modified barrier.</returns>
        [NotNull] 
        Task<Failable<Barrier>> ModifyBarrierAsync(string barrierId, string title, string base64Picture, string description);

        /// <summary>
        /// Modifies an exiting barrier.
        /// </summary>
        /// <param name="barrierId">The identifier of the barrier to delete.</param>
        /// <returns>Failable, indicating success</returns>
        [NotNull] 
        Task<Failable> DeleteBarrierAsync(string barrierId);

        /// <summary>
        /// Adds a new solution to an existing barrier.
        /// </summary>
        /// <param name="barrierId">The identifier of the existing barrier.</param>
        /// <param name="solutionToAdd">The solution to add.</param>
        /// <returns>The modified barrier.</returns>
        [NotNull] 
        Task<Failable<Barrier>> AddBarrierSolutionAsync(string barrierId, Solution solutionToAdd);

        /// <summary>
        /// Votes up or down an existing barrier.
        /// </summary>
        /// <param name="barrierId">The identifier of the existing barrier.</param>
        /// <param name="vote">The vote to execute.</param>
        /// <returns>Failable indicating success.</returns>
        [NotNull]
        Task<Failable> VoteBarrierAsync(string barrierId, Vote vote);

        /// <summary>
        /// Votes up or down an existing solution.
        /// </summary>
        /// <param name="solutionId">The identifier of the existing solution.</param>
        /// <param name="vote">The vote to execute.</param>
        /// <returns>Failable indicating success.</returns>
        [NotNull]
        Task<Failable> VoteSolutionAsync(string solutionId, Vote vote);
    }
}