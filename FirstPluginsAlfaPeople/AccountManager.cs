using Dynacoop2023.Controller;
using Dynacoop2023.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Text;
using System.Threading.Tasks;

namespace FirstPluginsAlfaPeople
{
    public class AccountManager : IPlugin
    {
        
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity opportunity = (Entity)context.InputParameters["Target"];

            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;

            tracingService.Trace("Iniciou processo de Plugin");

            if (accountReference != null)
            {
                tracingService.Trace("Conta encontrada");

                ContaController contaController = new ContaController(service);
                Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "dcp_nmr_total_opp" });
                contaController.IncrementNumberOfOpp(oppAccount);
            }
        }

       
    }
}
