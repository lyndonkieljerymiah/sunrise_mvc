﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Helpers.Validations
{

    /// <summary> 
    /// Defines supported comparisons between values. 
    /// </summary> 
    public enum ValueComparison : int
    {
        /// <summary> 
        /// Values are compared to be equal. 
        /// </summary> 
        IsEqual = 0,

        /// <summary> 
        /// Values are compared to be inequal. 
        /// </summary> 
        IsNotEqual = 1,

        /// <summary> 
        /// Value is compared to be greater than other value. 
        /// </summary> 
        IsGreaterThan = 2,

        /// <summary> 
        /// Value is compared to be greater than or equal to other value. 
        /// </summary> 
        IsGreaterThanOrEqual = 3,

        /// <summary> 
        /// Value is compared to be less than other value. 
        /// </summary> 
        IsLessThan = 4,

        /// <summary> 
        /// Value is compared to be less than or equal to other value. 
        /// </summary> 
        IsLessThanOrEqual = 5
    }


    /// <summary> 
    /// Base class for performing validation between two properties. 
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class PropertyValidationAttribute : ValidationAttribute
    {
        #region Fields 

        private readonly string propertyName;
        private object value;

        #endregion

        #region Ctor 

        /// <summary> 
        /// Initializes new instance of the <see cref="PropertyValidationAttribute"/> class. 
        /// </summary> 
        /// <param name="propertyName">The name of the other property.</param> 
        /// <exception cref="ArgumentNullException">If <paramref name="propertyName"/> is <c>null</c>, empty or whitespace.</exception> 
        protected PropertyValidationAttribute(string propertyName)
            : base()
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            this.propertyName = propertyName;
        }
       
        #endregion

        #region Properties 

        /// <summary> 
        /// Gets whether or not <see cref="ValidationContext"/> is required. 
        /// </summary> 
        public override bool RequiresValidationContext
        {
            get { return true; }
        }

        /// <summary> 
        /// Gets the name of the other property. 
        /// </summary> 
        protected string PropertyName
        {
            get { return this.propertyName; }
        }

        #endregion

        #region Methods 

        /// <summary> 
        /// Gets the value of the other property. 
        /// </summary> 
        /// <param name="validationContext">The context of the validation.</param> 
        /// <returns>A value of the other property.</returns> 
        /// <exception cref="InvalidOperationException">If object type of the validation context does not contain <see cref="PropertyName"/> property.</exception> 
        /// <exception cref="NotSupportedException">If property requires indexer parameters.</exception> 
        protected object GetValue(ValidationContext validationContext)
        {
            Type type = validationContext.ObjectType;
            PropertyInfo property = type.GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            if (property == null)
                throw new InvalidOperationException(String.Format("Type {0} does not contains public instance property {1}.", type.FullName, PropertyName));

            if (IsIndexerProperty(property))
                throw new NotSupportedException("Property with indexer parameters is not supported.");

            value = property.GetValue(validationContext.ObjectInstance);

            return value;
        }

        private bool IsIndexerProperty(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            return (parameters != null && parameters.Length > 0);
        }

        #endregion
    }
}
