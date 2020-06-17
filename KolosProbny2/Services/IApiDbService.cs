using KolosProbny2.DTO_s.Req;
using KolosProbny2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolosProbny2.Services
{
    public interface IApiDbService
    {
        IEnumerable<Object> GetAllZamowienie();
        IEnumerable<Object> GetZmowienieNazwisko(string nazwisko);
        void AddNewOrder(int id, CustomOrder customOrder);
    }
}
