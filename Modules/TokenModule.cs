using AutenticationDemo.DTOs.Negocio;
using Nancy;

namespace AutenticationDemo.Modules
{
    public class TokenModule : NancyModule
    {
        public TokenModule() : base("api/token")
        {
            Post("/", args =>
            {

                var apiKey = UserDatabase.ValidateUser(
                    (string)this.Request.Form.Username,
                    (string)this.Request.Form.Password);

                return string.IsNullOrEmpty(apiKey)
                    ? new Response { StatusCode = HttpStatusCode.Unauthorized }
                : this.Response.AsJson(new { ApiKey = apiKey});
            });
            Delete("/", args =>
            {
                var apiKey = (string)this.Request.Form.ApiKey;
                UserDatabase.RemoveApiKey(apiKey);
                return new Response { StatusCode = HttpStatusCode.OK };
            });
        }
    }
}