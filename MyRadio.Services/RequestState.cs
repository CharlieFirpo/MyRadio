using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MyRadio.Services
{
    /// <summary>
    /// This class stores the State of the request.
    /// </summary>
    public class RequestState
    {
        public readonly int BufferSize { get; private set; }
        public StringBuilder ResponseContent { get; set; }
        public byte[] BufferRead { get; set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public Stream ResponseStream { get; set; }

        public RequestState()
        {
            BufferSize = 1024;
            BufferRead = new byte[BufferSize];
            ResponseContent = new StringBuilder();
            Request = null;
            ResponseStream = null;
        }
    }

}
