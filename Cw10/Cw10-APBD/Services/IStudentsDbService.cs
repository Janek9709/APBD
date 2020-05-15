using Cw10_APBD.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10_APBD.Services
{
    public interface IStudentsDbService
    {
        List<Student> GetAllStudents();
        void ChangeStudent(Student student);
        void DeleteStudent(string id);
        void EnrollStudent(Student student);
        void PromoteStudent(Enrollment enrollment);
    }
}
