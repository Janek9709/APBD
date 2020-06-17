using KolosMuzyk.DTO_s.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolosMuzyk.Services
{
    public interface IMusicDbService
    {
        IEnumerable<object> GetMusicianById(int id);

        void AddCustomMusician(CustomMusician custom);
    }
}
