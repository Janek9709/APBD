using Cw3.DTO_s.Requests;
using Cw3.DTO_s.Responses;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentDbService
    {
        public string connectionString = "Data source=db-mssql;Initial Catalog=s18313;Integrated Security=True";
        public IActionResult AddStudent(AddStudentRequest request)
        {
            //logika laczenia z baza i etc.
            if (!ModelState.IsValid)
                return StatusCode(400);

            var idOfStudies = 0;
            var maxIdOfEnrollment = -1;
            DateTime when = DateTime.Today;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {

                connection.Open();
                var transaction = connection.BeginTransaction("addStudent");
                try
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "SELECT IDSTUDY FROM STUDIES WHERE NAME = @studiesR";
                    command.Parameters.AddWithValue("studiesR", request.Studies);

                    var dr = command.ExecuteReader();

                    if (!dr.Read())
                        return StatusCode(400);

                    while (dr.Read())
                        idOfStudies = (int)dr["IDSTUDY"];

                    dr.Close();

                    //drugi punkt, szukanie najwiekszego z enrollment
                    command.CommandText = "SELECT MAX(IDENROLLMENT) AS MAX, STARTDATE FROM ENROLLMENT WHERE SEMESTER = 1 AND IDSTUDY = @idStudiesR GROUP BY STARTDATE";
                    command.Parameters.AddWithValue("idStudiesR", idOfStudies);
                    dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        command.CommandText = "INSERT INTO ENROLLMENT VALUES((SELECT MAX(IDENROLLMENT)+1 FROM ENROLLMENT), 1, @idStudiesR, @dateT);";
                        command.Parameters.AddWithValue("idStudiesR", idOfStudies);
                        command.Parameters.AddWithValue("dateT", DateTime.Today.ToShortDateString());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        when = (DateTime)dr["STARTDATE"];
                        maxIdOfEnrollment = (int)dr["MAX"];
                    }
                    dr.Close();

                    if (maxIdOfEnrollment == -1)
                    {
                        command.CommandText = "SELECT MAX(IDENROLLMENT) AS MAX FROM ENROLLMENT WHERE SEMESTER = 1 AND IDSTUDY = @idStudiesR";
                        command.Parameters.AddWithValue("idStudiesR", idOfStudies);
                        dr = command.ExecuteReader();
                        while (dr.Read())
                            maxIdOfEnrollment = (int)dr["MAX"];

                        dr.Close();
                    }

                    command.CommandText = "SELECT INDEXNUMBER FROM STUDENT WHERE INDEXNUMBER = @index";
                    command.Parameters.AddWithValue("index", request.IndexNumber);
                    dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        return BadRequest("Exception");
                    }

                    dr.Close();

                    command.CommandText = "INSERT INTO STUDENT VALUES(@index2, @fName, @lName, @bDate, @mEnroll);";
                    command.Parameters.AddWithValue("index2", request.IndexNumber);
                    command.Parameters.AddWithValue("fName", request.FirstName);
                    command.Parameters.AddWithValue("lName", request.LastName);
                    command.Parameters.AddWithValue("bDate", request.BirthDate);
                    command.Parameters.AddWithValue("mEnroll", maxIdOfEnrollment);
                    command.ExecuteNonQuery();

                    var returnResponse = new AddStudentResponse
                    {
                        Semester = 1,
                        IndexNumber = request.IndexNumber,
                        StartDate = when
                    };

                    transaction.Commit();
                    return StatusCode(201, returnResponse);
                }
                catch (SqlException exception)
                {
                    transaction.Rollback();
                    return BadRequest("Exception");
                }

            }

        }

        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(400);


            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                var transaction2 = connection.BeginTransaction("promoteStudent");
                try
                {
                    command.Connection = connection;
                    command.Transaction = transaction2;
                    command.CommandText = "SELECT IDSTUDY FROM ENROLLMENT WHERE IDSTUDY = (SELECT IDSTUDY FROM STUDIES WHERE NAME = @studiesR) AND SEMESTER = @semesterR;";
                    command.Parameters.AddWithValue("studiesR", request.Studies);
                    command.Parameters.AddWithValue("semesterR", request.Semester);

                    var dr = command.ExecuteReader();

                    if (!dr.Read())
                        return StatusCode(404);

                    dr.Close();

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "PromoteStudent";
                    command.Parameters.Add(new SqlParameter("@studiesR", request.Studies));
                    command.Parameters.Add(new SqlParameter("@semesterR", request.Semester));

                    dr = command.ExecuteReader();
                    dr.Read();

                    dr.Close();

                    command.CommandText = "SELECT IDSTUDY, SEMESTER, STARTDATE FROM ENROLLMENT WHERE IDSTUDY = (SELECT IDSTUDY FROM STUDIES WHERE NAME = @studiesR) AND SEMESTER = @semesterR2 GROUP BY STARTDATE;";
                    command.Parameters.AddWithValue("studiesR2", request.Studies);
                    command.Parameters.AddWithValue("semesterR2", request.Semester);
                    dr = command.ExecuteReader();
                    dr.Read();

                    var returnRespone = new PromoteStudentResponse
                    {
                        Semester = (int)dr["SEMESTER"],
                        StudyName = (String) request.Studies,
                        StartDate = (DateTime)dr["STARTDATE"]
                    };
                    dr.Close();

                    transaction2.Commit();
                    return StatusCode(202, returnRespone);

                }
                catch (SqlException exception)
                {
                    transaction2.Rollback();
                    return BadRequest("Exception");

                }
            }
        }
    }
}
