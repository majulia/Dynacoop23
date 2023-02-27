using Dynacoop2023.Controller;
using Dynacoop2023.Model;
using FirstPluginsAlfaPeople.DynacoopISV;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FirstPluginsAlfaPeople
{
    public class AccountManager : PluginCore
    {        
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity opportunity = new Entity();
            bool? incrementOrDecrement = null;

            SetVariables(this.Context, out opportunity, out incrementOrDecrement);
            ExecuteOpportunityProcess(this.Context, opportunity, incrementOrDecrement);
        }

        private void ExecuteOpportunityProcess(IPluginExecutionContext context, Entity opportunity, bool? incrementOrDecrement)
        {
            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;

            if (accountReference != null)
            {
                Entity oppAccount = UpdateAccount(incrementOrDecrement, accountReference);

                if (context.MessageName == "Update")
                {
                    Entity opportunityPostImage = (Entity)context.PostEntityImages["PostImage"];
                    EntityReference postAccountReference = (EntityReference)opportunityPostImage["parentaccountid"];
                    UpdateAccount(true, postAccountReference);
                }
            }
        }

        private Entity UpdateAccount(bool? incrementOrDecrement, EntityReference accountReference)
        {
            ContaController contaController = new ContaController(this.Service);
            Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "dcp_nmr_total_opp" });
            contaController.IncrementOrDecrementNumberOfOpp(oppAccount, incrementOrDecrement);
            return oppAccount;
        }

        private void SetVariables(IPluginExecutionContext context, out Entity opportunity, out bool? incrementOrDecrement)
        {
            if (context.MessageName == "Create")
            {
                opportunity = (Entity)context.InputParameters["Target"];
                incrementOrDecrement = true;
            }
            else
            {
                opportunity = (Entity)context.PreEntityImages["PreImage"];
                incrementOrDecrement = false;
            }
        }
    }
}
