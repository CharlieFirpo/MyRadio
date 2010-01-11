using System;
namespace MyRadio.Services
{
    interface IWebRequestHandler
    {
        string AsyncResponseContent { get; }
        string MakeWebRequest(string url);
        void MakeWebRequestAsync(string url, int timeOut);
        event Action<string> AsyncResponseArrived;
        event Action<Exception> Error;
    }
}
