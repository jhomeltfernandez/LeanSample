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
    [DisplayName("Destination")]
    public class DestinationController : BaseController
    {
        // GET: Destination
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var distinations = Context.DestinationRepo.Get(S=>S.Deleted==false).OrderBy(o => o.Name).ToList();

            return PartialView("_GetList", distinations);
        }

        public ActionResult CreateEdit(int id = 0)
        {
            Destination destination;

            if (id > 0)
            {
                destination = Context.DestinationRepo.GetByID(id);
            }
            else
            {
                destination = new Destination();
            }

            return PartialView("_CreateEditForm", destination);
        }

        [HttpPost]
        public ActionResult CreateEdit(Destination model)
        {
            RequestResultModel response = new RequestResultModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        var toUpdate = Context.DestinationRepo.GetByID(model.Id);
                        toUpdate.Name = model.Name;

                        Context.DestinationRepo.Update(toUpdate);
                        Context.Save();

                        response.Message = GetMessage(Data.Enums.ResponseMessage.Update);
                        response.ReturnId = toUpdate.Id.ToString();
                    }
                    else
                    {
                        var toInsert = model;
                        toInsert.Deleted = false;

                        Context.DestinationRepo.Insert(toInsert);
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
                var destination = Context.DestinationRepo.GetByID(id);

                if (destination != null)
                {
                    destination.Deleted = true;

                    Context.DestinationRepo.Update(destination);
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