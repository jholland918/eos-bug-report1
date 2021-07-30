using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp1
{
    public class ItchioOAuthClient
    {
        /// <summary>
        /// The URI of the Authorization Server that will prompt the user to grant authorization to the client id
        /// </summary>
        private readonly string _authorizationEndpoint = "https://itch.io/user/oauth";
        private readonly string _clientId;
        private readonly string _redirectUri;

        /// <summary>
        /// OAuth 2 client to authorize a user with implicit flow.
        /// </summary>
        /// <param name="clientId">The OAuth 2 client id the user will authorize to act on their behalf</param>
        /// <param name="redirectUri">The redirect URI the Authorization Server will send the user back to which will have the auth_token appended to it as a hash(#) value. Usually uses a loopback address and a port for desktop apps (eg http://127.0.0.1:30411/)</param>
        public ItchioOAuthClient(string clientId, string redirectUri)
        {
            _clientId = clientId;
            _redirectUri = redirectUri;
        }

        /// <summary>
        /// Sends user to OAuth Authorization Endpoint
        /// </summary>
        /// <returns>OAuth accessToken</returns>
        public string RequestAuthorization()
        {
            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(_redirectUri);
            http.Start();

            // Creates the OAuth 2.0 authorization request.
            var query = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "scope", "profile:me" },
                { "response_type", "token" },
                { "redirect_uri", _redirectUri }
            };

            var queryString = new StringBuilder();
            foreach (var item in query)
            {
                queryString.AppendFormat("{0}={1}&", Uri.EscapeDataString(item.Key), Uri.EscapeDataString(item.Value));
            }

            string authorizationRequest = $"{_authorizationEndpoint}?{queryString.ToString().TrimEnd('&')}";

            // Opens request in the browser.
            Process.Start(new ProcessStartInfo
            {
                FileName = authorizationRequest,
                UseShellExecute = true
            });

            // Waits for the OAuth authorization response.
            string accessToken = null;
            while (http.IsListening)
            {
                var context = http.GetContext();

                if (context.Request.HttpMethod.ToUpper() == "GET")
                {
                    HandleAuthorizationCallback(context);
                }
                else if (context.Request.HttpMethod.ToUpper() == "POST")
                {
                    // Get access token from POST made in HTML page sent by HandleAuthorizationCallback()
                    accessToken = new StreamReader(context.Request.InputStream).ReadToEnd();
                    break;
                }
            }

            http.Stop();
            http.Close();

            return accessToken;
        }

        /// <summary>
        /// Responds to the Authorization Callback URL by sending an HTML page with JavaScript that will retrieve the access token from the URL hash
        /// </summary>
        private void HandleAuthorizationCallback(HttpListenerContext context)
        {
            string html = $@"
<html>
<head>
<meta http-equiv='refresh' content='10;url=https://google.com'>
<script>
var queryString = window.location.hash.slice(1);
var params = new URLSearchParams(queryString);
var accessToken = params.get('access_token');
const request = new Request('{_redirectUri}', {{ method: 'POST', body: accessToken }});
fetch(request);
</script>
</head>
<body>Please return to the app.</body></html>";

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            var response = context.Response;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
