using AutenticationDemo.DTOs.Negocio;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace AutenticationDemo.NancyBootstraper
{
    public class StatelessAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
             // Na inicialização do pedido, modificamos os encaminhamentos de solicitação para
             // inclui autenticação sem estado
             // Configurar a autenticação sem estado é simples. Basta usar o
             // NancyContext para obter o apiKey. Em seguida, use o apiKey para obter
             // a identidade do seu usuário.
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                    {
                        // recupera o token do Cabeçalho http    
                        var apiKey = (string) nancyContext.Request.Headers.Authorization;

                        //Obtem a identidade do usuário
                        return UserDatabase.GetUserFromApiKey(apiKey);
                    });

            AllowAccessToConsumingSite(pipelines);

            StatelessAuthentication.Enable(pipelines, configuration);
        }

        static void AllowAccessToConsumingSite(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(x =>
                {
                    x.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    x.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,DELETE,PUT,OPTIONS");
                });
        }
    }
}