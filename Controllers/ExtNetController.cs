using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using GeoSystem.Db.DAL;
using GeoSystem.Db.DAO;
using GeoSystem.Db.ViewModel;
using GeoSystem.Models;

namespace GeoSystem.Controllers
{
    public class ExtNetController : Controller
    {
        IBrigadeRepository brigadeRepository;
        IRequestRepository requestRepository;

        public ExtNetController(IBrigadeRepository brigadeRepository, IRequestRepository requestRepository) 
        {
            this.brigadeRepository = brigadeRepository;
            this.requestRepository = requestRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewBrigade()
        {
            Brigade brigade = new Brigade();
            return PartialView("Brigade", brigade);
        }

        public ActionResult NewRequest()
        {
            FormPanelRequest viewModel = new FormPanelRequest();
            viewModel.request = new Request();
            return PartialView("Request", viewModel);
        }

        public ActionResult Menu()
        {
            return PartialView("Menu");
        }
        [ChildActionOnly]
        public ActionResult ShowAllBrigades()
        {
            IEnumerable<Brigade> list = brigadeRepository.GetAll();
            return PartialView("BrigadesGrid", list);
        }
        [ChildActionOnly]
        public ActionResult ShowAllRequests()
        {
            IEnumerable<Request> list = requestRepository.GetAll();
            return PartialView("RequestsGrid", list);
        }

        public ActionResult GetBrigades()
        {
            return this.Store(brigadeRepository.GetAll());
        }

        public ActionResult getStatistics() {
            var model = requestRepository.GetAll()
                .Where(r => r.End > DateTime.Now.AddDays(-30))
                .GroupBy(r => r.BrigadeID)
                .Select(r => new object[]
                {
                    r.First().Brigade.BrigadeName,
                    r.Count(),
                    r.Sum(d => (d.End - d.Start).TotalDays)
                }).ToArray();
            return PartialView("Statistics", model);
        }

        public ActionResult CreateBrigade()
        {
            Brigade viewModel = new Brigade();
            return this.View("Index", viewModel);
        }

        public ActionResult CreateRequest()
        {
            FormPanelRequest viewModel = new FormPanelRequest();
            viewModel.request = new Request();
            return this.View("Index", viewModel);
        }

        public ActionResult SubmitBrigade(Brigade brigade)
        {
            brigadeRepository.Create(brigade);
            try
            {
                brigadeRepository.Save();
            }
            catch (DbEntityValidationException ex)
            {
                string msg = "Errors";
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    msg = validationError.Entry.Entity.ToString() + "\n";
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        msg += err.ErrorMessage + "\n";
                    }
                }

                X.Msg.Notify(new NotificationConfig
                {
                    Icon = Icon.Accept,
                    Title = "Errors",
                    Html = msg
                }).Show();

                ViewBag.regime = 1;
                return View("Index");
            }
            X.Msg.Notify(new NotificationConfig
            {
                Icon = Icon.Accept,
                Title = "Сохранение",
                Html = brigade.BrigadeName + " добавлена"
            }).Show();

            return RedirectToAction("Index");
        }

        public ActionResult SubmitRequest(Request request)
        {
            request.BrigadeID = request.Brigade.BrigadeID;
            request.Brigade = null;
            requestRepository.Create(request);

            try
            {
                requestRepository.Save();
            }
            catch (DbEntityValidationException ex)
            {
                string msg = "Errors";
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    msg = validationError.Entry.Entity.ToString() +"\n";
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        msg += err.ErrorMessage + "\n";
                    }
                }

                X.Msg.Notify(new NotificationConfig
                {
                    Icon = Icon.Accept,
                    Title = "Errors",
                    Html = msg
                }).Show();

                ViewBag.regime = 2;
                return View("Index");
            }


            X.Msg.Notify(new NotificationConfig
            {
                Icon = Icon.Accept,
                Title = "Сохранение",
                Html = request.RequestName + " добавлена"
            }).Show();

            return RedirectToAction("Index");
        }
    }
}