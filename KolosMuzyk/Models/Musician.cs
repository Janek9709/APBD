using System;
using System.Collections.Generic;

namespace KolosMuzyk.Models
{
    public partial class Musician
    {
        public Musician()
        {
            MusicianTrack = new HashSet<MusicianTrack>();
        }

        public int IdMusician { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public virtual ICollection<MusicianTrack> MusicianTrack { get; set; }
    }
}
