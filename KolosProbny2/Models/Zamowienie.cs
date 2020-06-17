using System;
using System.Collections.Generic;

namespace KolosProbny2.Models
{
    public partial class Zamowienie
    {
        public Zamowienie()
        {
            ZamowienieWyrobCukierniczy = new HashSet<ZamowienieWyrobCukierniczy>();
        }

        public int IdZamowienia { get; set; }
        public DateTime DataPrzyjecia { get; set; }
        public DateTime? DataRealizacji { get; set; }
        public string Uwagi { get; set; }
        public int KlientIdKlient { get; set; }
        public int PracownikIdPracownik { get; set; }

        public virtual Klient KlientIdKlientNavigation { get; set; }
        public virtual Pracownik PracownikIdPracownikNavigation { get; set; }
        public virtual ICollection<ZamowienieWyrobCukierniczy> ZamowienieWyrobCukierniczy { get; set; }
    }
}
