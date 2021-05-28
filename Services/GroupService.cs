using Microsoft.Graph;
using System.Threading;
using System.Threading.Tasks;

namespace Drikkehorn.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupService
    {
        //
        private readonly GraphServiceClient _graphClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphClient"></param>
        public GroupService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IGraphServiceGroupsCollectionPage> GetAsync()
            => _graphClient
            .Groups
            .Request()
            .GetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Task<Group> AddAsync(Group group)
            => _graphClient
            .Groups
            .Request()
            .AddAsync(group, CancellationToken.None);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="directoryObject"></param>
        /// <returns></returns>
        public Task AddGroupMember(Group group, DirectoryObject directoryObject)
            => _graphClient
            .Groups[group.Id]
            .Members
            .References
            .Request()
            .AddAsync(new DirectoryObject { Id = directoryObject.Id }, CancellationToken.None);
    }
}
