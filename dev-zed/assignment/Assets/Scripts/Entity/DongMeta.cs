using Newtonsoft.Json;

namespace Entity
{
    [JsonObject(MemberSerialization.Fields)]
    public struct DongMeta
    {
        [JsonProperty("bd_id")]
        readonly public int bdId;
        [JsonProperty("동")]
        readonly public string dong;
        [JsonProperty("지면높이")]
        readonly public int height;
    }
}