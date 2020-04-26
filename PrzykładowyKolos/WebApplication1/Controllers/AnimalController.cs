using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO_s.Request;
using WebApplication1.DTO_s.Response;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private IAnimalDbService _service;

        public AnimalController(IAnimalDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAnimals([FromQuery]string sortBy)
        {
            List<GetAnimalResponse> sortedList;

            try
            {
                sortedList = _service.GetAnimals();
            }
            catch(Exception ex)
            {
                return StatusCode(400);
            }

            if (sortBy == null)
                sortBy = "";

            sortBy = sortBy.ToLower();

            IEnumerable<GetAnimalResponse> sortedList2;

            if (sortBy == "name")
                sortedList2 = (IEnumerable<GetAnimalResponse>)sortedList.OrderByDescending(x => x.Name);
            else if (sortBy == "type")
                sortedList2 = (IEnumerable<GetAnimalResponse>)sortedList.OrderBy(x => x.Type);
            else if (sortBy == "admissiondate")
                sortedList2 = (IEnumerable<GetAnimalResponse>)sortedList.OrderBy(x => x.AdmissionDate.Year);
            else if (sortBy == "lastename")
                sortedList2 = (IEnumerable<GetAnimalResponse>)sortedList.OrderBy(x => x.OwnerLastName);
            else if (sortBy == "")
                sortedList2 = (IEnumerable<GetAnimalResponse>)sortedList.OrderByDescending(x => x.AdmissionDate.Year);
            else
                return StatusCode(400);



            //return Ok(sortedList[0].AdmissionDate.Year + " "+ sortedList[1].AdmissionDate.Year);
            return Ok(sortedList2.Select(x => new { x.Name, x.Type, x.AdmissionDate, x.OwnerLastName }));
        }

        [HttpPost]
        public IActionResult AddAnimal(AddFullAnimalRequest request)
        {
            try
            {
                _service.AddAnimalToDatabase(request);
            }
            catch (Exception ex)
            {
               return StatusCode(404);
            }

            return Ok("Dodano");

        }


    }
}