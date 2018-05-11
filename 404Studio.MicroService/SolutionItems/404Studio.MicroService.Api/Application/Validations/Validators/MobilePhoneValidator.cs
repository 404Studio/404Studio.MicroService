using System.Text.RegularExpressions;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace YH.Etms.Settlement.Api.Application.Validations.Validators
{
    public class MobilePhoneValidator : PropertyValidator, IRegularExpressionValidator
    {
        private readonly Regex regex;
        private const string expression = @"^(0|86|17951)?(13[0-9]|15[012356789]|17[013678]|18[0-9]|14[57])[0-9]{8}$";

        public MobilePhoneValidator()
        : base(new LanguageStringSource(nameof(MobilePhoneValidator)))
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
