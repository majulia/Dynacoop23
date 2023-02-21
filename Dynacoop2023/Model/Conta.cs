using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.Model
{
    public class Conta
    {
        public CrmServiceClient ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Conta(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "account";
        }

        public Guid Create()
        {

            Entity conta = new Entity(this.Logicalname);
            conta["name"] = "Volkswagen";
            conta["telephone1"] = "(11) 1515-3232";
            conta["fax"] = "(11) 1515-3232";

            conta["dcp_nmr_total_opp"] = 0;
            conta["dcp_tipo_relacao_opp"] = new OptionSetValue(100000000);
            conta["dcp_valor_total_opp"] = new Money(0);

            //conta["primarycontactid"] = new EntityReference("contact", new Guid("15e09654-05aa-ed11-9885-000d3a888d06"));

            //Representação de registro na tabela
            //Entity conta = new Entity(this.Logicalname);

            //Guid = id do registro da tabela
            Guid accountId = this.ServiceClient.Create(conta);
            return accountId;
        }

        public bool Update(Guid accountId, string telephone1)
        {
            Entity conta = new Entity(this.Logicalname, accountId);
            conta["telephone1"] = telephone1;
            this.ServiceClient.Update(conta);
            return false;
        }

        public bool Delete(Guid accounId)
        {
            try
            {
                this.ServiceClient.Delete(this.Logicalname, accounId);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public Entity GetAccountByName(string name)
        {
            QueryExpression queryAccount = new QueryExpression(this.Logicalname);
            queryAccount.ColumnSet.AddColumns("telephone1", "primarycontactid");
            queryAccount.Criteria.AddCondition("name", ConditionOperator.Equal, name);
            return RetrieveOneAccount(queryAccount);
        }

        private Entity RetrieveOneAccount(QueryExpression queryAccount)
        {
            EntityCollection accounts = this.ServiceClient.RetrieveMultiple(queryAccount);

            if (accounts.Entities.Count() > 0)
                return accounts.Entities.FirstOrDefault();
            else
                Console.WriteLine("Nenhuma conta encontrada com esse nome");

            return null;
        }

        public Entity GetAccountByContactName(string contactName, string[] columns)
        {
            QueryExpression queryAccount = new QueryExpression(this.Logicalname);
            queryAccount.ColumnSet.AddColumns(columns);
            queryAccount.AddLink("contact", "primarycontactid", "contactid");
            queryAccount.LinkEntities.FirstOrDefault().LinkCriteria.AddCondition("fullname", ConditionOperator.Equal, contactName);
            return RetrieveOneAccount(queryAccount);
        }

        public Entity GetAccountById(Guid id)
        {
            var context = new OrganizationServiceContext(this.ServiceClient);

            return (from a in context.CreateQuery("account")
                    join b in context.CreateQuery("contact")
                    on ((EntityReference)a["primarycontactid"]).Id equals b["contactid"]
                    where (Guid)a["accountid"] == id
                    select a).ToList().FirstOrDefault();
        }

        public EntityCollection GetAccountByLike(string like)
        {
            QueryExpression queryExpression = new QueryExpression(this.Logicalname);
            queryExpression.ColumnSet.AddColumns("name");
            queryExpression.Criteria.AddCondition("name", ConditionOperator.BeginsWith, like);
            return this.ServiceClient.RetrieveMultiple(queryExpression);
        }

        public void UpsertMultipleRequest(EntityCollection entityCollection)
        {
            OrganizationRequestCollection requestCollection = new OrganizationRequestCollection();

            foreach (Entity entity in entityCollection.Entities)
            {
                AddUpsertRequest(requestCollection, entity);
            }

            ExecuteMultipleResponse executeMultipleResponse = AddExecuteMultiple(requestCollection);

            foreach (var executeResponse in executeMultipleResponse.Responses)
            {
                if (executeResponse.Fault != null)
                    Console.WriteLine(executeResponse.Fault.ToString());
            }
        }

        private ExecuteMultipleResponse AddExecuteMultiple(OrganizationRequestCollection requestCollection)
        {
            ExecuteMultipleRequest executeMultipleRequest = new ExecuteMultipleRequest()
            {
                Requests = requestCollection,
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                }
            };

            ExecuteMultipleResponse executeMultipleResponse = (ExecuteMultipleResponse)this.ServiceClient.Execute(executeMultipleRequest);
            return executeMultipleResponse;
        }

        private static void AddUpsertRequest(OrganizationRequestCollection requestCollection, Entity entity)
        {
            UpsertRequest upsertRequest = new UpsertRequest()
            {
                Target = entity
            };

            requestCollection.Add(upsertRequest);
        }

        public Entity GetAccountByTelephone(string telephone1)
        {
            string fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' />
                                    <attribute name='primarycontactid' />
                                    <attribute name='telephone1' />
                                    <attribute name='accountid' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='telephone1' operator='eq' value='" + telephone1 + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";

            return this.ServiceClient.RetrieveMultiple(
                new FetchExpression(fetchXML)
            ).Entities.FirstOrDefault();
        }
    }
}
