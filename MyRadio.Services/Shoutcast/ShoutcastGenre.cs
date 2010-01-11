using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyRadio.Services.Shoutcast
{
    public class ShoutcastGenre
    {
        /// <summary>
        ///  The genre name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        ///  Number of Stations -> "Stations: number"
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///  The link to the station list
        /// </summary>
        public Uri Link { get; set; }
    }

    public class ShoutcastGenreComparer : IEqualityComparer<ShoutcastGenre>
    {

        #region IEqualityComparer<ShoutcastGenre> Members

        public bool Equals(ShoutcastGenre x, ShoutcastGenre y)
        {
            return x.Title.Equals(y.Title);
        }

        public int GetHashCode(ShoutcastGenre obj)
        {
            return obj.Title.ToLower().GetHashCode();
        }

        #endregion
    }
}
