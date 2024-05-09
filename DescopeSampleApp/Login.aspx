<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="DescopeSampleApp.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login</title>
    <script src="https://unpkg.com/@descope/web-js-sdk@1.10.45/dist/index.umd.js"></script>
    <script src="https://unpkg.com/@descope/web-component@3.11.14/dist/index.js"></script>
</head>
<body>
    <form id="loginForm" runat="server">
        <descope-wc project-id="P2dI0leWLEC45BDmfxeOCSSOWiCt" flow-id="sign-up-or-in"></descope-wc>
    </form>

    <script>
        const wcElement = document.getElementsByTagName('descope-wc')[0];
        const onSuccess = () => {
            window.location.replace('/');
        };
        const onError = (err) => console.log(err);

        wcElement.addEventListener('success', onSuccess);
        wcElement.addEventListener('error', onError);
    </script>
</body>
</html>
