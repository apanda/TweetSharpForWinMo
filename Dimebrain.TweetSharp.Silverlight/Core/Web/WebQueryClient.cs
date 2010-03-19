#region License

// TweetSharp
// Copyright (c) 2010 Daniel Crenna and Jason Diller
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using TweetSharp.Core.Extensions;
using System.Windows.Threading;
using System.Threading;

namespace TweetSharp.Core.Web
{
    public class WebQueryClient : IWebQueryClient
    {
        private readonly IDictionary<string, string> _headers;

        public WebQueryClient(IDictionary<string, string> headers, WebParameterCollection parameters, string userAgent,
                              bool useTransparentProxy)
        {
            _headers = headers;
            Parameters = parameters;
            UserAgent = userAgent;
            UseTransparentProxy = useTransparentProxy;
        }

        public WebParameterCollection Parameters { get; private set; }
        public string UserAgent { get; private set; }
        public bool UseTransparentProxy { get; private set; }

        #region IWebQueryClient Members

        public WebResponse Response { get; private set; }
        public WebRequest Request { get; private set; }
        public WebCredentials WebCredentials { get; set; }
        public WebException Exception { get; set; }

        public bool UseCompression { get; set; }
        public string ProxyValue { get; set; }
        public string SourceUrl { get; set; }
        public TimeSpan? RequestTimeout { get; set; }
        public bool KeepAlive { get; set; }

#if SILVERLIGHT4
        public bool IsOutOfBrowser { get; private set; }
#endif

        public WebRequest GetWebRequestShim(Uri address)
        {
            var request = (HttpWebRequest) WebRequest.Create(address);

            IsOutOfBrowser = true;                

            Request = request;
            
            //client.UseDefaultCredentials = false;
            //client.
            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    
                    request.Headers[header.Key] = header.Value;
                }
            }

            if (WebCredentials != null)
            {
                var credentials = WebCredentials;

                //client.Headers["Authorization"] = WebExtensions.ToAuthorizationHeader(credentials.Username, credentials.Password);
                request.Credentials = new NetworkCredential(credentials.Username, credentials.Password);
            }

            // TODO: Evaluate how to add GZIP to these calls with Accept

            if (!UserAgent.IsNullOrBlank())
            {
                request.Headers["UserAgent"] = UserAgent;
            }

            //return client;



            return request;
        }

        public WebResponse GetWebResponseShim(WebRequest request, IAsyncResult result)
        {
            try
            {
                var response = request.EndGetResponse(result);
                Response = response;
                return response;
            }
            catch (WebException ex)
            {
                Exception = ex;
                return HandleWebException(ex);
            }
        }

        public event EventHandler<OpenReadCompletedEventArgs> OpenReadCompleted;

        public void OpenReadAsync(Uri uri)
        {
            var request = (HttpWebRequest) GetWebRequestShim(uri);

#if SILVERLIGHT4
            /*if (IsOutOfBrowser)
            {
                var client = GetOutOfBrowserClient();
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(uri);
                return;
            }*/
#endif
            DispatcherTimer timer = new DispatcherTimer();
            Dispatcher dispatch = System.Windows.Deployment.Current.Dispatcher;
            switch (request.Method)
            {
                case "GET":
                    request.BeginGetResponse(
                        (ar) => {
                            if (RequestTimeout != null)
                            {
                                dispatch.BeginInvoke(() =>
                                {
                                    timer.Stop();
                                });
                            }
                            ResponseStreamCallback(ar); }, request);
                    break;
                case "POST":
                    request.BeginGetRequestStream((ar) => { timer.Stop(); ResponseStreamCallback(ar); }, request);
                    break;
            }
            // temp
            if (RequestTimeout != null)
            {
                timer.Tick += new EventHandler((s, e) =>
                {
                    request.Abort();
                    WebException webEx = new WebException("Network timeout", WebExceptionStatus.Timeout);
                    //OnOpenReadCompleted(null, webEx);
                    timer.Stop();

                });
                timer.Interval = new TimeSpan(0, 1, 30);
                timer.Start();
            }

        }

        public void OpenReadAsync(Uri uri, object state)
        {
            var request = GetWebRequestShim(uri);

            var pair = new Pair<WebRequest, object>
                           {
                               First = request,
                               Second = state
                           };
#if SILVERLIGHT4
            if(IsOutOfBrowser)
            {
                var client = GetOutOfBrowserClient();
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(uri, state); 
                return;
            }
#endif
            // A stream is always used because POST is required
            request.BeginGetRequestStream(RequestStreamCallback, pair);
        }

