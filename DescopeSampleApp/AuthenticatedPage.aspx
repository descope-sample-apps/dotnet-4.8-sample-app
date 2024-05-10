<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="DescopeSampleApp.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login</title>
    <script src="https://unpkg.com/@descope/web-js-sdk@1.10.45/dist/index.umd.js"></script>
</head>
<body>
    <form id="loginForm" runat="server">
        <p>Welcome to the Authenticated Page!</p>
        <button onclick="testSampleAPI(event)">Click here to test Sample API</button>
    </form>

    <div id="apiResponse"></div>

    <script>
        const sdk = Descope({ projectId: "__Your Descope Project ID__", persistTokens: true, autoRefresh: true });

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

        async function testSampleAPI(event) {
            event.preventDefault();
            const sessionToken = sdk.getSessionToken();
            if (sessionToken && !sdk.isJwtExpired(sessionToken)) {
                try {
                    const response = await fetch("/api/sample", {
                        headers: {
                            "Authorization": `Bearer ${sessionToken}`
                        }
                    });

                    if (!response.ok) {
                        throw new Error(`Request failed with status ${response.status}`);
                    }

                    const responseData = await response.json();
                    console.log(responseData);
                    document.getElementById("apiResponse").innerHTML = `<p>Status Code: ${response.status}</p><pre>${JSON.stringify(responseData, null, 2)}</pre>`;
                } catch (error) {
                    console.error("Request failed:", error.message);
                    document.getElementById("apiResponse").innerHTML = `<p>Error: ${error.message}</p>`;
                }
            } else {
                window.location.replace('/login.aspx');
            }
        }
    </script>
</body>
</html>
