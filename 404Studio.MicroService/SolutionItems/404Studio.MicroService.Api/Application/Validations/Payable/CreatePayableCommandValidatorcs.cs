using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;

namespace YH.Etms.Settlement.Api.Application.Validations.Payable
{
    public class CreatePayableCommandValidatorcs: AbstractValidator<InitPayableCommand>
    {
        public CreatePayableCommandValidatorcs()
        { }
    }
}
