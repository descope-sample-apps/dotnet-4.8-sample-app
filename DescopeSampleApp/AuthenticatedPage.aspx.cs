using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DescopeSampleApp
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.IsAuthenticated)
            {
                // Validate the session token
                var tokenValidator = new TokenValidator("P2dI0leWLEC45BDmfxeOCSSOWiCt");
                var sessionToken = Request.Cookies["sessionToken"]?.Value;
                if (sessionToken != null)
                {
                    try
                    {
                        var claimsPrincipal = tokenValidator.ValidateSession(sessionToken);
                        // Session token is valid, you can set user identity or perform other actions
                    }
                    catch (SecurityTokenValidationException ex)
                    {
                        // Session token validation failed, redirect to login page
                        Response.Redirect("~/Login.aspx");
                    }
                }
                else
                {
                    // No session token found, redirect to login page
                    Response.Redirect("~/Login.aspx");
                }
            }
        }
    }
}