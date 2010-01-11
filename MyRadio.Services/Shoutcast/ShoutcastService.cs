using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace MyRadio.Services.Shoutcast
{
    public class ShoutcastService  : IShoutcastService
    {
        public event Action<ShoutcastGenre> CustomGenreAdded;

        private readonly string m_ShoutcastUrl = "http://www.shoutcast.com";
        private readonly string m_DefaultTuneinBaseUrl = "http://www.shoutcast.com/sbin/tunein-station.pls";
        private readonly string m_GenresUrl = "http://yp.shoutcast.com/sbin/newxml.phtml";
        private readonly string m_StationsByGenreUrl = "http://yp.shoutcast.com/sbin/newxml.phtml?genre=";

        private List<ShoutcastGenre> m_Genres = new List<ShoutcastGenre>();

        /// <summary>
        /// Retrieves the genre list from the Shoutcast server
        /// </summary>
        /// <returns>A List of ShoutcastGenre objects</returns>
        public List<ShoutcastGenre> GetGenres()
        {
            WebRequestHandler wrh = new WebRequestHandler();
            string response = wrh.MakeWebRequest(m_GenresUrl + "?rss=1");
            m_Genres = ParseGenres(response);
            return m_Genres;
        }

        /// <summary>
        /// Parses the XML/RSS response from the WebRequest
        /// and returns a list of genres
        /// </summary>
        /// <param name="xmlString">The WebRequest response</param>
        /// <returns>A List of ShoutcastGenre objects</returns>
        private List<ShoutcastGenre> ParseGenres(string xmlString)
        {
            List<ShoutcastGenre> genres = new List<ShoutcastGenre>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNodeList items = doc.GetElementsByTagName("item");
            foreach (XmlNode node in items)
            {
                if (!node.HasChildNodes)
                    continue;

                ShoutcastGenre genre = new ShoutcastGenre();

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    string nodeName = childNode.Name.Trim().ToLower();
                    string innerText = childNode.InnerText;
                    if (nodeName == "title")
                        genre.Title = innerText;
                    else if (nodeName == "description")
                        genre.Description = innerText;
                    else if (nodeName == "link")
                        genre.Link = new Uri(innerText);
                }

                genres.Add(genre);
            }

            return genres;
        }

        /// <summary>
        /// Retrieves a list of all Shoutcast stations tagged with
        /// the given genre name
        /// </summary>
        /// <param name="genre">The name of the genre to search for</param>
        /// <returns>A List of ShoutcastStation objects</returns>
        public List<ShoutcastStation> GetStationsByGenre(string genreName)
        {
            WebRequestHandler wrh = new WebRequestHandler();
            string response = wrh.MakeWebRequest(m_StationsByGenreUrl + genreName);
            return ParseStations(response);
        }

        /// <summary>
        /// Retrieves a list of all Shoutcast stations tagged with
        /// the given genre name
        /// </summary>
        /// <param name="genre">The ShoutcastGenre to search for</param>
        /// <returns>A List of ShoutcastStation objects</returns>
        public List<ShoutcastStation> GetStationsByGenre(ShoutcastGenre genre)
        {
            return GetStationsByGenre(genre.Title);
        }

        /// <summary>
        /// Parses the XML response from the WebRequest
        /// and returns a list of stations
        /// </summary>
        /// <param name="xmlString">The WebRequest response</param>
        /// <returns>A List of ShoutcastStation objects</returns>
        private List<ShoutcastStation> ParseStations(string xmlString)
        {
            List<ShoutcastStation> stations = new List<ShoutcastStation>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            string tuneinBase = GetTuneinBase(doc);

            XmlNodeList stationNodes = doc.GetElementsByTagName("station");
            foreach (XmlNode node in stationNodes)
            {
                ShoutcastStation station = new ShoutcastStation();
                foreach (XmlAttribute attr in node.Attributes)
                {
                    switch (attr.Name.Trim().ToLower())
                    {
                        case "br":
                            int bitrate;
                            int.TryParse(attr.Value, out bitrate);
                            station.Bitrate = bitrate;
                            break;
                        case "name":
                            station.Name = attr.Value;
                            break;
                        case "mt":
                            station.MimeType = attr.Value;
                            break;
                        case "id":
                            int id;
                            int.TryParse(attr.Value, out id);
                            station.ID = id;
                            station.Link = new Uri(tuneinBase + "?id=" + id);
                            break;
                        case "genre":
                            station.Genres = CreateStationGenreList(attr.Value);
                            break;
                        case "ct":
                            station.CurrentTrack = attr.Value;
                            break;
                        case "lc":
                            int listeners;
                            int.TryParse(attr.Value, out listeners);
                            station.Listeners = listeners;
                            break;
                    }       
                }

            }

            return stations;
        }

        /// <summary>
        /// Generates a list of genres for a specific station
        /// </summary>
        /// <param name="genreString">The string containing the genres of a station</param>
        /// <returns>A List of ShoutcastStation objects</returns>
        private List<ShoutcastGenre> CreateStationGenreList(string genreString)
        {
            List<ShoutcastGenre> genres = new List<ShoutcastGenre>();

            string[] genresArr = genreString.Split(' ');
            foreach (string g in genresArr)
            {
                ShoutcastGenre genre = VerifyGenre(g);
                if (genre != null)
                    genres.Add(genre);
            }

            return genres;
        }

        /// <summary>
        /// Verifies if a genre is valid or already in the genre list
        /// and returns a ShoutcastGenre object if so
        /// </summary>
        /// <param name="genre">The name of the genre</param>
        /// <returns>Valid: A ShoutcastGenre; Invalid: null</returns>
        private ShoutcastGenre VerifyGenre(string genre)
        {
            if (genre.Length >= 3)
            {
                if (m_Genres.Count > 0)
                {
                    ShoutcastGenre fromList = m_Genres.Find(g => (g.Title.Equals(genre)));
                    if (fromList != null)
                        return fromList;

                    return CreateNewShoutcastGenre(genre);
                }

                return CreateNewShoutcastGenre(genre);
            }
            return null;
        }

        /// <summary>
        /// Creates a new ShoutcastGenre object from a genre name
        /// </summary>
        /// <param name="genre">The genre name</param>
        /// <returns>A ShoutcastGenre object</returns>
        private ShoutcastGenre CreateNewShoutcastGenre(string genre)
        {
            ShoutcastGenre scGenre = new ShoutcastGenre();
            scGenre.Title = genre;
            scGenre.Link = new Uri(m_StationsByGenreUrl + genre);
            scGenre.Description = String.Empty;

            m_Genres.Add(scGenre);
            OnCustomGenreAdded(scGenre);

            return scGenre;
        }

        /// <summary>
        /// Gets the base Uri segment for the playlists
        /// </summary>
        /// <param name="doc">The Stationlist XmlDocument</param>
        /// <returns>The base Url for the playlists</returns>
        private string GetTuneinBase(XmlDocument doc)
        {
            try
            {
                var tiBase = doc.GetElementsByTagName("tunein");
                if (tiBase.Count >= 0)
                {
                    var tiBaseUrl = tiBase[0].Attributes["base"];
                    if (tiBaseUrl != null)
                        return m_ShoutcastUrl + tiBaseUrl.Value;
                    return m_DefaultTuneinBaseUrl;
                }

                return m_DefaultTuneinBaseUrl;
            }
            catch (Exception)
            {
                return m_DefaultTuneinBaseUrl;
            }
        }

        /// <summary>
        /// Notify all subscribers to the CustomGenreAdded event
        /// </summary>
        /// <param name="genre"></param>
        protected virtual void OnCustomGenreAdded(ShoutcastGenre genre)
        {
            if (CustomGenreAdded != null)
                CustomGenreAdded(genre);
        }
    }
}
