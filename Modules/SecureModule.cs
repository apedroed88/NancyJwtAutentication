using System;
using AutenticationDemo.DTOs;
using AutenticationDemo.DTOs.Negocio;
using Nancy;
using Nancy.Security;

namespace AutenticationDemo.Modules
{
    public class SecureModule : NancyModule
    {
        public SecureModule():base("api/secure")
        {
            this.RequiresAuthentication();
            

            Get("/", args =>
            {
                //Context.CurrentUser foi definido pela StatelessAuthentication no pipeline
                var identity = this.Context.CurrentUser;

                //Retornar as informações seguras em uma resposta json
                var userModel = new UserModel(identity.Identity.Name);
                return this.Response.AsJson(new
                {
                    SecureContent = "Aqui está um conteúdo seguro que você só pode ver se você fornecer uma chave correta api",
                    User = userModel
                });
            });

            Post("/Usuario/Novo", args =>
            {
                Tuple<string, string> user = UserDatabase.CreateUser(this.Context.Request.Form["username"], this.Context.Request.Form["password"]);
                return this.Response.AsJson(new { username = user.Item1 });
            });
        }
    }
}