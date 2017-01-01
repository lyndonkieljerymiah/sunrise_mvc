using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Helpers.Validations
{
    public class CustomDateCurrentValidation : ValidationAttribute
    {
        public CustomDateCurrentValidation()
        {
            
        }

        private bool IsValid(DateTime value)
        {
            var result = value.Date.CompareTo(DateTime.Today.Date.AddDays(-1));
            if (result < 0)
            {
                return false;
            }
            return true;
        }

 
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return IsValid(Convert.ToDateTime(value));
            }
            return false;
        }
    }
}
