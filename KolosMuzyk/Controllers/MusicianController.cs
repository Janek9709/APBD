using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolosMuzyk.DTO_s.Req;
using KolosMuzyk.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolosMuzyk.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private readonly IMusicDbService _service;

        public MusicianController(IMusicDbService service)
        {
            _service = service;
        }

        [HttpGet("musicians/{id}")]
        public IActionResult GetMuscian(int id)
        {
            try
            {
                var resp = _service.GetMusicianById(id);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return NotFound("brak o podanym ID");
            }
        }

        [HttpPost("musicians")]
        public IActionResult AddMusician(CustomMusician custom)
        {
            try
            {
                _service.AddCustomMusician(custom);
                return Ok("dodano");
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }


    }
}
