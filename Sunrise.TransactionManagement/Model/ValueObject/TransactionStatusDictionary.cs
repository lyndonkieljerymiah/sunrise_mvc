using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class TransactionStatusDictionary : StatusBaseDictionary
    {

        private string[] containableCode;

        public TransactionStatusDictionary(string code) : base(code)
        {
            
        }
        public TransactionStatusDictionary()
        {

        }

        public bool IsPending() => Code == "ssp" ? true : false;
        public bool IsActive() => Code == "sscn" ? true : false;

        public void Activate() => Code = "sscn";
        public void Pending() => Code = "ssp";
        public void Completed() => Code = "ssc";
        public void Cancelled() => Code = "sscc";

        public static TransactionStatusDictionary CreatePending()
        {
            var status = new TransactionStatusDictionary("ssp");
            return status;
        }

        public static TransactionStatusDictionary CreateActive()
        {
            var status = new TransactionStatusDictionary("sscn");
            return status;
        }

        public static string[] GetOfficials()
        {
            return new string[] { "ssp"};
        }
        public static string[] GetActive()
        {
            return new string[] { "sscn" };
        }

        public void SetOfficial()
        {
            containableCode = new string[] { "ssp"};
        }

        public void SetActive()
        {
            containableCode = new string[] { "sscn" };
        }

        public string[] GetContainableCodes()
        {
            return containableCode;
        }
        

    }
}
