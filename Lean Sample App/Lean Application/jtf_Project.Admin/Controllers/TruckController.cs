using jtf_Project.Data;
using jtf_Project.Data.Entities;
using jtf_Project.Data.HelperModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Truck")]
    public class TruckController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }
        public ActionResult GetList()
        {
            var trucks = Context.TruckRepo.Get(s => s.Deleted == false).OrderBy(o => o.Name).ToList();

            return PartialView("_GetList", trucks);
        }

        public ActionResult CreateEdit(int id = 0)
        {
            Truck truck;

            if (id > 0)
            {
                truck = Context.TruckRepo.GetByID(id);
            }
            else
            {
                truck = new Truck();
            }

            return PartialView("_CreateEditForm", truck);
        }

        [HttpPost]
        public ActionResult CreateEdit(Truck model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.TruckRepo.GetByID(model.Id);
                        toUpdate.Name = model.Name;
                        toUpdate.Capacity = model.Capacity;
                        toUpdate.DriverId = model.DriverId;
                        toUpdate.PlateNumber = model.PlateNumber;

                        Context.TruckRepo.Update(toUpdate);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = model;
                        toInsert.Deleted = false;

                        Context.TruckRepo.Insert(toInsert);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Save);
                        response.ReturnId = toInsert.Id.ToString();
                    }

                    return ReturnSuccessResponse(response);
                }
                else
                {
                    response.Message = GetValidationErrors();
                }
            }
            catch (Exception e)
            {

                response.Message = GetValidationErrors();
            }

            return ReturnErrorResponse(response);
        }

        public ActionResult Delete(int id)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                var truck = Context.TruckRepo.GetByID(id);

                if (truck != null)
                {
                    truck.Deleted = true;

                    Context.TruckRepo.Update(truck);
                    Context.Save();
                    response.Message = GetMessage(Data.Enums.ResponseMessage.Delete);
                    return ReturnSuccessResponse(response);
                }

            }
            catch (Exception e)
            {

                response.Message = GetValidationErrors();
            }

            return ReturnErrorResponse(response);
        }
    }
}