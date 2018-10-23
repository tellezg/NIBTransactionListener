using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using NIBTransactionListener.Filters;
using NIBTransactionListener.Models;
using System.Globalization;


namespace NIBTransactionListener.Controllers
{
    [BasicAuthentication]
    public class TransactionsController : ApiController
    {
        // POST api/values
        public string Post([FromBody]string value)
        {

            //            {
            //                "source" : "LASistemas",
            //	"userid": "RXkaCz2ENI",
            //	"userpassword": "NRUewd1gwKiLj92" ,
            //	"pushid" : 112414 ,


            //	"transactionid": "A455414",
            //	"transactiondateTime": "",
            //	"transactiontype": "Deposit",
            //	"debitcredit": "C",
            //	"amount": 1506.05,
            //	"currency": "USD",
            //	"status": "APPROVED",
            //	"authorizationid": "5987101",
            //	"originalTransid": "0",


            //	"accountnumber": "01054906",
            //	"accounttype": "CHECKING",
            //	"description": "DEPOSIT VIA WIRE TRANSFER",
            //	"reference": "A-89774412-4414",
            //	"accountholderteference": "PAYROLL",


            //	"customerid": "778441541",
            //	"availablebalance": 3578.36,
            //	"accountHolderfirstname": "JOANTHAN",
            //	"accountHolderlastname": "BARRIOS",
            //	"accountHolderaddressL1": "CALLE 122 # 14-98",
            //	"accountHolderaddressl2": "APTO 204",
            //	"accountHoldercity": "BOGOTA",
            //	"accountHolderstate": "DC",
            //	"accountHolderpostalcode": "10010",
            //	"accountHoldercountrycode": "CO",
            //	"accountHolderphonenumber": "573102088821",
            //	"accountHolderemailaddress" : "JBARRIOS@NIBANK.COM"
            //}

            try
            {
                var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                string bodyText = bodyStream.ReadToEnd();
                NotifyEvent dataReceived = JsonConvert.DeserializeObject<NotifyEvent>(bodyText);

                NotififyEventResponse NIBResponse = new NotififyEventResponse();

                bool parameterErrors = false;
                string errordescription = " ";

                // validates information received
                // Sources
                if (dataReceived.source == null || dataReceived.source.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing source";

                }
                else
                {
                    string[] strSources = null;
                    char[] splitchar = { ',' };
                    strSources = Properties.Settings.Default.sources.Split(splitchar);
                    if (strSources.Count(x => x.Contains(dataReceived.source)) < 1)
                    {
                        parameterErrors = true;
                        errordescription += " -- Invalid source";
                    }
                }

                // pushid
                if (dataReceived.pushid == null || dataReceived.pushid.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing pushid";
                }

                // transactionid
                if (dataReceived.transactionid == null)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing transactionid";
                }

                // transactiondateTime  
                if (dataReceived.transactiondateTime != null)
                {
                    if (validatenumbers(dataReceived.transactiondateTime))
                    {
                        try
                        { 
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dataReceived.transactiondateTime));
                        DateTime dateTime = dateTimeOffset.UtcDateTime;
                        }
                        catch (Exception ex1)
                        {
                            parameterErrors = true;
                            errordescription += " -- Invalid transactiondateTime " + ex1.Message;
                        }

                    }
                }
                else
                {
                    parameterErrors = true;
                    errordescription += " -- Missing transactiondateTime";
                }

                //transactiontype
                if (dataReceived.transactiontype == null || dataReceived.transactiontype.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing transactiontype";
                }


                //debitcredit
                if (dataReceived.debitcredit == null || dataReceived.debitcredit.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing debitcredit";
                }
                else
                {
                    string[] strSources = null;
                    char[] splitchar = { ',' };
                    strSources = Properties.Settings.Default.debitcredit.Split(splitchar);
                    if (strSources.Count(x => x.Contains(dataReceived.debitcredit)) < 1)
                    {
                        parameterErrors = true;
                        errordescription += " -- Invalid debitcredit";
                    }
                }

                // amount
                if (dataReceived.amount == null || dataReceived.amount.Length < 1)
                {
                        parameterErrors = true;
                        errordescription += " -- Missing amount";
                }
                else
                {
                    if (!validatenumbers(dataReceived.amount))
                    {
                            parameterErrors = true;
                            errordescription += " -- Invalid validatenumbers";
                    }
                }

