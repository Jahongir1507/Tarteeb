using Xeptions;

namespace Tarteeb.Api.Models.Processings.Users
{
    public class NullUserProcessingException:Xeption
    {
        public NullUserProcessingException()
            :base(message:"User is null.") 
        { }
    }
}
