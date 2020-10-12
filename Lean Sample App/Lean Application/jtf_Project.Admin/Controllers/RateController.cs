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
    public class RateController : BaseController
    {
        [DisplayName("Rates")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var distinations = Context.RateRepo.Get(s => s.Deleted == false).OrderBy(o => o.Destination.Name).ToList();

            return PartialView("_GetList", distinations);
        }

        public ActionResult CreateEdit(int id = 0)
        {
            Rate rate;

            if (id > 0)
            {
                rate = Context.RateRepo.GetByID(id);
            }
            else
            {
                rate = new Rate();
            }

            return PartialView("_CreateEditForm", rate);
        }

        [HttpPost]
        public ActionResult CreateEdit(Rate model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.RateRepo.GetByID(model.Id);
                        toUpdate.DestinationId = model.DestinationId;
                        toUpdate.DriverCost = model.DriverCost;
                        toUpdate.TruckId = model.TruckId;
                        toUpdate.Amount = model.Amount;
                        toUpdate.WaterCost = model.WaterCost;
                        toUpdate.HelperCost = model.HelperCost;

                        Context.RateRepo.Update(toUpdate);
                        Context.Save();


                        //foreach (var sale in toUpdate.Sales)
                        //{
                        //    sale.Less = toUpdate.DriverCost + toUpdate.WaterCost + toUpdate.HelperCost;

                        //    Context.SaleRepo.Update(sale);
                        //    Context.Save();
                        //}

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = model;
                        toInsert.Deleted = false;

                        Context.RateRepo.Insert(toInsert);
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
                var rate = Context.RateRepo.GetByID(id);

                if (rate != null)
                {
                    rate.Deleted = true;

                    Context.RateRepo.Update(rate);
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