using Xeptions;

namespace Tarteeb.Api.Models.Processings.Exceptions
{
    public class NullUserProcessingException : Xeption
    {
        public NullUserProcessingException()
            : base(message: "User is null")
        { }
    }
}
