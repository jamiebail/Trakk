using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.Repositories;

namespace API.Logic
{
    interface IReportLogic
    {

        List<GameReport> GetAllReports();
        GameReport GetReport(int reportId);

    }
}
