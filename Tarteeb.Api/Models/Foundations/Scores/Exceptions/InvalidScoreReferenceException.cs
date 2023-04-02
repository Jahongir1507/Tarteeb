using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class InvalidScoreReferenceException : Xeption
    {
        public InvalidScoreReferenceException(Exception innerException)
            : base(message: "Invalid score reference error occured.", innerException)
        { } 
    }
}