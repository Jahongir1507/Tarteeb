//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Milestones;

namespace Tarteeb.Api.Services.Foundations.Milestones
{
    public partial class MilestoneService : IMilestoneService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public MilestoneService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Milestone> AddMilestoneAsync(Milestone milestone) =>
        TryCatch(async () =>
        {
            ValidateMilestone(milestone);

            return await this.storageBroker.InsertMilestoneAsync(milestone);
        });

        public IQueryable<Milestone> RetrieveAllMilestones() =>
           TryCatch(() => storageBroker.SelectAllMilestones());

        public  ValueTask<Milestone> RemoveMilestoneByIdAsync(Guid milestoneId) =>
        TryCatch(async () =>
        {
            ValidateMilestoneId(milestoneId);
            var maybeMilestone = await this.storageBroker.SelectMilestoneByIdAsync(milestoneId);

            return await this.storageBroker.DeleteMilestoneAsync(maybeMilestone);
        });
    }
}
