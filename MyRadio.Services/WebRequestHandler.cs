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

        /// <summary>
        /// Makes a simple synchronous "GET" request and returns the response string
        /// </summary>
        /// <param name="url">The Url for the request</param>
        /// <returns>The response string</returns>
        public string MakeWebRequest(string url)
        {
            try
            {
                string responseContent;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Proxy = null;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(responseStream))
                            responseContent = sr.ReadToEnd();
                    }
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                OnError(ex);
                return String.Empty;
            }
        }

        /// <summary>
        /// Makes an asynchronous "GET" request.
        /// Use "AsyncResponseArrived" event to receive the response data.
        /// </summary>
        /// <param name="url">The Url for the request</param>
        /// <param name="timeOut">Timeout value in milliseconds</param>
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
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Callback to process the WebResponse
        /// </summary>
        /// <param name="result">RequestState</param>
        private void ResponseCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                HttpWebRequest request = state.Request;
                // End the Asynchronous response and get the actual resonse object
                state.Response = (HttpWebResponse)request.EndGetResponse(result);
                Stream responseStream = state.Response.GetResponseStream();
                state.ResponseStream = responseStream;

                // Begin async reading of the contents
                IAsyncResult readResult = responseStream.BeginRead(state.BufferRead,
                    0, state.BufferSize, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception ex)
            {
                RequestState state = (RequestState)result.AsyncState;
                if (state.Response != null)
                    state.Response.Close();
                OnError(ex);
            }
        }

        /// <summary>
        /// Callback to read the response stream
        /// </summary>
        /// <param name="result">RequestState</param>
        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                // determine how many bytes have been read
                int bytesRead = state.ResponseStream.EndRead(result);

                if (bytesRead > 0) // stream has not reached the end yet
                {
                    // append the read data to the ResponseContent and...
                    state.ResponseContent.Append(Encoding.ASCII.GetString(state.BufferRead, 0, bytesRead));
                    // ...read the next piece of data from the stream
                    state.ResponseStream.BeginRead(state.BufferRead, 0, state.BufferSize,
                        new AsyncCallback(ReadCallback), state);
                }
                else // end of the stream reached
                {
                    if (state.ResponseContent.Length > 0)
                    {
                        // fill prop, fire event
                        AsyncResponseContent = state.ResponseContent.ToString();
                        // close the stream and the response
                        state.ResponseStream.Close();
                        state.Response.Close();
                        OnAsyncResponseArrived(AsyncResponseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                RequestState state = (RequestState)result.AsyncState;
                if (state.Response != null)
                    state.Response.Close();
                OnError(ex);
            }
        }

        /// <summary>
        /// Callback to abort the WebRequest when triggered
        /// </summary>
        /// <param name="state">WebRequest</param>
        /// <param name="timedOut"></param>
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
