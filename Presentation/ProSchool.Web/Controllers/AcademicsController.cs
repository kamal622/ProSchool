using Newtonsoft.Json;
using ProSchool.Services.Academics;
using ProSchool.Services.Finance;
using ProSchool.Services.Models;
using ProSchool.Services.Settings;
using ProSchool.Web.Framework;
using ProSchool.Web.Framework.Controllers;
using ProSchool.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProSchool.Web.Controllers
{
    [Authorize]
    public class AcademicsController : BaseController
    {
        private readonly InfrastructureService _infraService;
        private readonly StudentService _studentService;
        private readonly InquiryService _inquiryService;
        private readonly InstitutionService _institutionService;
        private readonly FeeService _feeService;
        private readonly StudentProgramService _studentprogramService;
        public AcademicsController(InfrastructureService infraService, StudentService studentService, InquiryService inquiryService, InstitutionService institutionService,
            FeeService feeService, StudentProgramService studentprogramService)
        {
            this._infraService = infraService;
            this._studentService = studentService;
            this._inquiryService = inquiryService;
            this._institutionService = institutionService;
            this._feeService = feeService;
            this._studentprogramService = studentprogramService;
        }
        // GET: Academics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inquiry()
        {
            return View();
        }

        public ActionResult Registrations()
        {
            return View();
        }
        public JsonNetResult GetAllDivisions(bool sendDefaultItem = false)
        {
            var allData = this._infraService.GetAll();
            if (sendDefaultItem)
                allData.Insert(0, new Data.Models.Division { Id = 0, Name = "ALL" });
            var division = allData.Select(s => new { s.Id, s.Name });
            return JsonNet(division, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllSubjects(bool sendDefaultItem = false)
        {
            var allData = this._institutionService.GetAll();
            if (sendDefaultItem)
                allData.Insert(0, new Data.Models.Subject { Id = 0, Name = "ALL" });
            var subject = allData.Select(s => new { s.Id, s.Name });
            return JsonNet(subject, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllSub(bool sendDefaultItem = false)
        {
            var allData = this._institutionService.GetSub();
            if (sendDefaultItem)
                allData.Insert(0, new Data.Models.Subject { Id = 0, Name = "All" });
            var studsubject = allData.Select(s => new { s.Id, s.Name });
            return JsonNet(studsubject, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllFName(bool sendDefaultItem=false)
        {
            var allData = this._studentService.GetStudFname();
            if (sendDefaultItem)
                allData.Insert(0, new Data.Models.Student { Id = 0, FirstName = "All" });
            var studfname = allData.Select(s => new { s.Id, s.FirstName });
            return JsonNet(studfname, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllBatches(bool sendDefaultItem = false)
        {
            var allData = this._institutionService.GetAllBatche();
            if (sendDefaultItem)
                allData.Insert(0, new Data.Models.Batch { Id = 0, Name = "ALL" });
            var subject = allData.Select(s => new { s.Id, s.Name });
            return JsonNet(subject, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllFees()
        {
            var allData = this._feeService.GetAllFee();
            var subject = allData.Select(s => new { s.Id, s.Frequency, s.Name });
            return JsonNet(subject, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadStudentPhoto()
        {
            ViewBag.SysFileName = "";
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadStudentPhoto(HttpPostedFileBase[] fileToUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "StudentPhotos");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            HttpPostedFileBase File = fileToUpload[0];
            FileInfo fi = new FileInfo(File.FileName);
            string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
            var filePath = Path.Combine(path, sysFileName);
            File.SaveAs(filePath);

            ViewBag.UploadStudentPhoto = JsonConvert.SerializeObject(new { OriginalFileName = fi.Name, SystemFileName = sysFileName });
            return PartialView();
        }

        public ActionResult Students()
        {
            return View();
        }
        public JsonNetResult SaveInquiry(Data.Models.Student student, Data.Models.Inquiry inquiry, Data.Models.StudentProgram StudentProgram,bool IsRegisterCheck)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                if (student.Id == 0)
                {
                    inquiry.InquiryDate = DateTime.UtcNow;
                    student.IsActive = true;
                    student.RelativePath = null;
                    student.Remarks = null;
                    student.Inquiry = inquiry;
                    //this._inquiryService.Insert(inquiry);
                    int id = this._studentService.Insert(student);
                    response.Data = new { StudentId = id };
                    response.Status = ResponseStatus.Success;
                }
                else
                {
                    if (IsRegisterCheck == true)
                    {
                        StudentProgram.RegistrationDate = DateTime.UtcNow;
                        student.IsRegistered = true;
                        StudentProgram.NextDueDate = DateTime.UtcNow;
                        StudentProgram.IsActive = true;
                        this._studentService.Update(student);
                        int errorCode = 0;
                        int studentProgramId = this._studentprogramService.Insert(StudentProgram, out errorCode);
                        if (errorCode == 101)
                        {
                            response.Status = ResponseStatus.Warning;
                            response.Message = "Student already registered with same division and subject.";
                        }
                        else
                        {
                            response.Data = new { StudentId = student.Id, StudentProgramId = studentProgramId };
                            response.Status = ResponseStatus.Success;
                            response.Message = "Registration successfully.";
                        }
                    }
                    else
                    {
                        this._studentService.UpdateInquiry(student, inquiry);
                        response.Data = new { StudentId = student.Id };
                        response.Status = ResponseStatus.Success;
                        response.Message = "Information successfully updated.";
                    }
                   
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
        public JsonNetResult UpdateProgram(Data.Models.StudentProgram studentprogram)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                int errorCode = 0;
                int studentProgramId = this._studentprogramService.UpdateStudProgram(studentprogram, out errorCode);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Student already registered with same division and subject.";
                }
                else
                {
                    response.Data = new { StudentProgramId = studentProgramId };
                    response.Status = ResponseStatus.Success;
                    response.Message = "Registration successfully.";
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
        public JsonNetResult GetAllInquiry(int registerStatusId)
        {
            var allData = this._studentService.GetAllStudent(registerStatusId);
            var inquiry = allData.Select(s => new
            {
                s.Id,
                Name = s.FirstName + " " + s.LastName,
                PhoneNumber = s.Mobile,
                Department = s.Inquiry.Division.Name,
                Class = s.Inquiry.Subject.Name,
                InquiryDate = s.Inquiry.InquiryDate
            });
            return JsonNet(inquiry, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult DeleteInquiry(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();

                int errorCode = this._studentService.DeleteInquiry(Id);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Student already registered. Can not delete this inquiry.";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Inquiry deleted successfully.";
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
        public JsonNetResult DeleteRegStudentByID(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();

                int errorCode = this._studentService.DeleteRegStudentByID(Id);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Student already registered. Can not delete this program.";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "program deleted successfully.";
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
        public JsonNetResult GetStudentByEditId(int Id)
        {
            Data.Models.Student objStudent = this._studentService.GetForEditId(Id);

            StudentViewModel Student = new StudentViewModel
            {
                Id = objStudent.Id,
                InquiryId = objStudent.InquiryId,
                FirstName = objStudent.FirstName,
                MiddleName = objStudent.MiddleName,
                LastName = objStudent.LastName,
                Gender = Convert.ToChar(objStudent.Gender),
                Mobile = objStudent.Mobile,
                Address = objStudent.Address,
                Email = objStudent.Email,
                DOB = objStudent.DOB,
                Remarks = objStudent.Remarks,
                RegistrationNo = objStudent.RegistrationNo                
            };

            Data.Models.Inquiry objInquiry = this._inquiryService.GetForId(Id);

            InquiryViewModel Inquiry = new InquiryViewModel
            {
                Id = objInquiry.Id,
                Remark = objInquiry.Remark,
                DivisionId = objInquiry.DivisionId,
                SubjectId = objInquiry.SubjectId
            };
            return JsonNet(new { Student = Student, Inquiry = Inquiry }, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetStudentById(int Id)
        {
            Data.Models.Student objStudent = this._studentService.GetForId(Id);

            StudentViewModel Student = new StudentViewModel
            {
                Id = objStudent.Id,
                InquiryId = objStudent.InquiryId,
                FirstName = objStudent.FirstName,
                MiddleName = objStudent.MiddleName,
                LastName = objStudent.LastName,
                Gender = Convert.ToChar(objStudent.Gender),
                Mobile = objStudent.Mobile,
                Address = objStudent.Address,
                Email = objStudent.Email,
                DOB = objStudent.DOB,
                Remarks = objStudent.Remarks,
                School = objStudent.School,
                College = objStudent.College,
                SCAddress = objStudent.SCAddress,
                ContactName = objStudent.ContactName,
                ContactNumber = objStudent.ContactNumber,
                ContactRelation = objStudent.ContactRelation,
                RegistrationNo = objStudent.RegistrationNo
            };

            Data.Models.Inquiry objInquiry = this._inquiryService.GetForId(Id);

            InquiryViewModel Inquiry = new InquiryViewModel
            {
                Id = objInquiry.Id,
                Remark = objInquiry.Remark,
                DivisionId = objInquiry.DivisionId,
                SubjectId = objInquiry.SubjectId
            };
            return JsonNet(new { Student = Student, Inquiry = Inquiry }, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetInquiryById(int Id)
        {
            InquiryViewModel Inquiry = new InquiryViewModel();
            Data.Models.Inquiry objInquiry = this._inquiryService.GetForId(Id);

            if (Inquiry != null)
            {
                Inquiry.Id = objInquiry.Id;
                Inquiry.Remark = objInquiry.Remark;
                Inquiry.DivisionId = objInquiry.DivisionId;
                Inquiry.SubjectId = objInquiry.SubjectId;
            }
            return JsonNet(Inquiry, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetProgramById(int Id)
        {
            Data.Models.StudentProgram objProgram = this._studentprogramService.GetProgramId(Id);
            StudentProgramViewModel program = new StudentProgramViewModel();
            if (program != null)
            {
                program.Id = objProgram.Id;
                program.StudentId = objProgram.StudentId;
                program.DivisionId = objProgram.DivisionId;
                program.SubjectId = objProgram.SubjectId;
                program.BatchId = objProgram.BatchId;
                program.FeeId = objProgram.FeeId;
            }
            return JsonNet(program, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllRegistrationData(int DivisionId, int BatchId, int SubjectId)
        {
            var allData = this._studentService.GetAllRegistrations(DivisionId, BatchId, SubjectId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAllStudentData(int DivisionId, int BatchId, int SubjectId)
        {
            var allData = this._studentService.GetStudentData(DivisionId, BatchId, SubjectId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateIsActive(int Id, bool Checked)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._studentprogramService.Update(Id, Checked);
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
        public JsonNetResult UpdateStudentIsActive(int Id, bool Checked)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._studentService.UpdateIsActive(Id, Checked);
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
        public JsonNetResult SaveStudent(Data.Models.Student student)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                student.IsRegistered = true;
                this._studentService.Update(student);
                response.Status = ResponseStatus.Success;
                response.Message = "Data saved successfully.";

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
                LogException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateDueDate(int Id, DateTime extendDate)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._studentprogramService.UpdateNextDueDate(Id, extendDate);
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