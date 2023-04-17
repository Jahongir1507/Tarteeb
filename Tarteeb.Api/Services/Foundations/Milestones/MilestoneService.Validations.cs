//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Milestones
{
    public partial class MilestoneService
    {
        private void ValidateMilestone(Milestone milestone)
        {
            ValidateMilestoneNotNull(milestone);
        }

        private static void ValidateMilestoneNotNull(Milestone milestone)
        {
            if (milestone is null)
            {
                throw new NullMilestoneException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMilestoneException = new InvalidMilestoneException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMilestoneException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMilestoneException.ThrowIfContainsErrors();
        }
    }
}
