using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace MyRadio.Services
{
    public class WebRequestHandler : IWebRequestHandler
    { 
        public event Action<string> AsyncResponseArrived;
        public event Action<Exception> Error;
        public string AsyncResponseContent { get; private set; }
        private ManualResetEvent m_Reset = new ManualResetEvent(false);

        /// <summary>
        /// Makes a simple synchronous "GET" request and returns the response string
        /// </summary>
        /// <param name="url">The Url for the request</param>
        /// <returns>The response string</returns>
        public string MakeWebRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Proxy = null;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        string responseContent = sr.ReadToEnd();
                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void MakeWebRequestAsync(string url, int timeOut)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Proxy = null;

                RequestState state = new RequestState();
                state.Request = request;

                IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);

                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
                    new WaitOrTimerCallback(TimeOutCallback), request, timeOut, true);

                m_Reset.WaitOne();

                state.Response.Close();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void ResponseCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                HttpWebRequest request = state.Request;
                // End the Asynchronous response
                state.Response = (HttpWebResponse)request.EndGetResponse(result);
                Stream responseStream = state.Response.GetResponseStream();
                state.ResponseStream = responseStream;

                // Begin the reading of the contents of the HTML page and print it to the console.
                IAsyncResult readResult = responseStream.BeginRead(state.BufferRead,
                    0, state.BufferSize, new AsyncCallback(ReadCallback), state);

                return;
            }
            catch (Exception ex)
            {
                m_Reset.Set();
                OnError(ex);
            }
        }

        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                int bytesRead = state.ResponseStream.EndRead(result);

                if (bytesRead > 0)
                {
                    state.ResponseContent.Append(Encoding.ASCII.GetString(state.BufferRead, 0, bytesRead));
                    state.ResponseStream.BeginRead(state.BufferRead, 0, state.BufferSize,
                        new AsyncCallback(ReadCallback), state);
                    return;
                }
                else
                {
                    if (state.ResponseContent.Length > 0)
                    {
                        // fill prop, fire event
                        AsyncResponseContent = state.ResponseContent.ToString();
                        state.ResponseStream.Close();
                        m_Reset.Set();
                        OnAsyncResponseArrived(AsyncResponseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void TimeOutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                    request.Abort();
            }
        }

        protected virtual void OnAsyncResponseArrived(string responseContent)
        {
            if (AsyncResponseArrived != null)
                AsyncResponseArrived(responseContent);
        }

        protected virtual void OnError(Exception ex)
        {
            if (Error != null)
                Error(ex);
        }
    }
}
