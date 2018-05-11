using System;
using System.Text.RegularExpressions;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace YH.Etms.Settlement.Api.Application.Validations.Validators
{
    public class IdentityCardValidator : PropertyValidator, IRegularExpressionValidator
    {
        private readonly Regex regex;
        private const string expression = @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$";

        public string Expression => throw new NotImplementedException();

        public IdentityCardValidator()
        : base(new LanguageStringSource(nameof(LicenseNumberValidator)))
        {
            regex = RegexHelper.CreateRegEx(expression);
        }    

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;

            if (!regex.IsMatch((string)context.PropertyValue))
            {
                return false;
            }
            return true;
        }
    }
}
