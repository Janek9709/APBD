using Cw10_APBD.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10_APBD.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        private readonly s18313Context _database;

        public SqlServerStudentDbService(s18313Context database)
        {
            _database = database;
        }

        public void ChangeStudent(Student student)
        {
            _database.Update(student);
            _database.SaveChanges();
        }

        public void DeleteStudent(string id)
        {
            _database.Remove(_database.Student.Where(x => x.IndexNumber == id));
            _database.SaveChanges();
        }

        public void EnrollStudent(Student student)
        {
            var check = _database.Enrollment.Any(x => x.IdEnrollment == student.IdEnrollment);
            if (!check)
            {
                var enrollment = new Enrollment
                {
                    IdEnrollment = 1,
                    Semester = 1,
                    IdStudy = 1,
                    StartDate = DateTime.Now
                };

                _database.Add(enrollment);
                _database.SaveChanges();
            }

            _database.Add(student);
            _database.SaveChanges();
        }

        public List<Student> GetAllStudents()
        {
            var resp = _database.Student.ToList();
            return resp;
        }

        public void PromoteStudent(Enrollment enrollment)
        {
            var enrollmentPlus = new Enrollment
            {
                IdEnrollment = enrollment.IdEnrollment+1,
                Semester = enrollment.Semester + 1,
                IdStudy = enrollment.IdStudy,
                StartDate = enrollment.StartDate
            };

            _database.Add(enrollmentPlus);
            _database.SaveChanges();

            var listOfMachingOnes = _database.Student.Where(x => x.IdEnrollment == enrollment.IdEnrollment).ToList();

            foreach(var student in listOfMachingOnes)
            {
                student.IdEnrollment = enrollmentPlus.IdEnrollment;
            }

            _database.SaveChanges();
        }
    }
}
