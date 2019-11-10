using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bramf.Http
{
    /// <summary>
    /// Helps injecting HTTP services
    /// </summary>
    public static class HttpFrameworkConstructionExtensions
    {
        /// <summary>
        /// Injects a reusable http requestor
        /// </summary>
        /// <param name="construction">The construction.</param>
        /// <param name="url">The url to make the requesto to.</param>
        public static FrameworkConstruction AddReusableHttpRequest(this FrameworkConstruction construction, string url)
        {
            // Inject
            construction.Services.AddScoped<IReusableHttpRequest>(_ => new ReusableHttpRequest(url, new HttpRequestSettings()));

            return construction;
        }

        /// <summary>
        /// Injects a reusable http requestor
        /// </summary>
        /// <param name="construction">The construction.</param>
        /// <param name="url">The url to make the requesto to.</param>
        /// <param name="requestSettings">Configures the requestor</param>
        public static FrameworkConstruction AddReusableHttpRequest(this FrameworkConstruction construction, string url, Action<HttpRequestSettings> requestSettings)
        {
            // Get settings
            HttpRequestSettings settings = new HttpRequestSettings();
            requestSettings.Invoke(settings);

            // Inject
            construction.Services.AddScoped<IReusableHttpRequest>(_ => new ReusableHttpRequest(url, settings));

            return construction;
        }

        /// <summary>
        /// Injects a reusable http requestor
        /// </summary>
        /// <param name="construction">The construction.</param>
        /// <param name="url">The url to make the requesto to.</param>
        /// <param name="requestSettings">Configures the requestor</param>
        /// <param name="refreshAuthorizationInterval">The interval that the requestor will fire the function to get a new authorization value if needed</param>
        /// <param name="getAuthorizationValueFunction">The function that will be get executed to get the new authorization value</param>
        public static FrameworkConstruction AddReusableHttpRequest(this FrameworkConstruction construction, string url, Action<HttpRequestSettings> requestSettings, TimeSpan refreshAuthorizationInterval, Func<string> getAuthorizationValueFunction)
        {
            // Get settings
            HttpRequestSettings settings = new HttpRequestSettings();
            requestSettings.Invoke(settings);

            // Inject
            construction.Services.AddScoped<IReusableHttpRequest>(_ => new ReusableHttpRequest(url, settings).WithAuthorizationRefreshEach(refreshAuthorizationInterval, getAuthorizationValueFunction));

            return construction;
        }
    }
}
