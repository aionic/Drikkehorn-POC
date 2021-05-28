# Introduction

This project contains POC function code to support allowing the creation of an AU/Assignment of a group within the context of a service principal.  This ultimately allows a Service Account to create/manage AU's without needing group administration globally

## About

This project is a POC function application that is triggered with an HTTP GET and will create a group, administrative unit, assign a user to that group, and then assign a group as a member of that administrative unit. The function utilizes a service principal to connect to Azure Active Directory and execute a set of graph commands needed for group and administrative unit creation. Please take note that in order to complete the needed commands you will need to grant a set of administrative privileges provided below. This function should be secured behind APIM or a gateway that can apply policies for exposing this function.
This project utilizes the graph API's and to get the direct graph url's you can breakpoint the code and inspect the graph client for the specific url endpoints.

### Getting Started

To get started with this code you will need to create a [service principal application in the Azure application registration](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-authenticate-service-principal-powershell). Once you create the service principal you will need to navigate to API permissions and select the permissions bellow at the application level. This permission set will need administrative approval to become active. Click the grant admin consent button which will apply this permission set to the application.

Since this application is being run as a function without user intervention you will also need to grant permissions at the enterprise application level. Navigate to enterprise applications and locate the service principal. Once in the application navigate to permissions and grant the application consent to the directory. This is used since a user will not interact with the login and we need to allow permissions to be granted for the service principal when utilizing a token.
| Permission | Type | Description |
| ----------- | ----------- | ----------- |
| AdministrativeUnit.ReadWrite.All | Application | Read and write all administrative units |
| Group.ReadWrite.All | Application | Read and write all groups |
| User.Read.All | Application | Read all users' full profiles |

### Libraries

| Library | Description | Nuget Link |
| ----------- | ----------- | ----------- |
| Microsoft.Identity.Client | Used to login into the tenant and generate a token | https://www.nuget.org/packages/Microsoft.Identity.Client/ |
| Microsoft.Graph | Graph client that interacts with Azure graph API's | https://www.nuget.org/packages/Microsoft.Graph/4.0.0-preview.5 |

### Build and Test

Can execute this code leveraging [vscode](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp) or visual studio.
In VSCode you'll need the following plugins/extension

- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)
- [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [.NET CLI Tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
