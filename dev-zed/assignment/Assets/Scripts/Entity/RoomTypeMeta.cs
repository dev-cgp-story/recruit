using Newtonsoft.Json;

namespace Entity
{
    [JsonObject(MemberSerialization.Fields)]
    public struct RoomTypeMeta
    {
        [JsonProperty("룸타입id")]
        public readonly int roomTypeId;
    }
}