//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Users.Exceptions
{
    public class FailedUserServiceException :Xeption
    {
        public FailedUserServiceException(Exception innerException)
            :base (message:"Failed profile service occured,please contact support",
                 innerException)
        {}
    }
}
