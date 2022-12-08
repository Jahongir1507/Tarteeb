//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Tickets
{
    public partial class TicketService
    {
        private delegate ValueTask<Ticket> ReturningTicketFunction();

        private async ValueTask<Ticket> TryCatch(ReturningTicketFunction returningTicketFunction)
        {
            try
            {
                return await returningTicketFunction();
            }
            catch (NullTicketException nullTicketException)
            {
                throw CreateAndLogValidationException(nullTicketException);
            }
            catch (InvalidTicketException invalidTicketException)
            {
                throw CreateAndLogValidationException(invalidTicketException);
            }
            catch (SqlException sqlException)
            {
                var failedTicketStorageException = new FailedTicketStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTicketStorageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTicketReferenceException =
                    new InvalidTicketReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndDependencyValidationException(invalidTicketReferenceException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var failedTicketDependencyValidationException =
                     new AlreadyExistsTicketException(duplicateKeyException);

                throw CreateAndDependencyValidationException(failedTicketDependencyValidationException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTickedException = new LockedTicketException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedTickedException);
            }
            catch (Exception serviceException)
            {
                var failedServiceProfileException = new FailedTicketServiceException(serviceException);
                
                throw CreateAndLogServiceException(failedServiceProfileException);
            }
        }

        private Exception CreateAndLogServiceException(Xeption exception)
        {
            var ticketServiceException = new TicketServiceException(exception);
            this.loggingBroker.LogError(ticketServiceException);

            return ticketServiceException;
        }

        private TicketValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ticketValidationException = new TicketValidationException(exception);
            this.loggingBroker.LogError(ticketValidationException);

            return ticketValidationException;
        }

        private TicketDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ticketDependencyException = new TicketDependencyException(exception);
            this.loggingBroker.LogCritical(ticketDependencyException);

            return ticketDependencyException;
        }

        private TicketDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var ticketDependencyValidationException = new TicketDependencyValidationException(exception);
            this.loggingBroker.LogError(ticketDependencyValidationException);

            return ticketDependencyValidationException;
        }
    }
}
