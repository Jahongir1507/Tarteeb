//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Milestones
{
    public partial class MilestoneService
    {
        private delegate ValueTask<Milestone> ReturningMilestoneFunction();
        private delegate IQueryable<Milestone> ReturnigMilestonesFunction();

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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var failedMilestoneDependencyValidationException =
                    new AlreadyExistsMilestoneException(duplicateKeyException);

                throw CreateAndLogMilestoneDependencValidationException(failedMilestoneDependencyValidationException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidMilestoneReferenceException =
                    new InvalidMilestoneReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndDependencyValidationException(invalidMilestoneReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedMilestoneException =
                    new LockedMilestoneException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedMilestoneException);
            }
            catch (Exception serviceException)
            {
                var failedMilestoneServiceException = new FailedMilestoneServiceException(serviceException);

                throw CreateAndLogServiceException(failedMilestoneServiceException);
            }
        }

        private IQueryable<Milestone> TryCatch(ReturnigMilestonesFunction returnigMilestonesFunction)
        {
            try
            {
                return returnigMilestonesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedMilestoneStorageException = new FailedMilestoneStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedMilestoneStorageException);
            }
            catch (Exception exception)
            {
                var failedMilestoneServiceException = new FailedMilestoneServiceException(exception);

                throw CreateAndLogServiceException(failedMilestoneServiceException);
            }
        }

        private MilestoneServiceException CreateAndLogServiceException(
          Exception exception)
        {
            var milestoneServiceException = new MilestoneServiceException(exception);
            this.loggingBroker.LogError(milestoneServiceException);

            return milestoneServiceException;
        }

        private MilestoneDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var milestoneDependencyValidationException = new MilestoneDependencyValidationException(exception);
            this.loggingBroker.LogError(milestoneDependencyValidationException);

            return milestoneDependencyValidationException;
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

        private MilestoneDependencyValidationException CreateAndLogMilestoneDependencValidationException(
            Xeption exception)
        {
            var milestoneDependencyValidationException = new MilestoneDependencyValidationException(exception);
            this.loggingBroker.LogError(milestoneDependencyValidationException);

            return milestoneDependencyValidationException;
        }
    }
}
