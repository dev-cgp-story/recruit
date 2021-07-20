using Newtonsoft.Json;
using Entity.Converter;

namespace Entity
{
    [JsonConverter(typeof(CoordinateConverter))]
    public struct Coordinate
    {
        public readonly float[] values;

        public Coordinate(float[] values)
        {
            this.values = values;
        }
    }
}