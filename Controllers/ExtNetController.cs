using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using GeoSystem.Db.DAL;
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

        [ChildActionOnly]
        public ActionResult NewBrigade()
        {
            Brigade brigade = new Brigade();
            return PartialView("Brigade", brigade);
        }
        [ChildActionOnly]
        public ActionResult NewRequest()
        {
            FormPanelRequest viewModel = new FormPanelRequest();
            viewModel.request = new Request();
            return PartialView("Request", viewModel);
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView("Menu");
        }
        [ChildActionOnly]
        public ActionResult ShowAllBrigades()
        {
            IEnumerable<Brigade> list =
            //    new List<Brigade>(new Brigade[] { 
            //    new Brigade() { BrigadeID = 1, BrigadeName = "b1"},
            //    new Brigade() { BrigadeID = 2, BrigadeName = "b2"}
            //});
            brigadeRepository.GetAll();
            return PartialView("BrigadesGrid", list);
        }
        [ChildActionOnly]
        public ActionResult ShowAllRequests()
        {

            IEnumerable<Request> list = new List<Request>(new Request[] { 
                new Request() { RequestID = 1, IsDone = true, RequestName = "r1",
                    Brigade = new Brigade() { BrigadeID = 1, BrigadeName = "b1"} },
                new Request() { RequestID = 2, IsDone = true, RequestName = "r2",
                    Brigade = new Brigade() { BrigadeID = 2, BrigadeName = "b2"}}
            });
            //requestRepository.GetAllWithBrigade();
            
            return PartialView("RequestsGrid", list);
        }

        public ActionResult GetBrigades()
        {
            //var res = brigadeRepository.GetAll()
            //    .Select(b => 
            //       new BrigadeComboBox { BrigadeName = b.BrigadeName, Brigade = b }
            //    ).ToList();

            IEnumerable<BrigadeComboBox> list =
                new List<BrigadeComboBox>(new BrigadeComboBox[] {
                new BrigadeComboBox { BrigadeName = "b1", Brigade = new Brigade() { BrigadeID = 1, BrigadeName = "b1"} },
                new BrigadeComboBox { BrigadeName = "b2", Brigade = new Brigade() { BrigadeID = 2, BrigadeName = "b2"} }
            });

            return this.Store(list);
        }
        [ChildActionOnly]
        public ActionResult getStatistics() {
            var model = requestRepository.GetAll()
                .Where(r => r.End > DateTime.Now.AddDays(-30))
                .GroupBy(r => r.Brigade)
                .Select(r => new object[]
                {
                    r.First().Brigade?.BrigadeName,
                    r.Count(),
                    r.Sum(d => (d.End - d.Start).TotalDays)
                }).ToArray();
            return PartialView("Statistics", model);
        }

        public ActionResult BrigadeHandleChanges(StoreDataHandler handler)
        {
            List<Brigade> items = handler.ObjectData<Brigade>();
            string errorMessage = null;

            if (handler.Action == StoreAction.Update)
            {
                items.ForEach(i =>
                {
                    brigadeRepository.Update(i);
                });
            }

            try
            {
                brigadeRepository.Save();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (errorMessage != null)
            {
                return this.Store(errorMessage);
            }

            return handler.Action == StoreAction.Update ? (ActionResult)this.Store(items) : (ActionResult)this.Content(""); ;
        }

        public ActionResult RequestHandleChanges(StoreDataHandler handler)
        {
            List<Request> requests = handler.ObjectData<Request>();
            string errorMessage = null;

            if (handler.Action == StoreAction.Update)
            {
                requests.ForEach(r => 
                {
                    requestRepository.Update(r);
                });
            }
            
            try
            {
                requestRepository.Save();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (errorMessage != null)
            {
                return this.Store(errorMessage);
            }

            return handler.Action == StoreAction.Update ? (ActionResult)this.Store(requests) : (ActionResult)this.Content(""); ;
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
                    Title = "Ошибка!",
                    Html = msg
                }).Show();

                return View("Index");
            }
            //X.Msg.Notify(new NotificationConfig
            //{
            //    Icon = Icon.Accept,
            //    Title = "Сохранение",
            //    Html = brigade.BrigadeName + " добавлена"
            //}).Show();

            return RedirectToAction("Index");
        }

        public ActionResult SubmitRequest(Request request)
        {
            Brigade brigade = brigadeRepository.Get(request.Brigade?.BrigadeID);
            request.Brigade = brigade;
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