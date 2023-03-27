﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Teams;
using Tarteeb.Api.Services.Foundations.Teams;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITeamService teamService;

        public TeamServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.teamService = new TeamService(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object);
        }

        public static TheoryData<int> InvalidSeconds()
        {
            int secondsInPast = -1 * new IntRange(
                min: 60,
                max: short.MaxValue).GetValue();

            int secondsInFuture = new IntRange(
                min: 0,
                max: short.MaxValue).GetValue();

            return new TheoryData<int>
            {
                secondsInPast,
                secondsInFuture
            };
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedExceptoin) =>
            actualException => actualException.SameExceptionAs(expectedExceptoin);

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Team CreateRandomModifyTeam(DateTimeOffset dates)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            Team randomTeam = CreateRandomTeam(dates);

            randomTeam.CreatedDate = randomTeam.CreatedDate.AddDays(randomDaysInPast);

            return randomTeam;
        }

        private static Team CreateRandomTeam(DateTimeOffset dates) =>
            CreateTeamFiller(dates).Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 99).GetValue();

        private static Team CreateRandomTeam() =>
            CreateTeamFiller(GetRandomDateTime()).Create();

        private static IQueryable<Team> CreateRandomTeams()
        {
            return CreateTeamFiller(dates: GetRandomDateTime())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<Team> CreateTeamFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Team>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}