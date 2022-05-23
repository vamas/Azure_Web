# Azure_Web

The template of AzureAD secured WebAPI and Blazor client applications.

# Configuration

## WebAPI

### AzureAD
- Register the API in AzureAD
- Create app secret to allowus of the api from other services (if required)
- Expose API with creating appropriate scopes

### Application
- Configure JWT Bearer authentication middleware is startup.cs. The parameters in appsettings.json are
  - Instance    #Authentication URL
  - ClientId
  - TenantId
```
  services.AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAD", options));
```
- Enable UseAuthentication and UseAuthorization in startup.cs
```
  app.UseAuthentication();
  app.UseAuthorization();
```
- Configure RequiredScopes attribute on AzureAD protected controllers. The scopes must much registered app scopes.
```
    [Authorize]
    [RequiredScope("databasetables.read")]
    [Route("[controller]")]
    [ApiController]
```

## Blazor Server SPA

### AzureAD
- Register the application in AzureAD
- Add API permissions to access WebAPI using Delegated Permissions to use signed-in user context

### Application
- Configure AzureAD Microsoft Identity middleware pipeline. The parameters in appsettings.json are
  - Instance    #Authentication URL
  - Domain
  - TenantId
  - ClientId
  - ClientSecret
  - CallbackPath    # "/signin-oidc"
- Configure downstream API parameters. Important!!! - for ScopeForAccessToken value use exposed api urls, e.g. api://{guid}/scope
  - ApiBaseAddress
  - ScopeForAccessToken
- In startup.cs enable middleware pipeline with EnableTokenAcquisitionToCallDownstreamApi to inject ITokenAcquisition to be used by HttpClient 
```
  string[] initialScopes = Configuration.GetValue<string>("CallApi:ScopeForAccessToken")?.Split(' ');
  services.AddMicrosoftIdentityWebAppAuthentication(Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddInMemoryTokenCaches();
```
- To prevent error IDW10502 when website doens't know how to handle unknown cookies enable cookie filter
```
  services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, 
                options => options.Events = new RejectSessionCookieWhenAccountNotInCacheEvents());
```
```
  public class RejectSessionCookieWhenAccountNotInCacheEvents : CookieAuthenticationEvents
    {
        public async override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            try
            {
                var tokenAcquisition = context.HttpContext.RequestServices.GetRequiredService<ITokenAcquisition>();
                string token = await tokenAcquisition.GetAccessTokenForUserAsync(
                    scopes: new[] { "profile" },
                    user: context.Principal);
            }
            catch (MicrosoftIdentityWebChallengeUserException ex) when (AccountDoesNotExitInTokenCache(ex))
            {
                context.RejectPrincipal();
            }
        }
        /// <summary>
        /// Is the exception thrown because there is no account in the token cache?
        /// </summary>
        /// <param name="ex">Exception thrown by <see cref="ITokenAcquisition"/>.GetTokenForXX methods.</param>
        /// <returns>A boolean telling if the exception was about not having an account in the cache</returns>
        private static bool AccountDoesNotExitInTokenCache(MicrosoftIdentityWebChallengeUserException ex)
        {
            return ex.InnerException is MsalUiRequiredException && (ex.InnerException as MsalUiRequiredException).ErrorCode == "user_null";
        }
    }
```
- Prefix all HttpClient calls to obtain token and include it in the requests
```
  var scope = _configuration["CallApi:ScopeForAccessToken"];
  var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope });
  _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
```

## Useful links

[Enable MS Identity middleware on API](https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-app-configuration)

[Expose API](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-expose-web-apis)

[Configure client apps App registrations](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis)

[Call a web API from ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-3.1&pivots=server)

[Sign a user into a Desktop application using Microsoft Identity Platform and call a protected ASP.NET Core Web API, which calls Microsoft Graph on-behalf of the user](https://github.com/Azure-Samples/active-directory-dotnet-native-aspnetcore-v2/tree/master/2.%20Web%20API%20now%20calls%20Microsoft%20Graph)

[IDW10502 Error](https://xtremeownage.com/2021/11/10/microsoftidentitywebchallengeuserexception-idw10502/)

[Blazor Server App with Azure AD Authentication, that calls the Microsoft Graph API on-behalf of the signed-in user.](https://github.com/wmgdev/BlazorGraphApi)
