
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Globalization;

namespace TomGrable.Website.Extensions.MvcExtensions.ComponentModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class BooleanValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public bool Value
        {
            get;
            set;
        }

        public override bool IsValid(object value)
        {
            return value is bool && (bool)value == Value;
        }

        public override string FormatErrorMessage(string name)
        {
            return Translate.Text(ErrorMessageString, name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "checkrequired"
            };
        }
    }
}
