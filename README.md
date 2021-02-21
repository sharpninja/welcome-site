# Welcome Site

![Docker Image CI](https://github.com/sharpninja/welcome-site/workflows/Docker%20Image%20CI/badge.svg?branch=master)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/4e01573860d346198f982e0d10f37a5e)](https://www.codacy.com/gh/sharpninja/welcome-site/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=sharpninja/welcome-site&amp;utm_campaign=Badge_Grade)

I put this project together as a simple way to post a site to introduce myself to my new coworkers 
and to get to know them.

The site is build with [Server-Side Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0) 
on [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) and 
[SyncFusion Blazor Controls](https://www.syncfusion.com/blazor-components) 
(you can get a free community license [here](https://www.syncfusion.com/products/communitylicense)).

The content is stored in Markdown files and displayed with [Markdig](https://github.com/xoofx/markdig).  

The site is configured for ASP.Net Identity and by default allows using 
[GitHub Authentication](https://docs.github.com/en/github/authenticating-to-github/about-authentication-to-github).

The pipeline builds and publishes a Docker file to an 
[Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/) and is hosted with a
free-tier Linux-based [Azure App Service](https://azure.microsoft.com/en-us/services/app-service/) and it uses 
[Azure SQL Server](https://azure.microsoft.com/en-us/services/sql-database/campaign/) for persistence.

## Customizing

* Change the Markdown files in the `wwwroot` folder.
* Add questions to the Survey.
* Change `shared/NavMenu.razor` to modify the menu.

## Requirements

The application is designed to utilize the features of Microsoft Azure.

### Services

* [GitHub](https://github.com): 
    Provides source control and CI/CD pipeline.
* [GitHub Authentication](https://docs.github.com/en/github/authenticating-to-github/about-authentication-to-github): 
    Provides OAuth2 Login for Users.
* [Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/): 
    Deployment cache for Docker files.
* [Azure App Service](https://azure.microsoft.com/en-us/services/app-service/): 
    Docker Container Host
* [Azure SQL Server](https://azure.microsoft.com/en-us/services/sql-database/campaign/): 
    Survey and Identity data storage.
* [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/): 
    Encrypted secrets management.
* [Azure Storage](https://azure.microsoft.com/en-us/services/storage/): 
    Blob storage for encrypted secrets.
* [Azure Active Directory](https://azure.microsoft.com/en-us/services/active-directory/): 
    Used for managing the client credentials to connect the App Service to the 
    Azure Key Vault and Azure Storage.

## Deployment

Follow [these steps](https://docs.microsoft.com/en-us/azure/app-service/deploy-github-actions?tabs=applevel) 
from Microsoft.

### Notes

* Use [User Secrets Locally](WelcomeSite/Secrets.md) for local development.
* Create [Secrets in GitHub](https://docs.github.com/en/actions/reference/encrypted-secrets) 
  for GitHub Actions
    * AZURE_PASSWORD: Client Secret for pushing to Azure Container Registry
    * AZURE_USERNAME: Client Login for pushing to Azure Container Registry
    * AZURE_TENANT: Tenant ID for pushing to Azure Container Registry

## Interesting Bits

* Single Blazor page, navigate via the `WelcomeSite.Services.NavManager` class.
