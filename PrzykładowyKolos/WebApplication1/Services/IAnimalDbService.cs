using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTO_s.Request;
using WebApplication1.DTO_s.Response;

namespace WebApplication1.Services
{
    public interface IAnimalDbService
    {
        public void AddAnimalToDatabase(AddFullAnimalRequest request);
        public List<GetAnimalResponse> GetAnimals();
    }
}
