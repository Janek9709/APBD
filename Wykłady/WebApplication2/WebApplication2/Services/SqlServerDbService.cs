using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Services
{
    public class SqlServerDbService : IDbService
    {
        public void CheckIndexNumber(string index)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand com = new SqlCommand();


        }

        //...
    }
}
