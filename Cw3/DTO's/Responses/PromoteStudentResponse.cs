﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DTO_s.Responses
{
    public class PromoteStudentResponse
    {
        public int Semester { get; set; }
        public string StudyName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
