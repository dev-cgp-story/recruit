using Newtonsoft.Json;

namespace Entity
{
    [JsonObject(MemberSerialization.Fields)]
    public struct RoomType
    {
        [JsonProperty("coordinatesBase64s")]
        public readonly Coordinate[] coordinates;
        [JsonProperty("meta")]
        public readonly RoomTypeMeta meta;
    }
}