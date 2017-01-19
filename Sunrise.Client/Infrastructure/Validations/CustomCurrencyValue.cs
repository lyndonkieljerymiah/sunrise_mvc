using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Infrastructure.Validations
{
    public class CustomCurrencyValue : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int outValue = 0;
                var parseValue = int.TryParse(Convert.ToString(value), out outValue);
                if(!parseValue)
                    return new ValidationResult("Value should be numeric");
                if(outValue == 0)
                    return new ValidationResult("Amount should have value");
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Amount must no be empty");
            }
        }
    }
}