                //currency
                if (dataReceived.currency == null || dataReceived.currency.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing currency";
                }
                else
                {
                    if (validatecurrency(dataReceived.currency) < 1)
                    {
                        parameterErrors = true;
                        errordescription += " -- Invalid currency";
                    }
                }

                // status
                if (dataReceived.status == null || dataReceived.status.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing status";
                }

                //accountnumber
                if (dataReceived.accountnumber == null || dataReceived.accountnumber.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountnumber";
                }

                // customerid
                if (dataReceived.customerid == null || dataReceived.customerid.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing customerid";
                }

                //accountHolderfirstname
                if (dataReceived.accountholderfirstname == null || dataReceived.accountholderfirstname.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholderfirstname";
                }

                // accountholderaddressl1
                if (dataReceived.accountholderaddressl1 == null || dataReceived.accountholderaddressl1.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholderaddressl1";
                }

                // accountHoldercity
                if (dataReceived.accountholdercity == null || dataReceived.accountholdercity.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholdercity";
                }

                // accountHolderstate
                if (dataReceived.accountholderstate == null || dataReceived.accountholderstate.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholderstate";
                }

                // accountHolderpostalcode
                if (dataReceived.accountholderpostalcode == null || dataReceived.accountholderpostalcode.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholderpostalcode";
                }

                // accountHoldercountrycode

                if (dataReceived.accountholdercountrycode == null || dataReceived.accountholdercountrycode.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholdercountrycode";
                }
                else
                {
                    if (validatecountrycode(dataReceived.accountholdercountrycode) < 1)
                    {
                        parameterErrors = true;
                        errordescription += " -- Invalid accountholdercountrycode";
                    }
                }

                // accountHolderphonenumber
                if (dataReceived.accountholderphonenumber == null || dataReceived.accountholderphonenumber.Length < 1)
                {
                    parameterErrors = true;
                    errordescription += " -- Missing accountholderphonenumber";
                }

                // 




                Random rnd = new Random();
                int single = rnd.Next(1, 10);
                NIBResponse.NotificationEventId = rnd.Next(1, 99999999).ToString();

