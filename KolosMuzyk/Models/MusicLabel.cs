using System;
using System.Collections.Generic;

namespace KolosMuzyk.Models
{
    public partial class MusicLabel
    {
        public MusicLabel()
        {
            Album = new HashSet<Album>();
        }

        public int IdMusicLabel { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Album { get; set; }
    }
}
