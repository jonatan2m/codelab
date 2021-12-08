using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web3_1.MapperExamples;

namespace Web3_1.FluentValidationExamples
{
    /// <summary>
    /// It is possible to inject other services here, to call a repository, for example
    /// </summary>
    public class EventViewModelValidator : FluentValidation.AbstractValidator<EventViewModel>
    {
        public EventViewModelValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("{PropertyName} is empty")
                .MinimumLength(5)
                .WithMessage("{PropertyName} requires more than 5 charactes");
            RuleFor(e => e)
                .MustAsync(EventTitleValidateEventBriteCodeIfExists)
                .WithMessage("Event Brite code is invalid.");
        }
        
        public async Task<bool> EventTitleValidateEventBriteCodeIfExists(EventViewModel @event, System.Threading.CancellationToken cancellationToken)
        {
            if (@event.EventBriteCode > 0)
            {                
                //call the EventBrite API and if they find, returns true, if not, returns false
                return @event.EventBriteCode % 2 == 0;
            }

            return await Task.FromResult(true);
        }
    }
}
