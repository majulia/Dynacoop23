if (typeof (AlfaPeople) == "undefined") { AlfaPeople = {} }
if (typeof (AlfaPeople.Product) == "undefined") { AlfaPeople.Product = {} }

AlfaPeople.Account = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

        debugger;
    },

    CnpjOnChange: function (indexedDB) {

        var formContext = indexedDB.getFormContext();


        var cnpj = formContext.getAttribute("dcp_cnpj").getValue();
        if (true) {
            if (cnpj.length == 14) {
                var formatterCNPJ = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
                var id = Xrm.Page.data.entity.getId();

                var queryAccount = "";

                if (id.length > 0) {
                    queryAccount += "anda accountid ne" + id;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("account", "?$select=name&$filter=dcp_cnpj eq" + formattedCNPJ + "" + queryAccountId).then(

                    function sucess(results) {
                        if (results.entities.length == 0) {
                            formContext.getAttribute("dcp_cnpj").setValue(formatterCNPJ);
                        } else {
                            formContext.getAttribute("dcp_cnpj").setValue();
                            AlfaPeople.Account.DynamicsAlert("CNPJ existe no sistema", "CNPJ duplicado")
                        }

                    },

                    function (error) {
                        AlfaPeople.Account.DynamicsAlert("Por favor, entre em contato com o administrador", "Erro no sistemas");
                    }
                );
            },

            DynamicsAlert: function(alertText, alertTitle) {
                var alertStrings = {
                    confirmButtonLabel: "Ok",
                    text: alertText,
                    title: alertTitle
                };

                var alertOptions = {
                    height: 120,
                    width: 200
                }
            }
        }
    }


}
FormControl: function() {
    //Desabilitar
    formContext.getControl("industrycontact").setDisabled(true);

    //Obrigatório
    formContext.getAttribute("primarycontactid").setRequiredLevel("required");

    //Pegar o controle = manipuladores
    formContext.getControl("fax").setVisible(true);
}


GetSet: function() {
    //get inteiros
    var numeroTotalOpp = formsContext.getAttribute("dcp_nmr_total_opp").getValue();

    //get Nome
    var nomeDaConta = formContext.getAttribute("name").setValue();

    //pickList
    //seguros = 20
    var setor = formContext.getAttribute("industrycode").getValue();

    //Money ou decimal
    var valor = formContext.getAttribute("dcp_valor_total_opp").getValue();


    //Consulta
    var lookup = formContext.getAttribute("primarycontactid").getValue();


    //set
    //Numero Inteiro
    formContext.getAttribute("dcp_nmr_total_opp").setValue(10);

    //Texto
    formContext.getAttribute("Name").setValue("Novo nome da Conta!");//Texto

    //PickList
    formContext.getAttribute("industrycode").setValue(20);

    //Money ou decimal
    formContext.getAttribute("dcp_valor_total_opp").setValue(10.50);

    //lookup
    var lookup = [];
    lookup[0] = {};
    lookup[0].name = { "Qualquer nome"};
    lookup.id = "";
    lookup[0].entityType = "contact"; //sempre a tabela
    formContext.getAttribute("primarycontactid").setValue(lookup);


}
}