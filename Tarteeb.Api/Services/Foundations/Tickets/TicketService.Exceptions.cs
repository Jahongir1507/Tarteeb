//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Tickets.Exception;
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
        }

        public TicketValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ticketValidationException =
                new TicketValidationException(exception);

            this.loggingBroker.LogError(ticketValidationException);

            return ticketValidationException;
        }
    }
}
