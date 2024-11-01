using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepositories reportRepositories;
        public ReportService()
        {
            reportRepositories = new ReportRepositories();
        }
        public List<dynamic> GetReports()
        {
            return reportRepositories.GetReports();
        }

    }
}
