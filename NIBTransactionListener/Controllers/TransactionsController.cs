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

            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            string bodyText = bodyStream.ReadToEnd();

            NotifyEvent dataResponse = JsonConvert.DeserializeObject<NotifyEvent>(bodyText);

            NotififyEventResponse NIBResponse = new NotififyEventResponse();

            Random rnd = new Random();
            int single = rnd.Next(1, 10);
            NIBResponse.NotificationEventId = rnd.Next(1, 99999999).ToString();
            NIBResponse.ResponseCode = "00";
            NIBResponse.ResponseDescription = "Transaction was processed successfully";

            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NotififyEventResponse));
            ser.WriteObject(ms, NIBResponse);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);





            //MemoryStream stream1 = new MemoryStream();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NotififyEventResponse));
            //ser.WriteObject(stream1, NIBResponse);
            //StreamReader sr = new StreamReader(stream1);
            //return sr.ReadToEnd();



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
            public string accountholderaddressL1
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


}
}
