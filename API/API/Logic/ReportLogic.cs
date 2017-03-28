using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Repositories;

namespace API.Logic
{
    public class ReportLogic : IReportLogic
    {
        readonly IRepository<GameReport> _reportRepository = new Repository<GameReport>();

        public List<GameReport> GetAllReports()
        {
            return _reportRepository.GetAll();
        }

        public GameReport GetReport(int reportId)
        {
            return _reportRepository.FindBy(x => x.Id == reportId).FirstOrDefault();
        }
    }
}