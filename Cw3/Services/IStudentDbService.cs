using Cw3.DTO_s.Requests;
using Cw3.DTO_s.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Services
{
    public interface IStudentDbService
    {
        public IActionResult AddStudent(AddStudentRequest request);

        public IActionResult PromoteStudent(PromoteStudentRequest request);

        public bool GetStudent(string id);

        public bool CheckLoginRequest(LoginRequestDto request);

        public bool WriteToken(Guid guid, string request);

        public TokenResponse CheckToken(string incomingToken);
    }
}
