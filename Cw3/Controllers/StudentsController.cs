using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public string connectionString = "Data source=db-mssql;Initial Catalog=s18313;Integrated Security=True";

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet()]
        public IActionResult GetStudents()
        {
            //string orderBy
            //return "Nowak, Kowlaski";
            //return $"Nowak, Kowalski, Iksinski sortowanie={orderBy}";
            //return Ok(_dbService.GetStudents());
            List<Student> listOfStudents = new List<Student>();

            using(var client = new SqlConnection(connectionString))
            {
                using(var sqlInstrucion = new SqlCommand())
                {
                    sqlInstrucion.Connection = client;
                    sqlInstrucion.CommandText = "SELECT STUDENT.FIRSTNAME, STUDENT.LASTNAME, STUDENT.BIRTHDATE, STUDIES.NAME, ENROLLMENT.SEMESTER FROM STUDENT JOIN ENROLLMENT ON STUDENT.IDENROLLMENT = ENROLLMENT.IDENROLLMENT JOIN STUDIES ON ENROLLMENT.IDSTUDY = STUDIES.IDSTUDY;";

                    client.Open();
                    var dr = sqlInstrucion.ExecuteReader();
                    while (dr.Read())
                    {
                        listOfStudents.Add(new Student
                        {
                            FirstName = dr["FIRSTNAME"].ToString(),
                            LastName = dr["LASTNAME"].ToString(),
                            BirthDate = Convert.ToDateTime(dr["BIRTHDATE"].ToString()),
                            Semester = Convert.ToInt32(dr["SEMESTER"].ToString()),
                            NameOfStudies = dr["NAME"].ToString()
                        });
                    }
                }
            }
            return Ok(listOfStudents);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            /*
            if(id == 1)
            {
                return Ok("Kowalski");
            }
            else if(id == 2)
            {
                return Ok("Nowak");
            }

            return NotFound("Nie znaleziono studenta");
            */
            List<Enrollment> listOfEnrollments = new List<Enrollment>();

            using (var client = new SqlConnection(connectionString))
            {
                using (var sqlInstrucion = new SqlCommand())
                {
                    sqlInstrucion.Connection = client;
                    sqlInstrucion.CommandText = "SELECT ENROLLMENT.SEMESTER, ENROLLMENT.STARTDATE, STUDIES.NAME FROM ENROLLMENT JOIN STUDENT ON ENROLLMENT.IDENROLLMENT = STUDENT.IDENROLLMENT JOIN STUDIES ON ENROLLMENT.IDSTUDY = STUDIES.IDSTUDY WHERE STUDENT.INDEXNUMBER = @givenId;";
                    sqlInstrucion.Parameters.AddWithValue("givenId", id);

                    client.Open();
                    var dr = sqlInstrucion.ExecuteReader();
                    while (dr.Read())
                    {
                        listOfEnrollments.Add(new Enrollment
                        {
                            Semester = Convert.ToInt32(dr["SEMESTER"].ToString()),
                            StudyName = dr["NAME"].ToString(),
                            StartDate = Convert.ToDateTime(dr["STARTDATE"].ToString())
                        });
                    }
                }
            }

            return Ok(listOfEnrollments);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id)
        {
            //co tu dodac bez bazy danych?
            return Ok("Aktualizacja dokonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            //co tu dodac bez bazy danych?
            return Ok("Usuwanie ukonczone");
        }
    }
}