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
            string url = 
            string clientId = 
            string clientSecret = 

            CrmServiceClient serviceClient = new CrmServiceClient($"AuthType=ClientSecret;Url=https://{url};AppId={clientId};ClientSecret={clientSecret};");

            if (!serviceClient.CurrentAccessToken.Equals(null))
                Console.WriteLine("Conexão Realizada com sucesso");
            else
                Console.WriteLine("Conexão falhou");


            return serviceClient;
        }
    }
}
