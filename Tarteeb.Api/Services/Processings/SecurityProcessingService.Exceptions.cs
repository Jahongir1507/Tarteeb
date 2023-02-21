using Tarteeb.Api.Models.Users.Exceptions;

namespace Tarteeb.Api.Services.Processings
{
    public partial class SecurityProcessingService
    {
        private delegate string ReturningTokenFunction();
        
        private string TryCatch(ReturningTokenFunction returningTokenFunction)
        {
            try
            {
                return returningTokenFunction();
            }
            catch(NullUserException nullUserException)
            {

            }
        }
    }
}
