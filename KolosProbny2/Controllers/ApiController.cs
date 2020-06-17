using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolosProbny2.DTO_s.Req;
using KolosProbny2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolosProbny2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IApiDbService _service;

        public ApiController(IApiDbService service)
        {
            _service = service;
        }

        [HttpPost("clients/{id}/orders")]
        public IActionResult PostOrder(int id, CustomOrder customOrder)
        {
            try
            {
                _service.AddNewOrder(id, customOrder);
                return Ok("dodano");
            }
            catch(Exception ex)
            {
                return NotFound("brak wyrobu");
            }
        }


        [HttpGet("orders")]
        public IActionResult GetOrder(string surname)
        {
            try
            {
                if(surname == null)
                {
                    var resp = _service.GetAllZamowienie();
                    return Ok(resp);
                }
                else
                {
                    var resp = _service.GetZmowienieNazwisko(surname);
                    return Ok(resp);
                }
           }
            catch(Exception ex)
            {
                return NotFound("brak o takim nazwisku");
            }
        }
    }
}
