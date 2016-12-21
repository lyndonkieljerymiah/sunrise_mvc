using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunrise.Client.Domains.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public string Term { get; set; }

        public string ChequeNo { get; set; }

        public DateTime Date { get; set; }

        public decimal Currency { get; set; }


        public IEnumerable<SelectListItem> Terms
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Cheque", Value = "cheque"},
                    new SelectListItem() {Text = "Cash", Value = "cash"}
                };
            }
            
        }

    }
}
