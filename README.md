# Azure_Web
The template of WebAPI and Blazor client apps that are consuming the api securely.
[Enable MS Identity middleware on API](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-app-configuration)
appsetting.json:
    "AzureAD": {
      "Instance": "https://login.microsoftonline.com/",       #auth endpoint
      "ClientId": null,                                       #api clients id from Expose API, e.g. api://{guid}
      "TenantId": null                                        #AD tenant Id
    },
[Expose API](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-expose-web-apis)
[Configure client apps App registrations](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis)
[Call a web API from ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-3.1&pivots=server)
[Sign a user into a Desktop application using Microsoft Identity Platform and call a protected ASP.NET Core Web API, which calls Microsoft Graph on-behalf of the user](https://github.com/Azure-Samples/active-directory-dotnet-native-aspnetcore-v2/tree/master/2.%20Web%20API%20now%20calls%20Microsoft%20Graph)
[IDW10502 Error](https://xtremeownage.com/2021/11/10/microsoftidentitywebchallengeuserexception-idw10502/)

Client app Authentication must be configured

Useful links:
[Blazor Server App with Azure AD Authentication, that calls the Microsoft Graph API on-behalf of the signed-in user.](https://github.com/wmgdev/BlazorGraphApi)