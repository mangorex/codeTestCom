using System.Text.Json.Serialization;

namespace codeTestCom.Models
{
    public class Car
    {
        public string Name { get; set; }
        public CarType Type { get; set; }

        public Car(string name, CarType type)
        {
            this.Name = name;
            this.Type = type;
        }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CarType
    {
        Premium,
        Suv,
        Small
    }
}

