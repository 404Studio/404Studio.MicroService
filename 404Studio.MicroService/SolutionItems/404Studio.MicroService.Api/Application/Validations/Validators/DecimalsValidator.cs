using FluentValidation.Resources;
using FluentValidation.Validators;

namespace YH.Etms.Settlement.Api.Application.Validations.Validators
{
    public class DecimalsValidator : PropertyValidator
    {
        private int _maxDiligit = 0;    //默认0位

        public DecimalsValidator() : base(new LanguageStringSource(nameof(DecimalsValidator)))
        {

        }

        public DecimalsValidator(int maxDiligit) :base(new LanguageStringSource(nameof(DecimalsValidator)))
        {
            _maxDiligit = maxDiligit;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;
            var value = context.PropertyValue.ToString();
            var dotIndex = value.IndexOf('.');
            if (dotIndex > -1)
                return value.Split('.')[dotIndex].Length <= _maxDiligit;
            return true;
        }
    }
}
