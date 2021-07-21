using System;
using Newtonsoft.Json;

namespace Entity.Converter
{
    public class CoordinateConverter : JsonConverter<Coordinate>
    {
        public override Coordinate ReadJson(JsonReader reader, Type objectType, Coordinate existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string base64string = Convert.ToString(reader.Value);
            var bytes = Convert.FromBase64String(base64string);
            float[] floats = new float[bytes.Length / sizeof(float)];
            Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
            return new Coordinate(floats);
        }

        public override void WriteJson(JsonWriter writer, Coordinate value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}