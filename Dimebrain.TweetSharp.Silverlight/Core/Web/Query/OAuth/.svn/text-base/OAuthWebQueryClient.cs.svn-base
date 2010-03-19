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
using TweetSharp.Core.Extensions;
using TweetSharp.Core.OAuth;

namespace TweetSharp.Core.Web.Query.OAuth
{
    internal class OAuthWebQueryClient : IWebQueryClient
    {
#if SILVERLIGHT4
        public bool IsOutOfBrowser { get; private set; }
#endif
        private readonly IDictionary<string, string> _headers;

        public OAuthWebQueryClient(IDictionary<string, string> headers, WebParameterCollection parameters,
                                   OAuthParameterHandling parameterHandling, string userAgent)
        {
            _headers = headers;
            Parameters = parameters;
            ParameterHandling = parameterHandling;
            UserAgent = userAgent;
            Method = "GET";
        }

        public OAuthWebQueryClient(IDictionary<string, string> headers, WebParameterCollection parameters,
                                   OAuthParameterHandling parameterHandling, string userAgent, string method)
        {
            _headers = headers;
            Parameters = parameters;
            ParameterHandling = parameterHandling;
            UserAgent = userAgent;
            Method = method;
        }

        public OAuthParameterHandling ParameterHandling { get; private set; }
        public WebParameterCollection Parameters { get; private set; }
        public string UserAgent { get; private set; }
        public string Method { get; private set; }

        #region IWebQueryClient Members

        public WebResponse Response { get; private set; }
        public WebRequest Request { get; private set; }
        public WebCredentials WebCredentials { get; set; }
        public bool KeepAlive { get; set; }

        public WebException Exception { get; set; }
        public string SourceUrl { get; set; }

        public bool UseCompression { get; set; }
        public string ProxyValue { get; set; }
        public TimeSpan? RequestTimeout { get; set; }

        public WebRequest GetWebRequestShim(Uri address)
        {
            var request = (HttpWebRequest) WebRequest.Create(address);

            // NOTE: Copied from WebQueryClient.SetUpRequestForCompatibleProxy()
            // You can't set headers in a GET request in Silverlight,
            // so override to POST in all cases and use the 'X-Twitter-Method'
            // header value to let the proxy know it should be a GET.
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["X-Twitter-Method"] = Method;
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

            if (Parameters != null)
            {
                var hasParameters = address.Query.Contains("?");
                foreach (var parameter in Parameters)
                {
                    address.Query.Then(hasParameters ? "&" : "?");
                    address.Query.Then("{0}={1}".FormatWith(parameter.Name, parameter.Value));
                    hasParameters = true;
                }
            }

            request.Headers["X-Twitter-Query"] = SourceUrl;

            SetAuthorizationHeader(request);

            Request = request;
            return request;
        }

        // Note: Adapted from OAuthWebQuery.SetAuthorizationHeader()

        public WebResponse GetWebResponseShim(WebRequest request, IAsyncResult result)
        {
            try
            {
                Response = request.EndGetResponse(result);
                return Response;
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
            var request = GetWebRequestShim(uri);

            request.BeginGetResponse(BeginGetResponseCompleted, request);
        }

        public void OpenReadAsync(Uri uri, object state)
        {
            var request = GetWebRequestShim(uri);
            var pair = new Pair<WebRequest, object>
                           {
                               First = request,
                               Second = state
                           };

            request.BeginGetResponse(BeginGetResponseCompleted, pair);
        }

        public void CancelAsync()
        {
            Request.Abort();
        }

        #endregion

        protected void SetAuthorizationHeader(WebRequest request)
        {
            var sb = new StringBuilder("OAuth ");

            var parameters = 0;
            foreach (var parameter in Parameters)
            {
                if (parameter.Name.IsNullOrBlank() || parameter.Value.IsNullOrBlank())
                {
                    continue;
                }

                parameters++;
                var format = parameters < Parameters.Count ? "{0}=\"{1}\"," : "{0}=\"{1}\"";
                sb.Append(format.FormatWith(parameter.Name, parameter.Value));
            }

            var authorization = sb.ToString();

            request.Headers["X-Twitter-Auth"] = authorization;
        }

        public void OnOpenReadCompleted(Stream result)
        {
            if (OpenReadCompleted == null)
            {
                return;
            }
            var args = new OpenReadCompletedEventArgs(result);
            OpenReadCompleted(this, args);
        }

        private void BeginGetResponseCompleted(IAsyncResult ar)
        {
            WebRequest request;
            if (ar.AsyncState is WebRequest)
            {
                request = (WebRequest) ar.AsyncState;
            }
            else
            {
                var pair = (Pair<WebRequest, object>) ar.AsyncState;
                request = pair.First;
            }

            Response = request.EndGetResponse(ar);
            var stream = Response.GetResponseStream();
            OnOpenReadCompleted(stream);
        }

        private static WebResponse HandleWebException(WebException ex)
        {
            if (ex.Response != null && ex.Response is HttpWebResponse)
            {
                return ex.Response;
            }

            // not the droids we're looking for
            throw ex;
        }
    }
}