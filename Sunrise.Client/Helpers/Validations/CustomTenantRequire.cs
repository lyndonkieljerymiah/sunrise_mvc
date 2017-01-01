using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Helpers.Validations
{
    public class CustomTenantRequire : ValidationAttribute
    {
        public string Exception { get; set; }

        public CustomTenantRequire(string exception)
        {
            Exception = exception;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}
