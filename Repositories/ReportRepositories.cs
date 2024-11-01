using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ReportRepositories : IReportRepositories
    {
        public List<dynamic> GetReports() => ReportDAO.GetReports();

    }
}
