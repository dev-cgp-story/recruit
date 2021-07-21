using Newtonsoft.Json;

namespace Entity
{
    [JsonObject(MemberSerialization.Fields)]
    public struct Dong
    {
        [JsonProperty("roomtypes")]
        public readonly RoomType[] roomTypes;
        [JsonProperty("meta")]
        public readonly DongMeta meta;
    }
}