//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public partial class NotFoundScoreException : Xeption
    {
        public NotFoundScoreException(Guid id)
            : base(message: $"Could not find score with id: {id}")
        { }
    }
}
