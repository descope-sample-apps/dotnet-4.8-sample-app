<img width="1088" alt="Screenshot 2024-05-10 at 11 59 05 AM" src="https://github.com/descope-sample-apps/dotnet-4.8-sample-app/assets/32936811/d9487528-727a-4cf1-8607-ae5731305c76">

# Descope ASP.NET Web App Sample

This sample application demonstrates the integration of Descope with a .NET Framework 4.8 backend and a traditional ASP.NET web application using JavaScript for user authentication flows.

## Table of Contents 📝

1. [Features](#features)
2. [Installation](#installation)
3. [Running the Application](#running-the-application)
4. [Environment Setup](#environment-setup)
5. [API Protection with TokenValidator](#api-protection-with-tokenvalidator)
6. [Using Descope Web Component](#using-descope-web-component)
7. [Issue Reporting](#issue-reporting)
8. [License](#license)

## Installation 💿

Clone the repository:

```bash
git clone https://github.com/descope-sample-apps/dotnet-4.8-sample-app
```

Navigate to the cloned repository directory. Install dependencies and build the solution by opening the `.sln` file in Visual Studio and restoring NuGet packages.

## Running the Application 🚀

To start the application:

1. Open the solution file (`.sln`) in Visual Studio.
2. Set the `DescopeProjectId` environment variable (see [Environment Setup](#environment-setup)).
3. Run the solution (F5 or the "Start" button in Visual Studio).

## Environment Setup 🛠️

1. Set the `DESCOPE_PROJECT_ID` environment variable:

- **Windows**:
  ```bash
  setx DESCOPE_PROJECT_ID "YOUR_DESCOPE_PROJECT_ID"
  ```

Replace `YOUR_DESCOPE_PROJECT_ID` with your actual Descope Project ID.

2. Place your Descope Project ID in the SDK initialization in the `AuthenticatedPage.aspx`, so that the web component will use your own flows:

```
const sdk = Descope({ projectId: "YOUR_DESCOPE_PROJECT_ID", persistTokens: true, autoRefresh: true });
```

## API Protection with TokenValidator 🔒

The `TokenValidator` class is used to secure API endpoints by validating JWT tokens, passed to your backend as a [Bearer Token](https://swagger.io/docs/specification/authentication/bearer-authentication/). Here’s an example of how to protect an API controller:

```csharp
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;

namespace DescopeSampleApp.Controllers {
    public class SampleController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            var authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader != null && authorizationHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var sessionToken = authorizationHeader.Parameter;
                if (!string.IsNullOrEmpty(sessionToken))
                { 
                    // Validate the session token
                    var tokenValidator = new TokenValidator("YOUR_DESCOPE_PROJECT_ID");
                    try
                    {
                        var claimsPrincipal = await tokenValidator.ValidateSession(sessionToken);
                        return Ok("This is a sample API endpoint.");
                    }
                    catch (SecurityTokenValidationException)
                    {
                        return Unauthorized();
                    }
                }
            }

            return Unauthorized();
        }
    }
}
```

## Using Descope Web Component 🌐

In the `AuthenticatedPage.aspx` file, use the Descope Web SDK to handle user authentication:

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="DescopeSampleApp.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="https://unpkg.com/@descope/web-js-sdk@1.10.45/dist/index.umd.js"></script>
</head>
<body>
    <form id="loginForm" runat="server">
        <p>Welcome to the Authenticated Page!</p>
    </form>

    <script>
        const sdk = Descope({ projectId: "YOUR_DESCOPE_PROJECT_ID", persistTokens: true, autoRefresh: true });

        const sessionToken = sdk.getSessionToken()
        const currentPath = window.location.pathname;
        console.log(currentPath)
        if ((sessionToken) && (!sdk.isJwtExpired(sessionToken))) {
            // User is logged in
        } else {
            if (currentPath != '/login.aspx') {
                // Redirect to login page
                window.location.replace('/login.aspx');
            }
        }
    </script>
</body>
</html>
```

## Issue Reporting ⚠️

For any issues or suggestions, please [open an issue](https://github.com/descope-sample-apps/dotnet-4.8-sample-app/issues) on GitHub.

## License 📜

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
