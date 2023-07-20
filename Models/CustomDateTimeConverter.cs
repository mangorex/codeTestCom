using System;
using Newtonsoft.Json;

namespace codeTestCom.Models
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd/MM/yyyy";

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return DateTime.MinValue;

            string dateString = reader.Value.ToString();
            DateTime result;
            DateTime.TryParseExact(dateString, Format, null, System.Globalization.DateTimeStyles.None, out result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(Format));
        }
    }

}
