using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProSchool.Web.Models
{
    public class ReportViewModels
    {
        public string DataSetName { get; set; }
        public string ReportPath { get; set; }
        public object ReportParameters { get; set; }
        public object ReportDataSource { get; set; }

    }

    public class InvoiceReportModel
    {
        public int Id { get; set; }
        public List<ProSchool.Services.Models.InvoiceDataset> ReportDataSource { get; set; }
    }

}