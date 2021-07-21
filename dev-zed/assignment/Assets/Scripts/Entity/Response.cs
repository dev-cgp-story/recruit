using Newtonsoft.Json;

namespace Entity
{
    [JsonObject(MemberSerialization.Fields)]
    public class Response<T>
    {
        public readonly bool success;
        public readonly int code;
        public readonly T data;
    }
}