using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTO_s.Request;
using WebApplication1.DTO_s.Response;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class SqlServerAnimalDbService : IAnimalDbService
    {
        public string connectionString = "Data source=db-mssql;Initial Catalog=s18313;Integrated Security=True";


        public List<GetAnimalResponse> GetAnimals()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand())
                {
                    List<GetAnimalResponse> listOfAnimals = new List<GetAnimalResponse>();

                    command.Connection = connection;
                    command.CommandText = "SELECT NAME, TYPE, ADMISSIONDATE, LASTNAME FROM ANIMAL JOIN OWNER ON ANIMAL.IDOWNER = OWNER.IDOWNER;";
                    //command.Parameters.AddWithValue("givenId", id);
                    connection.Open();

                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var animal = new GetAnimalResponse {
                            Name = dr["NAME"].ToString(),
                            Type = dr["TYPE"].ToString(),
                            AdmissionDate = (DateTime)dr["ADMISSIONDATE"],
                            OwnerLastName = dr["LASTNAME"].ToString()
                        };

                        listOfAnimals.Add(animal);
                    }
                    dr.Close();
                    return listOfAnimals;
                }
            }
            catch (SqlException exception)
            {

                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        public void AddAnimalToDatabase(AddFullAnimalRequest request)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                var transaction = connection.BeginTransaction("AddAnimal");
                try
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO ANIMAL (NAME, TYPE, ADMISSIONDATE, IDOWNER) VALUES (@NAME, @TYPE, @ADMISSIONDATE, @IDOWNER)";
                    command.Parameters.AddWithValue("NAME", request.OneAnimal.Name);
                    command.Parameters.AddWithValue("TYPE", request.OneAnimal.Type);
                    command.Parameters.AddWithValue("ADMISSIONDATE", request.OneAnimal.AdmissionDate);
                    command.Parameters.AddWithValue("IDOWNER", request.OneAnimal.IdOwner);
                    command.ExecuteNonQuery();

                    if (request.Procedures != null)
                    {
                        foreach (var element in request.Procedures)
                        {
                            command.CommandText = "INSERT INTO \"PROCEDURE\" (NAME, DESCRIPTION) VALUES (@NAME23, @DESCRIPTION)";
                            command.Parameters.AddWithValue("NAME23", element.Name);
                            command.Parameters.AddWithValue("DESCRIPTION", element.Description);
                            command.ExecuteNonQuery();

                            command.CommandText = "INSERT INTO PROCEDURE_ANIMAL VALUES ((SELECT MAX(IdProcedure) FROM \"PROCEDURE\"), (SELECT MAX(IdAnimal) FROM ANIMAL), @DATE3)";
                            command.Parameters.AddWithValue("DATE3", DateTime.Today);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                    }

                    transaction.Commit();
                }
                catch (SqlException exception)
                {
                    transaction.Rollback();
                    throw new System.ArgumentException("Parameter cannot be null", "original");
                }
            }

        }
    }
}
