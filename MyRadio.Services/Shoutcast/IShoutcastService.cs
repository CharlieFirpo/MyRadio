using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyRadio.Services.Shoutcast
{
    public interface IShoutcastService
    {
        List<ShoutcastGenre> GetGenres();
        List<ShoutcastStation> GetStationsByGenre(string genreName);
        event Action<ShoutcastGenre> CustomGenreAdded;
    }
}
