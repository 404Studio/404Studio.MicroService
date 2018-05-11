using System.Text.RegularExpressions;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace YH.Etms.Settlement.Api.Application.Validations.Validators
{
    public class LicenseNumberValidator : PropertyValidator, IRegularExpressionValidator
    {
        private readonly Regex regex;
        private const string expression = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9港澳]{1}$";

        public LicenseNumberValidator()
        : base(new LanguageStringSource(nameof(LicenseNumberValidator)))
        {
            regex = RegexHelper.CreateRegEx(expression);
        }        

        public string Expression => expression;

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
