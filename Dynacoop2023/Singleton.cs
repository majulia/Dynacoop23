using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.Controller
{
    public class Singleton
    {
        public static CrmServiceClient GetService()
        {
            string url = "orgc9746b6b.crm2.dynamics.com";
            string clientId = "9bb5eadf-429d-4b27-9f59-5e6ce93b5597";
            string clientSecret = "gA78Q~7CV3VWYUMMLx2qkUIIaZm_cbpiT4sZ5cKD";

            CrmServiceClient serviceClient = new CrmServiceClient($"AuthType=ClientSecret;Url=https://{url};AppId={clientId};ClientSecret={clientSecret};");

            if (!serviceClient.CurrentAccessToken.Equals(null))
                Console.WriteLine("Conexão Realizada com sucesso");
            else
                Console.WriteLine("Conexão falhou");


            return serviceClient;
        }
    }
}
