using FluentValidation;
using YH.Etms.Settlement.Api.Application.Validations.Validators;

namespace YH.Etms.Settlement.Api.Application.Validations
{
    public static class CustomValidorExtension
    {
        public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new MobilePhoneValidator());
        }

        public static IRuleBuilderOptions<T, string> LicenseNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new LicenseNumberValidator());
        }

        public static IRuleBuilderOptions<T, string> IdentityCard<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IdentityCardValidator());
        }
    }
}
