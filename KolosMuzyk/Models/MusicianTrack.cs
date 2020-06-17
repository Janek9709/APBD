using System;
using System.Collections.Generic;

namespace KolosMuzyk.Models
{
    public partial class MusicianTrack
    {
        public int IdMusicianTrack { get; set; }
        public int IdMusician { get; set; }
        public int IdTrack { get; set; }

        public virtual Musician IdMusicianNavigation { get; set; }
        public virtual Track IdTrackNavigation { get; set; }
    }
}
