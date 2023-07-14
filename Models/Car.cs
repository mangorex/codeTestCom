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

    public enum CarType
    {
        Premium,
        Suv,
        Small
    }
}

