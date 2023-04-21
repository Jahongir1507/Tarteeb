//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;

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
                (Rule: IsInvalid(milestone.Deadline), Parameter: nameof(milestone.Deadline)),
                (Rule: IsInvalid(milestone.CreatedDate), Parameter: nameof(milestone.CreatedDate)),
                (Rule: IsInvalid(milestone.UpdatedDate), Parameter: nameof(milestone.UpdatedDate)),
                (Rule: IsInvalid(milestone.AssigneeId), Parameter: nameof(milestone.AssigneeId)),
                (Rule: IsNotRecent(milestone.CreatedDate), Parameter: nameof(milestone.CreatedDate)),

                (Rule: IsNotSame(
                      firstDate: milestone.CreatedDate,
                      secondDate: milestone.UpdatedDate,
                      secondDateName: nameof(Milestone.UpdatedDate)),
                Parameter: nameof(Milestone.CreatedDate)));
        }

        private void ValidateMilestoneOnModify(Milestone milestone)
        {
            ValidateMilestoneNotNull(milestone);

            Validate(
                (Rule: IsInvalid(milestone.Id), Parameter: nameof(Milestone.Id)),
                (Rule: IsInvalid(milestone.Title), Parameter: nameof(Milestone.Title)),
                (Rule: IsInvalid(milestone.Deadline), Parameter: nameof(Milestone.Deadline)),
                (Rule: IsInvalid(milestone.CreatedDate), Parameter: nameof(Milestone.CreatedDate)),
                (Rule: IsInvalid(milestone.UpdatedDate), Parameter: nameof(Milestone.UpdatedDate)),
                (Rule: IsInvalid(milestone.AssigneeId), Parameter: nameof(Milestone.AssigneeId)),
                (Rule: IsNotRecent(milestone.UpdatedDate), Parameter: nameof(Milestone.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: milestone.UpdatedDate,
                    secondDate: milestone.CreatedDate,
                    secondDateName: nameof(milestone.CreatedDate)),
                Parameter: nameof(milestone.UpdatedDate)));
        }

        private static void ValidateAgainstStorageMilestoneOnModify(
           Milestone inputMilestone, Milestone storageMilestone)
        {
            ValidateStorageMilestoneExist(storageMilestone, inputMilestone.Id);

            Validate(
                 (Rule: IsNotSame(
                         firstDate: inputMilestone.CreatedDate,
                         secondDate: storageMilestone.CreatedDate,
                         secondDateName: nameof(Milestone.CreatedDate)),
                 Parameter: nameof(Milestone.CreatedDate)),

            (Rule: IsSame(
                  firstDate: inputMilestone.UpdatedDate,
                  secondDate: storageMilestone.UpdatedDate,
                  secondDateName: nameof(Milestone.UpdatedDate)),
          Parameter: nameof(Milestone.UpdatedDate)));
        }
        private static dynamic IsNotSame
          (DateTimeOffset firstDate,
          DateTimeOffset secondDate,
          string secondDateName) => new
          {
              Condition = firstDate != secondDate,
              Message = $"Date is not same as {secondDateName}."
          };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
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

        private static void ValidateStorageMilestoneExist(Milestone maybeMilestone, Guid milestoneId)
        {
            if (maybeMilestone is null)
            {
                throw new NotFoundMilestoneException(milestoneId);
            }
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