#if SILVERLIGHT4
        private void client_OpenReadCompleted(object sender, System.Net.OpenReadCompletedEventArgs e)
        {
         	var stream = e.Result;
            var state = e.UserState;
            
            OnOpenReadCompleted(stream, state);           
        }
#endif

        public virtual void CancelAsync()
        {
            Request.Abort();
        }

        #endregion

        public virtual void OnOpenReadCompleted(Stream stream, object state)
        {
            if (OpenReadCompleted == null)
            {
                return;
            }
            var args = new OpenReadCompletedEventArgs(stream, state);
            OpenReadCompleted(this, args);
        }

        private void RequestStreamCallback(IAsyncResult ar)
        {
            WebRequest request;
            if (ar.AsyncState is WebRequest)
            {
                request = (HttpWebRequest) ar.AsyncState;
            }
            else
            {
                var pair = (Pair<WebRequest, object>) ar.AsyncState;
                request = pair.First;
            }

            var url = request.RequestUri.ToString();
            var content = Encoding.UTF8.GetBytes(url);

            using (var stream = request.EndGetRequestStream(ar))
            {
                stream.Write(content, 0, content.Length);
                stream.Close();

                request.BeginGetResponse(ResponseStreamCallback, ar.AsyncState);
            }
        }

        private void ResponseStreamCallback(IAsyncResult ar)
        {
            HttpWebRequest request;
            if (ar.AsyncState is WebRequest)
            {
                request = (HttpWebRequest) ar.AsyncState;
            }
            else
            {
                var pair = (Pair<WebRequest, object>) ar.AsyncState;
                request = (HttpWebRequest) pair.First;
            }

            try
            {
                var response = request.EndGetResponse(ar);
                var stream = response.GetResponseStream();

                Response = response;
                var state = ar.AsyncState is Pair<WebRequest, object>
                                ? ((Pair<WebRequest, object>) ar.AsyncState).
                                      Second
                                : null;

                OnOpenReadCompleted(stream, state);
            }
            catch (WebException ex)
            {
                Exception = ex;
                Response = HandleWebException(ex);
                OnOpenReadCompleted(null, ex);
                return;
            }
        }

        private static WebResponse HandleWebException(WebException ex)
        {
            if (ex.Response != null && ex.Response is HttpWebResponse)
            {
                return ex.Response;
            }
            if (ex.Status == WebExceptionStatus.RequestCanceled || ex.Status == WebExceptionStatus.Timeout)
            {
                return ex.Response;
            }

            // not the droids we're looking for
            throw ex;
        }

        protected void SetUpRequestForCompatibleProxy(WebRequest request)
        {
            // You can't set headers in a GET request in Silverlight,
            // so override to POST in all cases and use the 'X-Twitter-Method'
            // header value to let the proxy know it should be a GET.
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!UseTransparentProxy)
            {
                // Proxy not used
                return;
            }

            request.Headers["X-Twitter-Method"] = "GET";
            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            if (UseCompression)
            {
                request.Headers["X-Twitter-Accept"] = "gzip,deflate";
            }

            if (WebCredentials != null)
            {
                var credentials = WebCredentials;
                request.Headers["X-Twitter-Auth"] = WebExtensions.ToAuthorizationHeader(credentials.Username,
                                                                                        credentials.Password);
            }

            if (!UserAgent.IsNullOrBlank())
            {
                request.Headers["X-Twitter-Agent"] = UserAgent;
            }

            request.Headers["X-Twitter-Query"] = SourceUrl;
        }

#if SILVERLIGHT4
        public WebClient GetOutOfBrowserClient()
        {
            var client = new WebClient();
            //client.UseDefaultCredentials = false;
            //client.
            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    client.Headers[header.Key] = header.Value;
                }
            }

            if (WebCredentials != null)
            {
                var credentials = WebCredentials;
                
                //client.Headers["Authorization"] = WebExtensions.ToAuthorizationHeader(credentials.Username, credentials.Password);
                client.Credentials = new NetworkCredential(credentials.Username, credentials.Password);
            }

            // TODO: Evaluate how to add GZIP to these calls with Accept

            if (!UserAgent.IsNullOrBlank())
            {
                client.Headers["UserAgent"] = UserAgent;
            }

            return client;
        }
#endif
    }
}
