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


