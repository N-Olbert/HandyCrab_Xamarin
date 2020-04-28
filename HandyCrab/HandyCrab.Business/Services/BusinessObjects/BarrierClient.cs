using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HandyCrab.Common.Entitys;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    internal class BarrierClient: BaseClient, IBarrierClient
    {
        public BarrierClient(HttpClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public async Task<Failable<IEnumerable<Barrier>>> GetBarriersAsync(double longitude, double latitude, float radius)
        {
            const string settingsName = "BarrierClientBaseAddressGet";
            try
            {
                var data = new PositionRequestData { Longitude = longitude, Latitude = latitude, Radius = radius };
                var result = await PerformBarrierRequest<List<Barrier>>(settingsName, data, HttpMethod.Get);
                return result.ConvertDown<IEnumerable<Barrier>>();
            }
            catch (Exception e)
            {
                return new Failable<IEnumerable<Barrier>>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable<IEnumerable<Barrier>>> GetBarriersAsync(string postCode)
        {
            const string settingsName = "BarrierClientBaseAddressGet";
            try
            {
                var requestData = new PostCodeRequestData {Postcode = postCode};
                var result = await PerformBarrierRequest<List<Barrier>>(settingsName, requestData, HttpMethod.Get);
                return result.ConvertDown<IEnumerable<Barrier>>();
            }
            catch (Exception e)
            {
                return new Failable<IEnumerable<Barrier>>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable<Barrier>> GetBarrierAsync(string id)
        {
            const string settingsName = "BarrierClientBaseAddressGet";
            try
            {
                var requestData = new IdRequestData {Id = id};
                return await PerformBarrierRequest<Barrier>(settingsName, requestData, HttpMethod.Get);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable<Barrier>> AddBarrierAsync(Barrier barrierToAdd, string base64Picture, Solution barrierSolution)
        {
            const string settingsName = "BarrierClientBaseAddressAdd";
            try
            {
                if (barrierToAdd == null || string.IsNullOrEmpty(barrierToAdd.Title ?? barrierToAdd.Postcode) ||
                    barrierToAdd.Longitude == 0 || barrierToAdd.Latitude == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(barrierToAdd));
                }

                var requestData = new BarrierRequestData
                {
                    Description = barrierToAdd.Description,
                    Longitude = barrierToAdd.Longitude,
                    Latitude = barrierToAdd.Latitude,
                    Picture = barrierToAdd.Picture,
                    Postcode = barrierToAdd.Postcode,
                    Solution = barrierSolution,
                    Title = barrierToAdd.Title
                };

                return await PerformBarrierRequest<Barrier>(settingsName, requestData, HttpMethod.Post);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable<Barrier>> ModifyBarrierAsync(string barrierId, string title, string base64Picture,
                                                                string description)
        {
            const string settingsName = "BarrierClientBaseAddressModify";
            try
            {
                if (string.IsNullOrEmpty(barrierId))
                {
                    throw new ArgumentNullException(nameof(barrierId));
                }

                var requestData = new BarrierRequestData
                {
                    Id = barrierId,
                    Description = description,
                    Picture = base64Picture,
                    Title = title
                };

                return await PerformBarrierRequest<Barrier>(settingsName, requestData, HttpMethod.Put);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable<Barrier>> AddBarrierSolutionAsync(string barrierId, Solution solutionToAdd)
        {
            const string settingsName = "BarrierClientBaseAddressAddSolution";
            try
            {
                if (string.IsNullOrEmpty(barrierId))
                {
                    throw new ArgumentNullException(nameof(barrierId));
                }

                if (solutionToAdd == null)
                {
                    throw new ArgumentNullException(nameof(solutionToAdd));
                }

                var requestData = new BarrierRequestData
                {
                    Id = barrierId,
                    Solution = solutionToAdd
                };

                return await PerformBarrierRequest<Barrier>(settingsName, requestData, HttpMethod.Post);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable> VoteBarrierAsync(string barrierId, Vote vote)
        {
            const string settingsName = "BarrierClientBaseAddressVote";
            try
            {
                if (string.IsNullOrEmpty(barrierId))
                {
                    throw new ArgumentNullException(nameof(barrierId));
                }

                var requestData = new VoteRequestData
                {
                    Id = barrierId,
                    Vote = vote
                };

                return await PerformBarrierRequest<object>(settingsName, requestData, HttpMethod.Put);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        /// <inheritdoc />
        public async Task<Failable> VoteSolutionAsync(string solutionId, Vote vote)
        {
            const string settingsName = "BarrierClientBaseAddressSolutionVote";
            try
            {
                if (string.IsNullOrEmpty(solutionId))
                {
                    throw new ArgumentNullException(nameof(solutionId));
                }

                var requestData = new VoteRequestData
                {
                    Id = solutionId,
                    Vote = vote
                };

                return await PerformBarrierRequest<object>(settingsName, requestData, HttpMethod.Put);
            }
            catch (Exception e)
            {
                return new Failable<Barrier>(e);
            }
        }

        //// private methods

        private async Task<Failable<T>> PerformBarrierRequest<T>(string settingsName, object requestData, HttpMethod method)
        {
            var uri = AssemblyConfig<BarrierClient>.GetValue(settingsName);
            var message = await GetHttpMessageWithSessionCookie(uri, method);
            message.Content = requestData.ToJsonContent();

            var response = await Client.SendAsync(message);
            var result = await HandleResponseAsync<T>(response);
            return result;
        }

        #region private data structs (for json)
        private struct PositionRequestData
        {
            [JsonProperty("longitude")] internal double Longitude;
            [JsonProperty("latitude")] internal double Latitude;
            [JsonProperty("radius")] internal float Radius;
        }

        private struct PostCodeRequestData
        {
            [JsonProperty("postcode")] internal string Postcode;
        }

        private struct IdRequestData
        {
            [JsonProperty("_id")] internal string Id;
        }

        private struct BarrierRequestData
        {
            [JsonProperty("_id")] internal string Id;
            [JsonProperty("title")] internal string Title;
            [JsonProperty("longitude")] internal double Longitude;
            [JsonProperty("latitude")] internal double Latitude;
            [JsonProperty("picture")] internal string Picture;
            [JsonProperty("description")] internal string Description;
            [JsonProperty("postcode")] internal string Postcode;
            [JsonProperty("solution")] internal Solution Solution;
        }

        private struct VoteRequestData
        {
            [JsonProperty("_id")] internal string Id;
            [JsonProperty("vote")] internal Vote Vote;
        }
        #endregion
    }
}