using Hangfire.Dashboard;

namespace MazzaWebSite.Job
{
    public class MazzaAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            //TODO Authentication

            return true;
        }
    }

}