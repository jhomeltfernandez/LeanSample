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
    [DisplayName("Sales")]
    public class SaleController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var sales = Context.SaleRepo.Get().OrderByDescending(o=>o.Date).ToList();

            return PartialView("_GetList", sales);
        }

        public ActionResult CreateEdit(int id = 0)
        {
            Sale sale;

            if (id > 0)
            {
                sale = Context.SaleRepo.GetByID(id);
            }
            else
            {
                sale = new Sale();
            }

            return PartialView("_CreateEditForm", sale);
        }

        [HttpPost]
        public ActionResult CreateEdit(Sale model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.SaleRepo.GetByID(model.Id);
                        toUpdate.RateId = model.RateId;
                        toUpdate.Gross = model.Gross;
                        toUpdate.Less = model.Less;
                        toUpdate.FuelCost = model.FuelCost;
                        toUpdate.Date = model.Date;
                        toUpdate.CalculateThenSet();

                        Context.SaleRepo.Update(toUpdate);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = model;
                        toInsert.UserId = User.Identity.Name;
                        toInsert.CalculateThenSet();

                        Context.SaleRepo.Insert(toInsert);
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
                var sale = Context.SaleRepo.GetByID(id);

                if (sale != null)
                {
                    Context.SaleRepo.Delete(sale);
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

        public ActionResult CreateEditOtherExpense(int id = 0, int saleId = 0)
        {
            OtherExpence otherExpense;
            
            if (id > 0)
            {
                otherExpense = Context.OtherExpenceRepo.GetByID(id);
            }
            else
            {
                otherExpense = new OtherExpence() 
                {
                    SaleId = saleId
                };
            }

            return PartialView("_CreateEditOtherExpense", otherExpense);
        }

        [HttpPost]
        public ActionResult CreateEditOtherExpense(OtherExpence model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.OtherExpenceRepo.GetByID(model.Id);
                        toUpdate.Name = model.Name;
                        toUpdate.Amount = model.Amount;
                        toUpdate.SaleId = model.SaleId;

                        Context.OtherExpenceRepo.Update(toUpdate);
                        Context.Save();


                        UpdateSale(toUpdate.SaleId.Value);

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = model;

                        Context.OtherExpenceRepo.Insert(toInsert);
                        Context.Save();

                        UpdateSale(toInsert.SaleId.Value);

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

        public ActionResult DeleteOtherExpense(int id)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                var model = Context.OtherExpenceRepo.GetByID(id);

                int saleId = model.SaleId.Value;

                if (model != null)
                {
                    Context.OtherExpenceRepo.Delete(model);
                    Context.Save();

                    UpdateSale(saleId);

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



        private void UpdateSale(int saleid)
        {
            var sale = Context.SaleRepo.GetByID(saleid);
            sale.CalculateThenSet();

            Context.SaleRepo.Update(sale);
            Context.Save();
        }

    }
}