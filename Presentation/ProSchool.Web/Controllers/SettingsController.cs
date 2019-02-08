using Newtonsoft.Json;
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
    public class SettingsController : BaseController
    {
        private readonly InfrastructureService _infraService;
        private readonly InstitutionService _institutionService;
        private readonly SystemService _systemService;
        public SettingsController(InfrastructureService infraService, InstitutionService institutionService,SystemService systemService)
        {
            this._infraService = infraService;
            this._institutionService = institutionService;
            this._systemService = systemService;
        }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        #region System Actions

        public ActionResult System()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadSchoolLogo(HttpPostedFileBase[] fileToUpload) { 
         string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "SchoolLogo");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

                HttpPostedFileBase File = fileToUpload[0];
                FileInfo fi = new FileInfo(File.FileName);
                string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                var filePath = Path.Combine(path, sysFileName);
                File.SaveAs(filePath);

            ViewBag.UploadSchoolLogo = JsonConvert.SerializeObject(new { OriginalFileName = fi.Name, SystemFileName = sysFileName });
            return PartialView();
        
    }
        public JsonNetResult SaveSystem(Data.Models.Setting[] system)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._systemService.UpdateSystem(system);
                response.Status = ResponseStatus.Success;
                response.Message = "Data succesfully updated";
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;

            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetByInstituteId()
        {
            var InstName = this._systemService.GetValueForName("InstitutionName");
            var Address = this._systemService.GetValueForName("Address");
            var City = this._systemService.GetValueForName("City");
            var State = this._systemService.GetValueForName("State");
            var Pincode = this._systemService.GetValueForName("Pincode");
            var Phone = this._systemService.GetValueForName("Phone");
            var AboutSchool = this._systemService.GetValueForName("AboutSchool");

            return JsonNet(new { InstitutionName = InstName, Address = Address, City = City, State =  State, Pincode = Pincode, Phone = Phone, AboutSchool = AboutSchool }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult Institution()
        {
            return View();
        }

        #region Infrastructure

        public ActionResult Infrastructure()
        {
            return View();
        }

        public JsonNetResult GetAllDivisions()
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var allData = this._infraService.GetAll();
                response.Data = allData.Select(s => new { s.Id, s.Name, s.Description });
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

        public JsonNetResult InsertDivision(Data.Models.Division division)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var Id = this._infraService.Insert(division);
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
        public JsonNetResult UpdateDivision(int Id,string name, string desc)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._infraService.Update(Id,name,desc);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Name already exists!";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Division update successfully";
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

        public JsonNetResult DeleteDivision(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._infraService.Delete(Id);
                if(errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "You can't delete this division because this division is already used by student";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Division successfully deleted";
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

        public JsonNetResult IsDivisionNameExists(string name)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                response.Data = this._infraService.IsDivisionNameExists(name);
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

        #endregion

        #region Institution
        //Subject
        public JsonNetResult InsertSubject(Data.Models.Subject subject)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var Id = this._institutionService.Insert(subject);
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
        public JsonNetResult IsSubjectNameExists(string name)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                response.Data = this._institutionService.IsSubjectNameExists(name);
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
        public JsonNetResult GetAllSubjects()
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var allData = this._institutionService.GetAll();
                response.Data = allData.Select(s => new { s.Id, s.Name, s.Description });
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
        public JsonNetResult UpdateSubject(int Id, string name, string desc)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._institutionService.Update(Id, name, desc);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Name already exists!";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Subject successfully updated";
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
        public JsonNetResult DeleteSubject(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._institutionService.Delete(Id);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "You can't delete this subject because this subject is already used by student";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Subject successfully deleted";
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
        //Batches
        public JsonNetResult InsertBatche(Data.Models.Batch batche)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var Id = this._institutionService.Insert(batche);
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
        public JsonNetResult IsBatcheNameExists(string name)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                response.Data = this._institutionService.IsBatcheNameExists(name);
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
        public JsonNetResult GetAllBatche()
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                var allData = this._institutionService.GetAllBatche();
                response.Data = allData.Select(s => new { s.Id, s.Name, s.Description, SubjectName=s.Subject.Name,s.Capacity });
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
        public JsonNetResult UpdateBatche(int Id, string name, int capacity, string desc)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode= this._institutionService.UpdateBatche(Id, name,capacity, desc);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "Name already exists!";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Batch successfully updated";
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
        public JsonNetResult DeleteBatche(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int errorCode = this._institutionService.DeleteBatche(Id);
                if (errorCode == 101)
                {
                    response.Status = ResponseStatus.Warning;
                    response.Message = "You can't delete this batch because this batch is already used by student";
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "Batch successfully deleted";
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
        #endregion
        
    }
}