using System.ComponentModel.DataAnnotations;

namespace Tarteeb.Api.Models.EmailConfigurations
{
    public class EmailMessage
    {
        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
