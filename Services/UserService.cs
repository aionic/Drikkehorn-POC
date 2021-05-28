using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drikkehorn.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService
    {
        //
        private readonly GraphServiceClient _graphClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphClient"></param>
        public UserService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task<IGraphServiceUsersCollectionPage> GetAsync(string filterString)
            => _graphClient
            .Users
            .Request()
            .Filter(filterString)
            .GetAsync();
    }
}
