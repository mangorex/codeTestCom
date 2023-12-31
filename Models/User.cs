﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace codeTestCom.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Dni { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int LoyaltyPoints { get; set; }
        public EnumSex Sex { get; set; }

        public User()
        {
            this.LoyaltyPoints = 0;
        }

        public User(string name, string surname, string dni, int age, EnumSex sex)
        {
            this.Name = name;
            this.Surname = surname;
            this.Dni = dni;
            this.Age = age;
            this.Sex = sex;
            this.PartitionKey = sex.ToString();
            this.LoyaltyPoints = 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumSex
    {
        Male,
        Female,
        Other
    }
}