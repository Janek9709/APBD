using System;
using System.Collections.Generic;

namespace KolosProbny2.Models
{
    public partial class Pracownik
    {
        public Pracownik()
        {
            Zamowienie = new HashSet<Zamowienie>();
        }

        public int IdPracownik { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public virtual ICollection<Zamowienie> Zamowienie { get; set; }
    }
}
