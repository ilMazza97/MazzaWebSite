using Hangfire;
using MazzaWebSite.Models;
using System;
using System.Linq;

namespace MazzaWebSite.Job
{
    public class HangfireJobManager
    {
        private static readonly MazzaDbContext db = new MazzaDbContext();
        public static void AddHangfireJob()
        {
            foreach (var job in db.Jobs.Where(j=>j.Active))
                RecurringJob.AddOrUpdate(job.JobName, () => Console.WriteLine("Daily Job"), job.JobCronExpression);
            
        }
    }
}