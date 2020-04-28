using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HandyCrab.Common.Entitys
{
    /// <summary>
    /// Vote enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Vote
    {
        /// <summary>
        /// Default -- no vote at all
        /// </summary>
        [EnumMember(Value = "NONE")]
        None = 0,

        /// <summary>
        /// Vote up
        /// </summary>
        [EnumMember(Value = "UP")]
        Up = 1,

        /// <summary>
        /// Vote down
        /// </summary>
        [EnumMember(Value = "DOWN")]
        Down = 2,
    }
}