using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Description).MinimumLength(2);
            RuleFor(p => p.DailyPrice).GreaterThan(0);
            RuleFor(p => p.DailyPrice).GreaterThan(100).When(p => p.BrandId==1);
            RuleFor(p =>p.Description).Must(StartWithA).WithMessage("Araç açıklaması 'A' harfi ile başlamalı");
        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A",false,new CultureInfo("en-US"));
        }
    }
}
