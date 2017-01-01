using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;

namespace Sunrise.Client.Helpers.Validations
{
    
    public class CustomRequiredMatchToValidation : PropertyValidationAttribute
    {

        private string _matchTo;
        private ValidationResult _success;
        private ValidationResult _failure;

        /// <summary>
        /// TODO: Initialize object
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="matchTo"></param>
        public CustomRequiredMatchToValidation(string propertyName,string matchTo) : base(propertyName)
        {
            this._success = ValidationResult.Success;
            _matchTo = matchTo;
            this._failure = new ValidationResult(String.Empty);
        }


        #region Private Method
        private ValidationResult CompareValue(object value,ValidationContext validationContext)
        {
            string needToMatch = ConvertToString(GetValue(validationContext));

            if (needToMatch.IsEmpty())
                return this._failure = new ValidationResult("Nothing to compare");

            //if match means it is required
            if (needToMatch == _matchTo)
            {
                var outputValue = ConvertToString(value);
                if (outputValue.IsNullOrWhiteSpace() || outputValue.IsEmpty())
                {
                    return _failure;
                }
            }
            return _success;
        }

        private string ConvertToString(object value)
        {
            if(value == null)  
                return String.Empty;

            return Convert.ToString(value);

        }
        #endregion

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                return CompareValue(value, validationContext);
            }
            return this._success;
        }
    }
}
