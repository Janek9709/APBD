using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.DTO_s.Request
{
    public class AddFullAnimalRequest
    {
        [Required]
        [JsonPropertyName("Animal")]
        public Animal OneAnimal { get; set; }

        public List<Procedure> Procedures { get; set; }
    }
}
