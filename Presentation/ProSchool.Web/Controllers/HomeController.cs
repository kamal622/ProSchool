using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProSchool.Web.Models;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ProSchool.Web.Framework;
using ProSchool.Web.Framework.Controllers;
using ProSchool.Services.Academics;
using ProSchool.Services.Finance;
using System;
using System.Linq;

namespace ProSchool.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly StudentService _studentService;
        private readonly StudentProgramInvoiceService _studentprograminvoiceService;
        private readonly StudentProgramService _studentProgramService;
        public HomeController(StudentService studentService, StudentProgramInvoiceService studentprograminvoiceService, StudentProgramService studentProgramService)
        {
            this._studentService = studentService;
            this._studentprograminvoiceService = studentprograminvoiceService;
            this._studentProgramService = studentProgramService;
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }

        }
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            bool[] activeStatus = this._studentService.GetStudentActiveStatus();
            DateTime[] dueDates = this._studentProgramService.GetDueDates();
            model.StudentActiveCount = activeStatus.Count(w => w == true);
            model.StudentInActiveCount = activeStatus.Count(w => w == false);
            model.OverDueCounts = dueDates.Count(w => w <= DateTime.UtcNow);
            DateTime dtNext = DateTime.UtcNow.AddMonths(1);
            model.NextDueCounts = dueDates.Count(w => w > DateTime.UtcNow && w <= dtNext);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Install()
        {

            var roleManager = new RoleManager<ProSchool.Web.Models.Role, int>(new RoleStore<ProSchool.Web.Models.Role, int, ProSchool.Web.Models.UserRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new ProSchool.Web.Models.Role();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (UserManager.FindByName("Admin") == null)
            {
                var user = new ProSchool.Web.Models.User { UserName = "Admin", Email = "Admin@ProSchool.com", EmailConfirmed = true };
                var result = UserManager.Create(user, "Admin!1234");
                UserManager.AddToRole(user.Id, "Admin");
            }
            return View();
        }
        public JsonNetResult GetDashboardData()
        {
            var allData = this._studentService.GetDashboardData();
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveInvoice(int StudentProgramId)
        {
            string InvoiceNo = "A001", chequeNo = null, BankName = null, IFSCCode = null;
            int PaymentStatus = 1, amount = 0;
            int? PaymentType = null;
            
            JsonResponse response = new JsonResponse();
            try
            {
                var Frequency = this._studentprograminvoiceService.GetFrequency(StudentProgramId, out amount);
                var result = 12 / Frequency;
                DateTime invoicedate = DateTime.UtcNow;
                DateTime endDate = invoicedate.AddMonths(Frequency);
                endDate = endDate.AddDays(-1);
                for (int i = 1; i <= 1; i++) //result
                {
                    int errorCode = this._studentprograminvoiceService.Insert(new Data.Models.StudentProgramInvoice { StudentProgramId = StudentProgramId, InvoiceNo = InvoiceNo, PaymentStatus = PaymentStatus, PaymentType = PaymentType, chequeNo = chequeNo, BankName = BankName, IFSCCode = IFSCCode, InvoiceDate = invoicedate, InvoiceAmount=amount,PeriodStartDate =invoicedate ,PeriodEndDate = endDate });
                    if (errorCode == 101)
                    {
                        response.Status = ResponseStatus.Warning;
                        // response.Message = "Invoice already generated.Please go to account page.";
                    }
                    else
                    {
                        response.Status = ResponseStatus.Success;
                        //response.Message = "Invoice generated successfully.";
                    }
                    invoicedate = invoicedate.AddMonths(Frequency);
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message + ex.StackTrace;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
    }
}