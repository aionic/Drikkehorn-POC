using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Drikkehorn.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class ServicePrincipalProvider : IAuthenticationProvider
    {
        private readonly string _applicationId;
        private readonly string _tenantId;
        private readonly string _authority;
        private readonly IConfidentialClientApplication _clientApplication;
        private IReadOnlyList<string> _scopes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="tenantId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="authority"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="scopes"></param>
        public ServicePrincipalProvider(
            string applicationId,
            string tenantId,
            string clientSecret,
            string authority,
            string redirectUrl, 
            List<string> scopes
        ) {
            _applicationId = applicationId;
            _tenantId = tenantId;
            _scopes = scopes.AsReadOnly();
            _authority = authority;
            _clientApplication = ConfidentialClientApplicationBuilder.Create(applicationId)
                                              .WithAuthority(authority)
                                              .WithRedirectUri(redirectUrl)
                                              .WithClientSecret(clientSecret)
                                              .Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var token = await GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult authResult = await _clientApplication.AcquireTokenForClient(_scopes)
                                .ExecuteAsync();
            return authResult.AccessToken;
        }
    }
}
