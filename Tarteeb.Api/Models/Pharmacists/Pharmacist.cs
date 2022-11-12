//---------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//---------------------------------

using System;

namespace Tarteeb.Api.Models.Pharmacist
{
    public class Pharmacist
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string lastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int Salary { get; set; }
        public int Perquisite { get; set; }
    }
}
