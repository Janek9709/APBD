using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DTO_s.Responses
{
    public class AddStudentResponse
    {
        public int Semester { get; set; }
        public string IndexNumber { get; set; }
        public DateTime StartDate { get; set; }
    }
}
