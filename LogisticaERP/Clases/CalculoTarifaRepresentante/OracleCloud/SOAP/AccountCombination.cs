using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases.CalculoTarifaRepresentante.OracleCloud.SOAP
{
    public class AccountCombination : SOAPBase
    {
        public AccountCombination() { }

        public Tuple<string, string, string> ValidateAndCreateAccount(ValidateAndCreateAccountReq.Envelope envelope)
        {
            string xml = envelope.SerializeToXML();
            var response = this.Send<Tuple<string, string, string>>(
                xml,
                SOAPBase.Service.AccountConbinationService,
                @"http://xmlns.oracle.com/apps/financials/generalLedger/accounts/codeCombinations/accountCombinationService/validateAndCreateAccounts");

            return response;
        }
    }
}