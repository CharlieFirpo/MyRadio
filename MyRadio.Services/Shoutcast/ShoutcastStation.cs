using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyRadio.Services.Shoutcast
{
    public class ShoutcastStation
    {
        /// <summary>
        /// The station name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The currently playing track
        /// </summary>
        public string CurrentTrack { get; set; }
        /// <summary>
        ///  List of genres the station belongs to
        /// </summary>
        public List<ShoutcastGenre> Genres { get; set; }
        /// <summary>
        /// The link to the playlist
        /// </summary>
        public Uri Link { get; set; }
        /// <summary>
        /// The station ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The bitrate
        /// </summary>
        public int Bitrate { get; set; }
        /// <summary>
        /// The current listener count
        /// </summary>
        public int Listeners { get; set; }
        /// <summary>
        /// MIME type, determines the codec used.
        /// [Audio/mpeg for MP3 and audio/aacp for aacPlus] 
        /// </summary>
        public string MimeType { get; set; }

    }
}
