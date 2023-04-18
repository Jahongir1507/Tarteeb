//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Milestones
{
    public partial class MilestoneService
    {
        private delegate ValueTask<Milestone> ReturningMilestoneFunction();

        private async ValueTask<Milestone> TryCatch(ReturningMilestoneFunction returningMilestoneFunction)
        {
            try
            {
                return await returningMilestoneFunction();
            }
            catch (NullMilestoneException nullMilestoneException)
            {
                throw CreateAndLogValidationException(nullMilestoneException);
            }
            catch (InvalidMilestoneException invalidMilestoneException)
            {
                throw CreateAndLogValidationException(invalidMilestoneException);
            }
            catch (MilestoneValidationException milestoneValidationException)
            {
                throw CreateAndLogValidationException(milestoneValidationException);
            }
        }

        private MilestoneValidationException CreateAndLogValidationException(Xeption exception)
        {
            var milestoneValidationException = new MilestoneValidationException(exception);
            this.loggingBroker.LogError(milestoneValidationException);

            return milestoneValidationException;
        }
    }
}
