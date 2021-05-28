using Microsoft.Graph;
using System.Threading;
using System.Threading.Tasks;

namespace Drikkehorn.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeUnitService
    {
        //
        private readonly GraphServiceClient _graphClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphClient"></param>
        public AdministrativeUnitService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IDirectoryAdministrativeUnitsCollectionPage> GetAsync()
            => _graphClient
            .Directory
            .AdministrativeUnits
            .Request()
            .GetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="administrativeUnit"></param>
        /// <returns></returns>
        public Task<AdministrativeUnit> AddAsync(AdministrativeUnit administrativeUnit)
            => _graphClient.Directory.AdministrativeUnits
            .Request()
            .AddAsync(administrativeUnit, CancellationToken.None);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="administrativeUnit"></param>
        /// <param name="directoryObject"></param>
        /// <returns></returns>
        public Task AddDirectoryObjectAsync(AdministrativeUnit administrativeUnit, DirectoryObject directoryObject)
            => _graphClient
            .Directory
            .AdministrativeUnits[administrativeUnit.Id]
            .Members
            .References
            .Request()
            .AddAsync(new DirectoryObject { Id = directoryObject.Id }, CancellationToken.None);
    }
}
