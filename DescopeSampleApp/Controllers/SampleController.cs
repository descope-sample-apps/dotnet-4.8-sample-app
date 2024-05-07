using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                    var tokenValidator = new TokenValidator("P2dI0leWLEC45BDmfxeOCSSOWiCt");
                    try
                    {
                        var claimsPrincipal = await tokenValidator.ValidateSession(sessionToken);
                        return Ok("This is a sample API endpoint.");
                    }
                    catch (SecurityTokenValidationException ex)
                    {
                        return Unauthorized();
                    }
                }
            }

            return Unauthorized();
        }
    }
}