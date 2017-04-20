using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;
using API.Repositories;

namespace API.Logic
{
    interface IReportLogic
    {

        List<GameReport> GetAllReports();
        GameReport GetReport(int reportId);
        EntityResponse UpdateReport(GameReport report);
        EntityResponse CreateReport(GameReport report);
        GameReport GetFixtureReport(int fixtureId);
    }
}
