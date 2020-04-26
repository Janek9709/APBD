using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTO_s.Response
{
    public class GetAnimalResponse
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public DateTime AdmissionDate { get; set; }

        public string OwnerLastName { get; set; }
    }
}
