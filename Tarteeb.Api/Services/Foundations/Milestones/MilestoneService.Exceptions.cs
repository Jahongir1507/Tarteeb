//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
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
            catch (SqlException sqlException)
            {
                var failedMilestoneStorageException = new FailedMilestoneStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedMilestoneStorageException);
            }
        }

        private MilestoneDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var milestoneDependencyException = new MilestoneDependencyException(exception);
            this.loggingBroker.LogCritical(milestoneDependencyException);

            return milestoneDependencyException;
        }

        private MilestoneValidationException CreateAndLogValidationException(Xeption exception)
        {
            var milestoneValidationException = new MilestoneValidationException(exception);
            this.loggingBroker.LogError(milestoneValidationException);

            return milestoneValidationException;
        }
    }
}
