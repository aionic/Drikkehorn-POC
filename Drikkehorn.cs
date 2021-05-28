using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Graph;
using Drikkehorn.Services;
using Drikkehorn.Providers;
using System.Linq;

namespace Drikkehorn
{
    /// <summary>
    /// 
    /// </summary>
    public static class SampleAdminUnitFunction
    {
        private static GroupService _groupService;
        private static AdministrativeUnitService _administrativeUnitService;
        private static UserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("SampleAdminUnitFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log
        ) {
            var graphClient = GenerateGraphClient();
            var id = Guid.NewGuid().ToString().Replace("-", "");

            _groupService = new GroupService(graphClient);
            _administrativeUnitService = new AdministrativeUnitService(graphClient);
            _userService = new UserService(graphClient);

            // Create a new group
            var newGroup = await CreateGroup(id);
            log.LogInformation($"Group {newGroup.DisplayName} was successfully create with id {newGroup.Id}");

            // Find user and add them to the group
            var user = (await _userService.GetAsync("startswith(userPrincipalName, 'SEARCH FOR USERNAME TO ADD HERE')")).Single();
            await _groupService.AddGroupMember(newGroup, user);
            log.LogInformation($"Directory object with id {user.Id} was successfully added to the group {newGroup.DisplayName} with id {id}");

            // Create a new Admin Unit
            var newAdministrativeUnit = await CreateAdministrativeUnit(id);
            log.LogInformation($"Administrative unit {newAdministrativeUnit.DisplayName} was successfully create with id {newAdministrativeUnit.Id}");

            // Add the group to the admin unit
            await _administrativeUnitService.AddDirectoryObjectAsync(newAdministrativeUnit, newGroup);
            log.LogInformation($"Directory object with id {newGroup.Id} was successfully added to the administrative unit {newAdministrativeUnit.DisplayName} with id {id}");

            return new OkObjectResult("OK");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static GraphServiceClient GenerateGraphClient()
        {
            var clientId = System.Environment.GetEnvironmentVariable("ClientId", EnvironmentVariableTarget.Process);
            var tenantId = System.Environment.GetEnvironmentVariable("TenantId", EnvironmentVariableTarget.Process);
            var clientSecret = System.Environment.GetEnvironmentVariable("ClientSecret", EnvironmentVariableTarget.Process);
            var authority = System.Environment.GetEnvironmentVariable("Authority", EnvironmentVariableTarget.Process);
            var redirectUrl = System.Environment.GetEnvironmentVariable("RedirectUrl", EnvironmentVariableTarget.Process);
            var scopesItem = Environment.GetEnvironmentVariable("Scopes");
            var scopes = JsonConvert.DeserializeObject<List<string>>(scopesItem);
            var authProvider = new ServicePrincipalProvider(clientId, tenantId, clientSecret, authority, redirectUrl, scopes);

            return new GraphServiceClient(authProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Task<Group> CreateGroup(string id)
            => _groupService.AddAsync(new Group
            {
                DisplayName = $"Sample Group {id}",
                MailEnabled = false,
                MailNickname = "Test",
                SecurityEnabled = true
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static Task<AdministrativeUnit> CreateAdministrativeUnit(string id)
            => _administrativeUnitService.AddAsync(new AdministrativeUnit { DisplayName = $"Sample AU {id}" });
    }
}
