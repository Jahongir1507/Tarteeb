//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Data;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Tarteeb.Api.Models.Foundations.Users;

namespace Tarteeb.Api.Services.Foundations.Milestones
{
    public partial class MilestoneService
    {
        private void ValidateMilestone(Milestone milestone)
        {
            ValidateMilestoneNotNull(milestone);

            Validate(
                (Rule: IsInvalid(milestone.Id), Parameter: nameof(milestone.Id)),
                (Rule: IsInvalid(milestone.Title), Parameter: nameof(milestone.Title)),
                (Rule: IsInvalid(milestone.Discription), Parameter: nameof(milestone.Discription)),
                (Rule: IsInvalid(milestone.Deadline), Parameter: nameof(milestone.Deadline)),
                (Rule: IsInvalid(milestone.CreatedDate), Parameter: nameof(milestone.CreatedDate)),
                (Rule: IsInvalid(milestone.UpdatedDate), Parameter: nameof(milestone.UpdatedDate)),
                (Rule: IsInvalid(milestone.AssigneeId), Parameter: nameof(milestone.AssigneeId))
                );
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

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
