using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProSchool.Web.Models
{
    public class DashboardViewModel
    {
        public int StudentActiveCount { get; set; }
        public int StudentInActiveCount { get; set; }

        public int OverDueCounts { get; set; }
        public int NextDueCounts { get; set; }
    }
}