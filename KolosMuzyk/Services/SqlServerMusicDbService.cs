using KolosMuzyk.DTO_s.Req;
using KolosMuzyk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolosMuzyk.Services
{
    public class SqlServerMusicDbService : IMusicDbService
    {
        private readonly s18313Context _database;

        public SqlServerMusicDbService(s18313Context database)
        {
            _database = database;
        }

        public void AddCustomMusician(CustomMusician custom)
        {
            _database.Database.BeginTransaction();
            try
            {
                var muz = new Musician
                {
                    IdMusician = _database.Musician.Select(e => e.IdMusician).Max() + 1,
                    FirstName = custom.firstName,
                    LastName = custom.lastName,
                    Nickname = custom.nickname
                };

                _database.Musician.Add(muz);

                foreach (var cs in custom.track)
                {
                    string check = _database.Track.Where(e => e.Duration == cs.duration && e.TrackName == cs.trackName).Select(e => e.TrackName).FirstOrDefault();

                    if (check == null)
                    {
                        var tr = new Track
                        {
                            IdTrack = _database.Track.Select(e => e.IdTrack).Max() + 1,
                            TrackName = cs.trackName,
                            Duration = cs.duration,
                            IdMusicAlbum = 1
                        };

                        _database.Track.Add(tr);

                        var mt = new MusicianTrack
                        {
                            IdMusicianTrack = _database.MusicianTrack.Select(e => e.IdMusicianTrack).Max() + 1,
                            IdTrack = _database.Track.Select(e => e.IdTrack).Max(),
                            IdMusician = _database.Musician.Select(e => e.IdMusician).Max()
                        };

                        _database.MusicianTrack.Add(mt);
                    }
                    else
                    {
                        var mt = new MusicianTrack
                        {
                            IdMusicianTrack = _database.MusicianTrack.Select(e => e.IdMusicianTrack).Max() + 1,
                            IdTrack = _database.Track.Where(e => e.Duration == cs.duration && e.TrackName == cs.trackName).Select(e => e.IdTrack).FirstOrDefault(),
                            IdMusician = _database.Musician.Select(e => e.IdMusician).Max()
                        };

                        _database.MusicianTrack.Add(mt);
                    }
                }
            }
            catch (Exception ex)
            {
                _database.Database.RollbackTransaction();
                throw new Exception("cos poszło nie tak xD");
            }

            _database.SaveChanges();
            _database.Database.CommitTransaction();


        }

        public IEnumerable<object> GetMusicianById(int id)
        {
            var resp = _database.MusicianTrack
                .Include(i => i.IdMusicianNavigation)
                .Include(i => i.IdTrackNavigation)
                .Where(e => e.IdMusician == id)
                .Select(e => new
                {
                    FirstName = e.IdMusicianNavigation.FirstName,
                    LastName = e.IdMusicianNavigation.LastName,
                    Nickname = e.IdMusicianNavigation.Nickname,
                    Muscian = _database.MusicianTrack
                    .Include(i => i.IdTrackNavigation)
                    .Where(e => e.IdMusician == id)
                    .Select(e => e.IdTrackNavigation.TrackName)
                    .ToList()
                })
                .ToList();

            return resp;
        }
    }
}
