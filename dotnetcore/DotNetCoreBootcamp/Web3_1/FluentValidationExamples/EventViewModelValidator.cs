using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web3_1.MapperExamples;

namespace Web3_1.FluentValidationExamples
{
    public class EventViewModelValidator : FluentValidation.AbstractValidator<EventViewModel>
    {
        public EventViewModelValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("{PropertyName} is empty")
                .MinimumLength(5)
                .WithMessage("{PropertyName} requires more than 5 charactes");
        }
    }
}
