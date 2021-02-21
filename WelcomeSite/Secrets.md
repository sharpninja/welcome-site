# Site Secrets

You need to setup [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows) 
to develop the code.  You should add the following values to this file.

```json
{
  "Kestrel:Certificates:Development:Password": "<Your machines password>",
  "ConnectionStrings:DefaultConnection": "Server=<host>;Initial Catalog=welcome-site-db;Persist Security Info=False;User ID=<username>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
  "BlobStorage": "<Azure Blob Storage for Keys>",
  "KeyVaultUri": "<Azure Key Vault URL>",
  "KeyVaultSecret": "<Azure Key Vault Secret>",
  "tenantId": "<Azure AD Application Tenant ID>",
  "clientId": "<Azure AD Application Client ID>",
  "GitHub_ClientId": "<OAuth Client ID>",
  "GitHub_ClientSecret": "<OAuth Client Secret>",
  "SFKey": "<Your Sync Fussion Key>"
}
```

> ___These key-value pairs should be added to the AppSettings in the configuration blade of the Azure App Service.  Do not add them
> to your appSettings.json files and put them in Git!___

[README](../README.md)
