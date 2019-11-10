using Bramf.Extensions;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bramf.Http
{
    /// <summary>
    /// Construct web requests easily
    /// </summary>
    public static class HttpRequest
    {
        #region Creation Methods

        /// <summary>
        /// Starts building a new HTTP request to an url
        /// </summary>
        /// <param name="endpoint">The url to make the request to</param>
        /// <param name="httpMethod">The Http method to use</param>
        public static HttpRequestBuilder MakeNew(string endpoint, HttpMethod httpMethod)
            => new HttpRequestBuilder(endpoint, httpMethod);

        /// <summary>
        /// Starts building a new HTTP request using the <see cref="HttpMethod.GET"/> method
        /// </summary>
        /// <param name="endpoint">The url to make the request to</param>
        public static HttpRequestBuilder MakeGet(string endpoint)
            => new HttpRequestBuilder(endpoint, HttpMethod.GET);

        /// <summary>
        /// Starts building a new HTTP request using the <see cref="HttpMethod.POST"/> method
        /// </summary>
        /// <param name="endpoint">The url to make the request to</param>
        public static HttpRequestBuilder MakePost(string endpoint)
            => new HttpRequestBuilder(endpoint, HttpMethod.POST);

        /// <summary>
        /// Starts building a new HTTP request using the <see cref="HttpMethod.PUT"/> method
        /// </summary>
        /// <param name="endpoint">The url to make the request to</param>
        public static HttpRequestBuilder MakePut(string endpoint)
            => new HttpRequestBuilder(endpoint, HttpMethod.PUT);

        #endregion

        #region Send Methods

        /// <summary>
        /// Sends the HTTP request to the server and waits for a response
        /// </summary>
        public static async Task<HttpResponse> SendAsync(this HttpRequestBuilder builder)
        {
            #region Configure

            // Create the url adding parameters
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(builder.RequestUrl);

            if (builder.RequestParameters != null)
                urlBuilder.Append($"?{string.Join("&", builder.RequestParameters)}");

            // Build the request
            HttpWebRequest http = WebRequest.CreateHttp(urlBuilder.ToString());
            http.Method = builder.HttpMethod.ToString();
            http.AllowAutoRedirect = builder.Settings.AllowAutoRedirect;
            http.Timeout = (int)builder.Settings.Timeout.TotalMilliseconds;
            http.KeepAlive = true;
            http.UserAgent = builder.Settings.UserAgent;
            http.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //http.Accept = builder.Settings.AcceptType.MimeType;

            // Add authorization
            if (builder.Authorization != null) http.Headers.Add(HttpRequestHeader.Authorization, builder.Authorization.ToString());

            // Add accept encoding and charset
            http.Headers.Add(HttpRequestHeader.AcceptEncoding, builder.Settings.AcceptEncoding);
            http.Headers.Add(HttpRequestHeader.AcceptCharset, builder.Settings.Charset.WebName);

            // Add custom headers
            if (builder.CustomHeaders != null)
            {
                foreach (var header in builder.CustomHeaders)
                    http.Headers.Add(header.Key, header.Value);
            }

            #endregion

            #region Content

            // If there is content to write to the body
            if (builder.Body.contentLength > 0)
            {
                // Write the content
                using (var requestStream = http.GetRequestStream()) // Get request stream
                using (var streamWriter = new StreamWriter(requestStream)) // Get stream writer to write the content
                    await streamWriter.WriteAsync(builder.Body.content); // Write the content

                // Set content headers
                http.ContentLength = builder.Body.contentLength;
                http.ContentType = builder.Body.serializer.MimeType;
            }

            #endregion

            #region Response

            // Send and wait for a response
            try
            {
                // Get response
                var httpResponse = (HttpWebResponse)await http.GetResponseAsync();

                // Create a HttpResponse
                HttpResponse response = new HttpResponse
                {
                    Charset = Encoding.GetEncoding(httpResponse.ContentType.ExtractString("charset=")),
                    ContentLength = httpResponse.ContentLength,
                    ContentType = httpResponse.ContentType.ExtractStringFromStart("charset"),
                    Cookies = httpResponse.Cookies,
                    Date = DateTimeOffset.Parse(httpResponse.Headers.Get("Date")),
                    Status = new HttpStatusCode((int)httpResponse.StatusCode, httpResponse.StatusDescription),
                    IsSuccessful = httpResponse.StatusCode == System.Net.HttpStatusCode.OK
                };

                // Set headers
                foreach (string headerKey in httpResponse.Headers.AllKeys)
                    response.Headers.Add(headerKey, httpResponse.Headers[headerKey]);

                // Create streams to read the content async
                using (var responseStream = httpResponse.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream, response.Charset))
                    response.RawContent = streamReader.ReadToEnd();

                return response;
            }

            // Catch web exceptions
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse validResponse)
                {
                    return new HttpResponse
                    {
                        Charset = Encoding.GetEncoding(validResponse.CharacterSet),
                        ContentType = validResponse.ContentType,
                        ContentLength = validResponse.ContentLength,
                        Status = new HttpStatusCode((int)validResponse.StatusCode, validResponse.StatusDescription),
                        Date = DateTimeOffset.Parse(validResponse.Headers.Get("Date"))
                    };
                }

                return new HttpResponse()
                {
                    Status = new HttpStatusCode((int)System.Net.HttpStatusCode.BadRequest, "Bad Request")
                };
            }

            #endregion
        }

        /// <summary>
        /// Sends the HTTP request to the server and waits for a response
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize as </typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task<HttpResponse<T>> SendAsync<T>(this HttpRequestBuilder builder)
        {
            // Send request and wait for a response
            var response = await SendAsync(builder);

            // Try to deserialize
            // If response content type is not the same the specified in the accept header...
            if (!response.ContentType.Contains(builder.Settings.AcceptType.MimeType))
                return null; // Return null

            // Deserialize content
            T content = builder.Settings.AcceptType.Deserialize<T>(response.RawContent);

            // Create a new response
            HttpResponse<T> newResponse = (response as HttpResponse<T>);
            newResponse.Body = content;

            return newResponse;
        }

        /// <summary>
        /// Sends the HTTP request and only returns the server response
        /// </summary>
        /// <typeparam name="T">The type of response to parse</typeparam>
        public static async Task<T> RawSendAsync<T>(this HttpRequestBuilder builder)
        {
            // Send async
            var response = await SendAsync(builder);

            // Try to deserialize
            // If response content type is not the same the specified in the accept header...
            if (!response.ContentType.Contains(builder.Settings.AcceptType.MimeType))
                return default; // Return null

            // Deserialize content
            T content = builder.Settings.AcceptType.Deserialize<T>(response.RawContent);

            // Return the content of the request
            return content;
        }

        /// <summary>
        /// Sends the HTTP request and only returns the server response
        /// </summary>
        public static async Task<string> RawSendAsync(this HttpRequestBuilder builder)
        {
            // Send async
            var response = await SendAsync(builder);

            // Try to deserialize
            // If response content type is not the same the specified in the accept header...
            if (!response.ContentType.Contains(builder.Settings.AcceptType.MimeType))
                return null; // Return null

            // Deserialize content
            object content = builder.Settings.AcceptType.DeserializeAnonymous(response.RawContent);

            // Return the content of the request
            return content.ToString();
        }

        #endregion
    }
}