                if (parameterErrors)
                {
                            NIBResponse.ResponseCode = "12";
                            NIBResponse.ResponseDescription = errordescription;

                }
                else
                {                
                            NIBResponse.ResponseCode = "00";
                            NIBResponse.ResponseDescription = "Transaction was processed successfully";
                }
                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NotififyEventResponse));
                ser.WriteObject(ms, NIBResponse);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);

            }
            catch (Exception ex)
            {
                return ex.Message; 
            }





        }


        public class NotifyEvent
        {
            private string sourceField;
            private string useridField;
            private string userpasswordField;
            private string pushidField;
            private string transactionidField;
            private string transactiondateTimeField;
            private string transactiontypeField;
            private string debitcreditField;
            private string amountField;
            private string currencyField;
            private string statusField;
            private string authorizationidField;
            private string originalTransidField;
            private string accountnumberField;
            private string accounttypeField;
            private string descriptionField;
            private string referenceField;
            private string accountholderteferenceField;
            private string customeridField;
            private string availablebalanceField;
            private string accountholderfirstnameField;
            private string accountholderlastnameField;
            private string accountholderaddressL1Field;
            private string accountholderaddressl2Field;
            private string accountholdercityField;
            private string accountholderstateField;
            private string accountholderpostalcodeField;
            private string accountholdercountrycodeField;
            private string accountholderphonenumberField;
            private string accountholderemailaddressField;

            [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
            public string source
            {
                get
                {
                    return this.sourceField;
                }
                set
                {
                    this.sourceField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
            public string userid
            {
                get
                {
                    return this.useridField;
                }
                set
                {
                    this.useridField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
            public string userpassword
            {
                get
                {
                    return this.userpasswordField;
                }
                set
                {
                    this.userpasswordField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
            public string pushid
            {
                get
                {
                    return this.pushidField;
                }
                set
                {
                    this.pushidField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
            public string transactionid
            {
                get
                {
                    return this.transactionidField;
                }
                set
                {
                    this.transactionidField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
            public string transactiondateTime
            {
                get
                {
                    return this.transactiondateTimeField;
                }
                set
                {
                    this.transactiondateTimeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
            public string transactiontype
            {
                get
                {
                    return this.transactiontypeField;
                }
                set
                {
                    this.transactiontypeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
            public string debitcredit
            {
                get
                {
                    return this.debitcreditField;
                }
                set
                {
                    this.debitcreditField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
            public string amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
            public string currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
            public string status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
            public string authorizationid
            {
                get
                {
                    return this.authorizationidField;
                }
                set
                {
                    this.authorizationidField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
            public string originalTransid
            {
                get
                {
                    return this.originalTransidField;
                }
                set
                {
                    this.originalTransidField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
            public string accountnumber
            {
                get
                {
                    return this.accountnumberField;
                }
                set
                {
                    this.accountnumberField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
            public string accounttype
            {
                get
                {
                    return this.accounttypeField;
                }
                set
                {
                    this.accounttypeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
            public string description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
            public string reference
            {
                get
                {
                    return this.referenceField;
                }
                set
                {
                    this.referenceField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
            public string accountholderteference
            {
                get
                {
                    return this.accountholderteferenceField;
                }
                set
                {
                    this.accountholderteferenceField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
            public string customerid
            {
                get
                {
                    return this.customeridField;
                }
                set
                {
                    this.customeridField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
            public string availablebalance
            {
                get
                {
                    return this.availablebalanceField;
                }
                set
                {
                    this.availablebalanceField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
            public string accountholderfirstname
            {
                get
                {
                    return this.accountholderfirstnameField;
                }
                set
                {
                    this.accountholderfirstnameField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
            public string accountholderlastname
            {
                get
                {
                    return this.accountholderlastnameField;
                }
                set
                {
                    this.accountholderlastnameField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
            public string accountholderaddressl1
            {
                get
                {
                    return this.accountholderaddressL1Field;
                }
                set
                {
                    this.accountholderaddressL1Field = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
            public string accountholderaddressl2
            {
                get
                {
                    return this.accountholderaddressl2Field;
                }
                set
                {
                    this.accountholderaddressl2Field = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
            public string accountholdercity
            {
                get
                {
                    return this.accountholdercityField;
                }
                set
                {
                    this.accountholdercityField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
            public string accountholderstate
            {
                get
                {
                    return this.accountholderstateField;
                }
                set
                {
                    this.accountholderstateField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
            public string accountholderpostalcode
            {
                get
                {
                    return this.accountholderpostalcodeField;
                }
                set
                {
                    this.accountholderpostalcodeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
            public string accountholdercountrycode
            {
                get
                {
                    return this.accountholdercountrycodeField;
                }
                set
                {
                    this.accountholdercountrycodeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
            public string accountholderphonenumber
            {
                get
                {
                    return this.accountholderphonenumberField;
                }
                set
                {
                    this.accountholderphonenumberField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
            public string accountholderemailaddress
            {
                get
                {
                    return this.accountholderemailaddressField;
                }
                set
                {
                    this.accountholderemailaddressField = value;
                }
            }

        }
    


        public partial class NotififyEventResponse
        {
            private string ResponseCodeField;
            private string ResponseDescriptionField;
            private string NotificationEventIdField;
            [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
            public string ResponseCode
            {
                get
                {
                    return this.ResponseCodeField;
                }
                set
                {
                    this.ResponseCodeField = value;
                }
            }
            [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
            public string ResponseDescription
        {
                get
                {
                    return this.ResponseDescriptionField;
                }
                set
                {
                    this.ResponseDescriptionField = value;
                }
            }
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string NotificationEventId
        {
            get
            {
                return this.NotificationEventIdField;
            }
            set
            {
                this.NotificationEventIdField = value;
            }
        }

    }


        public int validatecurrency(string currency)
        {
            IEnumerable<string> currencySymbols = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
            .Distinct()
            .OrderBy(x => x);

            return currencySymbols.Count(x => x.Contains(currency));
        }


        public int validatecountrycode(string country)
        {
            IEnumerable<string> currencySymbols = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(x => (new RegionInfo(x.LCID)).TwoLetterISORegionName)
            .Distinct()
            .OrderBy(x => x);

            return currencySymbols.Count(x => x.Contains(country));
        }


        public bool validatenumbers(string number)
        {
            return double.TryParse(number, out double n);
        }

    }
}
