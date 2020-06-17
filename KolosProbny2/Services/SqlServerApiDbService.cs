using KolosProbny2.DTO_s.Req;
using KolosProbny2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolosProbny2.Services
{
    public class SqlServerApiDbService : IApiDbService
    {
        private readonly s18313Context _database;

        public SqlServerApiDbService(s18313Context database)
        {
            _database = database;
        }

        public void AddNewOrder(int id, CustomOrder customOrder)
        {
            var checkClient = _database.Klient.Where(e => e.IdKlient == id).Select(e => e.IdKlient).First();

            _database.Database.BeginTransaction();
            try
            {

                var zamowienie = new Zamowienie
                {
                    IdZamowienia = _database.Zamowienie.Select(e => e.IdZamowienia).Max() + 1,
                    DataPrzyjecia = customOrder.dataPrzyjecia,
                    Uwagi = customOrder.uwagi,
                    KlientIdKlient = id,
                    PracownikIdPracownik = 1
                };

                _database.Zamowienie.Add(zamowienie);

                foreach (var elem in customOrder.wyroby)
                {
                    var idWyro = _database.WyrobCukierniczy.Where(e => e.Nazwa == elem.wyrob).Select(e => e.IdWyrobuCukierniczego).First();

                    var wyrC = new ZamowienieWyrobCukierniczy
                    {
                        Ilosc = elem.ilosc,
                        Uwagi = elem.uwagi,
                        IdWyrobuCukierniczego = idWyro,
                        IdZamowienia = zamowienie.IdZamowienia
                    };

                    _database.ZamowienieWyrobCukierniczy.Add(wyrC);
                }

            }
            catch (Exception ex)
            {
                _database.Database.RollbackTransaction();
                throw new Exception("bledny wyrob");
            }
            _database.SaveChanges();
            _database.Database.CommitTransaction();
        }

        public IEnumerable<Object> GetAllZamowienie()
        {
            //Pierwsza opcja z JOIN
            /*
            var resp = _database
                .Klient
                .Join(_database.Zamowienie, klient => klient.IdKlient, zamow => zamow.KlientIdKlient, (klient, zamow) => new { klient, zamow })
                .Join(_database.ZamowienieWyrobCukierniczy, p => p.zamow.IdZamowienia, f => f.IdZamowienia, (p, f) => new { p, f })
                .Join(_database.WyrobCukierniczy, pp => pp.f.IdWyrobuCukierniczego, ff => ff.IdWyrobuCukierniczego, (pp, ff) => new { pp, ff })
                .Select(e => new { Nazwisko = e.pp.p.klient.Nazwisko, Danie = e.ff.Nazwa })
                .ToList();
            */
            //Druga opcja  z Include
            var resp = _database
                .ZamowienieWyrobCukierniczy
                .Include(i => i.IdWyrobuCukierniczegoNavigation)
                .Include(i => i.IdZamowieniaNavigation)
                .Select(e => new { IdZamowienia = e.IdZamowieniaNavigation.IdZamowienia, Nazwa = e.IdWyrobuCukierniczegoNavigation.Nazwa })
                .ToList();

            return resp;
        }

        public IEnumerable<Object> GetZmowienieNazwisko(string nazwisko)
        {
            //Pierwsza opcja z JOIN
            /*
            var client = _database.Klient.Where(e => e.Nazwisko == nazwisko).Select(e => e.IdKlient).First();
            
            var resp = _database
                .Zamowienie
                .Join(_database.ZamowienieWyrobCukierniczy, p => p.IdZamowienia, f => f.IdZamowienia, (p, f) => new { p, f })
                .Join(_database.WyrobCukierniczy, pp => pp.f.IdWyrobuCukierniczego, ff => ff.IdWyrobuCukierniczego, (pp, ff) => new { pp, ff })
                .Where(e => e.pp.p.KlientIdKlient == client)
                .Select( e => new { e.ff.Nazwa })
                .ToList();
            */
            //Druga opcja  z Include
            var resp = _database
                .ZamowienieWyrobCukierniczy
                .Include(i => i.IdWyrobuCukierniczegoNavigation)
                .Include(i => i.IdZamowieniaNavigation)
                .Include(i => i.IdZamowieniaNavigation.KlientIdKlientNavigation)
                .Where(e => e.IdZamowieniaNavigation.KlientIdKlientNavigation.Nazwisko == nazwisko)
                .Select(e => new { Nazwisko = e.IdZamowieniaNavigation.KlientIdKlientNavigation.Nazwisko, Nazwa = e.IdWyrobuCukierniczegoNavigation.Nazwa })
                .ToList();

            if (resp.Count == 0)
                throw new Exception();
            //To niżej nie działa:
            //var resp = _database.("SELECT NAZWA FROM WyrobCukierniczy JOIN Zamowienie_WyrobCukierniczy ON WyrobCukierniczy.IdWyrobuCukierniczego = Zamowienie_WyrobCukierniczy.IdWyrobuCukierniczego JOIN Zamowienie ON Zamowienie_WyrobCukierniczy.IdZamowienia = Zamowienie.IdZamowienia JOIN KLIENT ON Zamowienie.Klient_IdKlient = Klient.IdKlient WHERE KLIENT.NAZWISKO = '{0}'", nazwisko).ToList();

            return resp;
        }
    }
}
