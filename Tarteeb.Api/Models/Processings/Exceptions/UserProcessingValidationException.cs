using Xeptions;

namespace Tarteeb.Api.Models.Processings.Exceptions
{
    public class UserProcessingValidationException : Xeption
    {
        public UserProcessingValidationException(Xeption innerException)
            : base(message: "User validation error occurred, please try again", innerException)
        { }
    }
}
