using System;
using Cw10_APBD.Models;
using Cw10_APBD.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw10_APBD.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentsDbService _service;

        public StudentController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpGet]
        IActionResult GetAllStudents()
        {
            try
            {
                var resp = _service.GetAllStudents();
                return Ok(resp);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        IActionResult ChangeStudent(Student student)
        {
            try
            {
                _service.ChangeStudent(student);
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        IActionResult DeleteStudent(string id)
        {
            try
            {
                _service.DeleteStudent(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("enroll")]
        IActionResult EnrollStudent(Student student)
        {
            try
            {
                _service.EnrollStudent(student);
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("promote")]
        IActionResult PromoteStudents(Enrollment enrollment)
        {
            try
            {
                _service.PromoteStudent(enrollment);
                return Ok();
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
    }
}