using ProSchool.Services.Academics;
using ProSchool.Services.Finance;
using ProSchool.Services.Models;
using ProSchool.Web.Framework;
using ProSchool.Web.Framework.Controllers;
using ProSchool.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace ProSchool.Web.Controllers
{
    [Authorize]
    public class FinanceController : BaseController
    {
        private readonly FeeService _feeService;
        private readonly StudentProgramInvoiceService _studentprogramInvoiceService;
        private readonly StudentProgramService _studentprogramService;

        public FinanceController(FeeService feeService, StudentProgramInvoiceService studentprogramInvoiceService, StudentProgramService studentprogramService)
        {
            this._feeService = feeService;
            this._studentprogramInvoiceService = studentprogramInvoiceService;
            this._studentprogramService = studentprogramService;
        }
        // GET: Finance
        public ActionResult Fee()
        {
            return View();
        }
        public ActionResult Accounts()
        {
            InvoiceReportModel model = new InvoiceReportModel();
            model.Id = 0;
            model.ReportDataSource = new List<InvoiceDataset>();

            return View(model);
        }
        [HttpPost]
        public ActionResult InvoiceDetails(int Id)
        {
            var ReportDataSource = this._studentprogramInvoiceService.getInvoiceDetails(Id);
            var schoolInfo = this._studentprogramInvoiceService.InstitutionDetails();
            var schoolName = schoolInfo.Where(w => w.Name == "InstitutionName").Select(s => s.Value).FirstOrDefault();
            var schooladdress = schoolInfo.Where(w => w.Name == "Address").Select(s => s.Value).FirstOrDefault();
            var schoolcity = schoolInfo.Where(w => w.Name == "City").Select(s => s.Value).FirstOrDefault();
            var schoolstate = schoolInfo.Where(w => w.Name == "State").Select(s => s.Value).FirstOrDefault();
            var schoolPincode = schoolInfo.Where(w => w.Name == "Pincode").Select(s => s.Value).FirstOrDefault();
            var schoolPhone = schoolInfo.Where(w => w.Name == "Phone").Select(s => s.Value).FirstOrDefault();
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportParameters = new { InstitutionName = schoolName , InstitutionAddress = schooladdress , InstitutionCity = schoolcity , InstitutionState = schoolstate , InstitutionPincode = schoolPincode , InstitutionPhone = schoolPhone },
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Invoice.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }
        public JsonNetResult GetAllFrequency()
        {
            var allData = this._feeService.GetAllFrequency();
            var frequency = allData.Select(s => new { s.Id, s.Description });
            return JsonNet(frequency, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveFee(Data.Models.Fee fee)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                if (fee.Id == 0)
                {
                    var Id = this._feeService.Insert(fee);
                }
                else
                    this._feeService.Update(fee);
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllFee()
        {
            var allData = this._feeService.GetAllFee();
            var fee = allData.Select(s => new { s.Id, s.Name, FrequencyName = s.FeeFrequency.Description, s.Amount });
            return JsonNet(fee, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetFeeById(int Id)
        {
            FeeViewModel Fee = new FeeViewModel();
            Data.Models.Fee objFee = this._feeService.GetForId(Id);
            if (Fee != null)
            {
                Fee.Id = objFee.Id;
                Fee.Name = objFee.Name;
                Fee.Frequency = objFee.Frequency.ToString();
                Fee.Amount = objFee.Amount;
            }
            return JsonNet(Fee, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult DeleteFee(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._feeService.Delete(Id);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "You can't delete this Fee because this fee is already used by student";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Fee successfully deleted";
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult DeleteInvoice(int invoiceId)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._studentprogramInvoiceService.DeleteStudentProgramInvoice(invoiceId);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Student already paid fee. Can not delete this fee.";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllInvoiceData(int StatusId, int SubjectId, string StudentName)
        {
            var allData = this._studentprogramInvoiceService.GetInvoiceData(StatusId, SubjectId, StudentName);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllStudInvoice(int SubjectId, string StudentName)
        {
            var allData = this._studentprogramInvoiceService.GetStudInvoice(SubjectId, StudentName);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateFee(int Id, int PaymentType, string BankName, string IFSCCode, bool IsChequeClear)  //int FeeId,
        {
            JsonResponse response = new JsonResponse();
            try
            {
                if (PaymentType == 1)
                {
                    BankName = null;
                    IFSCCode = null;
                    IsChequeClear = false;
                }
                this._studentprogramInvoiceService.Update(Id, PaymentType, BankName, IFSCCode, IsChequeClear);//FeeId,
                response.Status = ResponseStatus.Success;
                response.Message = "Payment done";
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message + ex.StackTrace;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetInvoiceById(int Id)
        {
            Data.Models.StudentProgramInvoice objInvoice = this._studentprogramInvoiceService.GetForId(Id);
            StudentProgramInvoiceViewModel Account = new StudentProgramInvoiceViewModel
            {
                Id = objInvoice.Id,
                FeeId = objInvoice.StudentProgram.FeeId,
                PaymentType = Convert.ToInt32(objInvoice.PaymentType),
                BankName = objInvoice.BankName,
                IFSCCode = objInvoice.IFSCCode,
                IsChequeClear = objInvoice.IsChequeClear
            };
            return JsonNet(Account, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult ViewFeeInfo(int Id)
        {
            var allData = this._studentprogramInvoiceService.GetFeeInfo(Id);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult ViewInvoiceDetails(int Id)
        {
            var allData = this._studentprogramInvoiceService.GetInvoiceDetails(Id);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdatePaymentStatus(int Id, bool PaymentStatus)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._studentprogramInvoiceService.UpdatePayStatus(Id, PaymentStatus);
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GenerateNewInvoice(int StudentProgramId, int FeeId)
        {
            string InvoiceNo = "A001", chequeNo = null, BankName = null, IFSCCode = null;
            int PaymentStatus = 1, amount = 0;
            int? PaymentType = null;
            JsonResponse response = new JsonResponse();
            try
            {
                var data = this._feeService.GetFrequencyId(FeeId);
                var Frequency = data.FeeFrequency.Frequency;
                var lastInvoiceDate = this._studentprogramInvoiceService.getLastDate(StudentProgramId);
                var result = 12 / Frequency;
                amount = data.Amount;
                DateTime invoicedate;
                DateTime endDate=DateTime.Now.Date;
                if (lastInvoiceDate == DateTime.Now.Date)
                {
                    invoicedate = lastInvoiceDate.AddMonths(Frequency);
                    endDate = invoicedate.AddDays(-1);
                }
                else
                {
                    invoicedate = lastInvoiceDate.AddMonths(Frequency);
                    endDate = invoicedate.AddDays(-1);
                }
                for (int i = 1; i <= 1; i++)
                {
                    int errorCode = this._studentprogramInvoiceService.Insert(new Data.Models.StudentProgramInvoice
                    {
                        StudentProgramId = StudentProgramId,
                        InvoiceNo = InvoiceNo,
                        PaymentStatus = PaymentStatus,
                        PaymentType = PaymentType,
                        chequeNo = chequeNo,
                        BankName = BankName,
                        IFSCCode = IFSCCode,
                        InvoiceDate = lastInvoiceDate,
                        InvoiceAmount = amount,
                        PeriodStartDate = lastInvoiceDate,
                        PeriodEndDate = endDate
                    });
                    this._studentprogramInvoiceService.UpdateNextDueDate(StudentProgramId, lastInvoiceDate, FeeId);
                    if (errorCode == 101)
                    {
                        response.Status = ResponseStatus.Warning;
                    }
                    else
                    {
                        response.Status = ResponseStatus.Success;
                    }
                    //invoicedate = invoicedate.AddMonths(Frequency);
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
        public JsonNetResult ShowInvoiceBtn(int StudentProgramId)
        {
            var allData = this._studentprogramInvoiceService.getAllData(StudentProgramId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult IsFeeNameExists(string name)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                response.Data = this._feeService.IsFeeNameExists(name);
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

    }
}