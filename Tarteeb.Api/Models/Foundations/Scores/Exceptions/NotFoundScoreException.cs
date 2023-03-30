//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public partial class NotFoundScoreException : Xeption
    {
        public NotFoundScoreException(Guid scoreId)
         : base(message: $"Couldn't find score with id: {scoreId}.")
        { }
    }
}
