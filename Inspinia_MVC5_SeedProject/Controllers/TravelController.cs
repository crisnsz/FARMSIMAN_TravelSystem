using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;

namespace ERP_GMEDINA.Controllers
{
    public class TravelController : Controller
    {
        private readonly FARSIMANEntities db = new FARSIMANEntities();

        public static List<tbTravelDetail> ListTravelDetails { get; set; } = new List<tbTravelDetail>();
        private static bool Modified { get; set; }

        public static List<int> ListEmployees { get; set; } = new List<int>();

        private static tbEmployee EmployeeInfo { get; set; } = new tbEmployee();

        private readonly Helpers Login = new Helpers();

        #region ActionResult

        public ActionResult GenerateReport(tbTravel tbTravel, string InitialDate, string FinalDate)
        {
            var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
            var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);

            var query = from travel in db.tbTravels
                        where travel.transporter_ID == tbTravel.transporter_ID &&
                              travel.departure_Date_and_Time >= vInitialDate &&
                              travel.departure_Date_and_Time <= vFinalDate
                        let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                        from travelDetail in db.tbTravelDetails
                        where travel.travel_ID == travelDetail.travel_ID
                        from employee in db.tbEmployees
                        where travelDetail.employee_ID == employee.employee_ID
                        from subsidiary in db.tbSubsidiaries
                        where travel.subsidiary_ID == subsidiary.subsidiary_ID
                        from transporter in db.tbTransporters
                        where travel.transporter_ID == transporter.transporter_ID
                        select new TravelReportDataModel
                        {
                            Travel_ID = travel.travel_ID,
                            Departure_Date_and_Time = travel.departure_Date_and_Time,
                            Total_distance_Kilometers = travel.distance_Kilometers,
                            Total_travel_Cost_per_Date = totalTravelCost,
                            TotalTravelCost = travel.total_travel_Cost,
                            Subsidiary_Name = subsidiary.subsidiary_Name,
                            Subsidiary_Direction = subsidiary.subsidiary_Direction,
                            Employee_Name = employee.employee_Name,
                            Employee_Direction = employee.employee_Direction,
                            Distance_Kilometers = travelDetail.distance_Kilometers,
                            Transporter_Name = transporter.transporter_Name,
                            Transporter_Fee = transporter.transporter_Fee
                        };

            List<TravelReportDataModel> reportData = query.ToList();

            return View(reportData);
        }

        public List<TravelReportDataModel> GetReportData(int transporter_ID, string InitialDate, string FinalDate)
        {
            var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
            var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);

            var query = from travel in db.tbTravels
                        where travel.transporter_ID == transporter_ID &&
                              travel.departure_Date_and_Time >= vInitialDate &&
                              travel.departure_Date_and_Time <= vFinalDate
                        let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                        from travelDetail in db.tbTravelDetails
                        where travel.travel_ID == travelDetail.travel_ID
                        from employee in db.tbEmployees
                        where travelDetail.employee_ID == employee.employee_ID
                        from subsidiary in db.tbSubsidiaries
                        where travel.subsidiary_ID == subsidiary.subsidiary_ID
                        from transporter in db.tbTransporters
                        where travel.transporter_ID == transporter.transporter_ID
                        select new TravelReportDataModel
                        {
                            Travel_ID = travel.travel_ID,
                            Departure_Date_and_Time = travel.departure_Date_and_Time,
                            Total_distance_Kilometers = travel.distance_Kilometers,
                            Total_travel_Cost_per_Date = totalTravelCost,
                            TotalTravelCost = travel.total_travel_Cost,
                            Subsidiary_Name = subsidiary.subsidiary_Name,
                            Subsidiary_Direction = subsidiary.subsidiary_Direction,
                            Employee_Name = employee.employee_Name,
                            Employee_Direction = employee.employee_Direction,
                            Distance_Kilometers = travelDetail.distance_Kilometers,
                            Transporter_Name = transporter.transporter_Name,
                            Transporter_Fee = transporter.transporter_Fee
                        };

            List<TravelReportDataModel> reportData = query.ToList();

            return reportData;
        }

        private void CleanVariables()
        {
            Modified = false;

            Session["employees"] = null;
            Session["travelDetailsDeleted"] = null;
            Session["travelDetails"] = null;

            ListEmployees.Clear();
        }

        private void GetUserInformation()
        {
            int employee_ID = 0;

            List<tbUser> UserInformation = Login.getUserInformation();

            foreach (tbUser user in UserInformation)
            {
                employee_ID = user.employee_ID;
            }

            EmployeeInfo = db.tbEmployees.Find(employee_ID);
        }

        // GET: /Travel/

        [SessionManager("Travel/Index")]
        public ActionResult Index()
        {
            CleanVariables();
            GetUserInformation();

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");

            var tbtravels = db.tbTravels;
            return View(tbtravels.ToList());
        }

        // GET: /Travel/Details/5

        [SessionManager("Travel/Details")]
        public ActionResult Details(int? id)
        {
            CleanVariables();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            return View(tbTravel);
        }

        // GET: /Travel/Create

        [SessionManager("Travel/Create")]
        public ActionResult Create()
        {
            CleanVariables();

            GetUserInformation();

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");

            tbTravel tbTravel = new tbTravel
            {
                distance_Kilometers = 0,
                total_travel_Cost = 0,
            };

            return View(tbTravel);
        }

        // POST: /Travel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Travel/Create")]
        public ActionResult Create([Bind(Include = "travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {
            try
            {
                using (TransactionScope Tran = new TransactionScope())
                {
                    GetUserInformation();

                    var travelDetails = (List<tbTravelDetail>)Session["travelDetails"];

                    if (travelDetails is null)
                    {
                        HandleErrorsOnCreate("No se pudo agregar el registro. Error: Detalle del Viaje Vacio");
                        return View(tbTravel);
                    }

                    var travelHeader = db.UDP_Gral_tbTravels_Insert(
                        tbTravel.subsidiary_ID, tbTravel.transporter_ID, tbTravel.employee_ID,
                        tbTravel.departure_Date_and_Time, tbTravel.distance_Kilometers, tbTravel.total_travel_Cost)
                        .FirstOrDefault();

                    if (travelHeader is null || travelHeader.ErrorMessage.StartsWith("-1"))
                    {
                        HandleErrorsOnCreate("No se pudo agregar el registro");
                        return View(tbTravel);
                    }

                    var errorOrInsertedID = Convert.ToInt32(travelHeader.ErrorMessage);

                    if (errorOrInsertedID <= 0)
                    {
                        HandleErrorsOnCreate("No se pudo agregar el registro");
                        return View(tbTravel);
                    }

                    if (travelDetails.Any())
                    {
                        var Transportists = db.tbTransporters.Find(tbTravel.transporter_ID);

                        if (Transportists == null)
                        {
                            HandleErrorsOnCreate("No se pudo agregar el registro");
                            return View(tbTravel);
                        }

                        foreach (int travelDetailEmployee_ID in travelDetails.Select(travelDetail => travelDetail.employee_ID))
                        {
                            var EmployeesSubsidiariesInfo = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == travelDetailEmployee_ID && x.subsidiary_ID == tbTravel.subsidiary_ID).SingleOrDefault();

                            if (EmployeesSubsidiariesInfo is null)
                            {
                                HandleErrorsOnCreate("No se pudo agregar el registro");
                                return View(tbTravel);
                            }

                            var travel_Cost = Transportists.transporter_Fee * (decimal)EmployeesSubsidiariesInfo.employeeSubsidiary_DistanceKM;

                            var listTravelDetalle = db.UDP_Gral_tbTravelDetail_Insert(
                                errorOrInsertedID, travelDetailEmployee_ID, EmployeesSubsidiariesInfo.employeeSubsidiary_DistanceKM, travel_Cost)
                                .FirstOrDefault();

                            if (listTravelDetalle is null || listTravelDetalle.ErrorMessage.StartsWith("-1"))
                            {
                                HandleErrorsOnCreate("No se pudo agregar el registro detalle");
                                return View(tbTravel);
                            }
                        }
                    }

                    if (!ModelState.IsValid || travelDetails.Count <= 0)
                    {
                        HandleErrorsOnCreate("No se pudo agregar el registro detalle");
                        return View(tbTravel);
                    }

                    Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                HandleErrorsOnCreate($"No se pudo agregar el registro. Error: {ex.Message}");
                throw;
            }

            return RedirectToAction("Index");
        }

        private void HandleErrorsOnCreate(string ErrorMessage)
        {
            CleanVariables();
            ModelState.AddModelError("", ErrorMessage);
            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name");
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name");
        }

        // GET: /Travel/Edit/5

        [SessionManager("Travel/Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            CleanVariables();

            GetUserInformation();

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);

            ViewBag.transporter_Fee = tbTravel.tbTransporter.transporter_Fee;

            ViewBag.subsidiaryAddress = db.tbSubsidiaries.Find(tbTravel.subsidiary_ID).subsidiary_Direction;

            var travelDetails = from detail in db.tbTravelDetails
                                where detail.travel_ID == id
                                select detail;

            var employeesAvaliable = from employeeSubsidiary in db.tbEmployeesSubsidiaries
                        .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                        .Include(es => es.tbSubsidiary)   // Include related entity tbEmployee
                                                          // Add more .Include statements for other related entities as needed
                                     join employee in db.tbEmployees on employeeSubsidiary.employee_ID equals employee.employee_ID
                                     join travelDetail in travelDetails on employee.employee_ID equals travelDetail.employee_ID into travelDetailGroup
                                     from travelDetail in travelDetailGroup.DefaultIfEmpty()
                                     join travel in db.tbTravels on employeeSubsidiary.subsidiary_ID equals travel.subsidiary_ID
                                     where travelDetail == null
                                     select employeeSubsidiary;

            ViewBag.EmployeesAvaliable = employeesAvaliable.ToList();

            Session["employeesAvaliable"] = employeesAvaliable.ToList();

            var employeesAdded = (from travelDetail in db.tbTravelDetails
                                  .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                                  .Include(es => es.tbTravel) // Include related entity tbSubsidiary
                                  join employee in db.tbEmployees
                                    on travelDetail.employee_ID equals employee.employee_ID
                                  select travelDetail).ToList();

            ViewBag.EmployeesAdded = employeesAdded;

            Session["travelDetails"] = employeesAdded.ToList();

            return View(tbTravel);
        }

        // POST: /Travel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [SessionManager("Travel/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "travel_ID,subsidiary_ID,transporter_ID,employee_ID,departure_Date_and_Time,distance_Kilometers,total_travel_Cost")] tbTravel tbTravel)
        {
            GetUserInformation();

            var travelDetails = (List<tbTravelDetail>)Session["travelDetails"];

            if (travelDetails == null || !ModelState.IsValid || travelDetails.Count == 0)
            {
                ModelState.AddModelError("", "No se pudo editar el Viaje. Error: Detalle del Viaje Vacío");
                HandleErrorsOnEdit("No se pudo editar el Viaje", tbTravel);
                return View(tbTravel);
            }

            using (TransactionScope Tran = new TransactionScope())
            {
                var listTravel = db.UDP_Gral_tbTravels_Update(tbTravel.travel_ID, tbTravel.subsidiary_ID, tbTravel.transporter_ID, tbTravel.employee_ID, tbTravel.departure_Date_and_Time, tbTravel.distance_Kilometers, tbTravel.total_travel_Cost)
                    .FirstOrDefault();

                if (listTravel == null || listTravel.ErrorMessage.StartsWith("-1"))
                {
                    ModelState.AddModelError("", "No se pudo agregar el registro");
                    HandleErrorsOnEdit("No se pudo editar detalle del Viaje", tbTravel);
                    return View(tbTravel);
                }

                var errorOrUpdatedID = Convert.ToInt32(listTravel.ErrorMessage);

                if (errorOrUpdatedID <= 0)
                {
                    ModelState.AddModelError("", "No se pudo editar el registro");
                    HandleErrorsOnEdit("No se pudo editar detalle del Viaje", tbTravel);
                    return View(tbTravel);
                }

                InsertNewTravelDetails(GetEmployeesAddedTravelFromSession(tbTravel));
                DeleteMarkedTravelDetails();

                Tran.Complete();
            }

            return RedirectToAction("Index");
        }


        private void HandleErrorsOnEdit(string ErrorMessage, tbTravel tbTravel)
        {
            CleanVariables();
            ModelState.AddModelError("", ErrorMessage);
            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.employee_ID = EmployeeInfo.employee_ID;
            ViewBag.employee_Name = EmployeeInfo.employee_Name;

            ViewBag.employee_ID = new SelectList(db.tbEmployees, "employee_ID", "employee_Name", tbTravel.employee_ID);
            ViewBag.subsidiary_ID = new SelectList(db.tbSubsidiaries, "subsidiary_ID", "subsidiary_Name", tbTravel.subsidiary_ID);
            ViewBag.transporter_ID = new SelectList(db.tbTransporters, "transporter_ID", "transporter_Name", tbTravel.transporter_ID);

            ViewBag.transporter_Fee = tbTravel.tbTransporter.transporter_Fee;

            ViewBag.subsidiaryAddress = db.tbSubsidiaries.Find(tbTravel.subsidiary_ID).subsidiary_Direction;

            var travelDetailsFromDB = from detail in db.tbTravelDetails
                                      where detail.travel_ID == tbTravel.travel_ID
                                      select detail;

            ListTravelDetails = travelDetailsFromDB.ToList();

            ListEmployees = travelDetailsFromDB.Select(detail => detail.employee_ID).ToList();

            var employeesAvaliable = from employeeSubsidiary in db.tbEmployeesSubsidiaries
                        .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                        .Include(es => es.tbSubsidiary)   // Include related entity tbEmployee
                                                          // Add more .Include statements for other related entities as needed
                                     join employee in db.tbEmployees on employeeSubsidiary.employee_ID equals employee.employee_ID
                                     join travelDetail in travelDetailsFromDB on employee.employee_ID equals travelDetail.employee_ID into travelDetailGroup
                                     from travelDetail in travelDetailGroup.DefaultIfEmpty()
                                     join travel in db.tbTravels on employeeSubsidiary.subsidiary_ID equals travel.subsidiary_ID
                                     where travelDetail == null
                                     select employeeSubsidiary;

            ViewBag.EmployeesAvaliable = employeesAvaliable.ToList();


            var employeesAdded = (from travelDetail in db.tbTravelDetails
                                  .Include(es => es.tbEmployee) // Include related entity tbSubsidiary
                                  .Include(es => es.tbTravel) // Include related entity tbSubsidiary
                                  join employee in db.tbEmployees
                                    on travelDetail.employee_ID equals employee.employee_ID
                                  select travelDetail).ToList();

            ViewBag.EmployeesAdded = employeesAdded;
        }

        #region Actions

        private void InsertNewTravelDetails(List<tbTravelDetail> local)
        {
            var listTravelDetailsDeletedSession = (List<tbTravelDetail>)Session["travelDetailsDeleted"];

            foreach (var localTravelDetail in local.Where(detail => detail.travel_Detail_ID == 0))
            {
                if (listTravelDetailsDeletedSession != null)
                {
                    var Exist = listTravelDetailsDeletedSession.Find(x => x.employee_ID == localTravelDetail.employee_ID);

                    if (Exist != null)
                    {
                        continue;
                    }
                }


                var ErrorORTravelDetailInserted = db.UDP_Gral_tbTravelDetail_Insert(
                    localTravelDetail.travel_ID,
                    localTravelDetail.employee_ID,
                    localTravelDetail.distance_Kilometers,
                    localTravelDetail.travel_Cost
                ).FirstOrDefault();

                if (ErrorORTravelDetailInserted is null || ErrorORTravelDetailInserted.ErrorMessage.StartsWith("-1"))
                {
                    ModelState.AddModelError("", "No se pudo agregar el registro");

                    return;
                }
            }
        }

        private void DeleteMarkedTravelDetails()
        {
            var listTravelDetailsDeletedSession = (List<tbTravelDetail>)Session["travelDetailsDeleted"];

            if (listTravelDetailsDeletedSession != null && listTravelDetailsDeletedSession.Count > 0)
            {
                var listTravelDetailSession = (List<tbTravelDetail>)Session["travelDetails"];

                foreach (var deletedTravelDetail in listTravelDetailsDeletedSession)
                {
                    var Exist = listTravelDetailSession.Find(x => x.employee_ID == deletedTravelDetail.employee_ID);

                    if (Exist != null)
                    {
                        return;
                    }

                    var ErrorORTravelDetailDeleted = db.UDP_Gral_tbTravelDetail_Delete(deletedTravelDetail.travel_Detail_ID).FirstOrDefault();

                    if (ErrorORTravelDetailDeleted is null || ErrorORTravelDetailDeleted.ErrorMessage.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "No se pudo agregar el registro");
                        return;
                    }
                }
            }
        }

        #endregion Actions

        public List<tbTravelDetail> GetEmployeesAddedTravelFromSession(tbTravel tbTravel)
        {
            var listTravelDetailSession = (List<tbTravelDetail>)Session["travelDetails"];

            if (listTravelDetailSession == null)
            {
                listTravelDetailSession = new List<tbTravelDetail>();
                Session["travelDetails"] = listTravelDetailSession;
            }

            foreach (tbTravelDetail travelDetail in listTravelDetailSession)
            {
                var employeeInfo = db.tbEmployees.Find(travelDetail.employee_ID)?.tbEmployeesSubsidiaries.FirstOrDefault();
                var transporterInfo = db.tbTransporters.Find(tbTravel.transporter_ID);

                if (employeeInfo == null)
                {
                    throw new InvalidOperationException("Employee information cannot be null.");
                }

                travelDetail.travel_ID = tbTravel.travel_ID;
                travelDetail.distance_Kilometers = employeeInfo.employeeSubsidiary_DistanceKM.Value;
                travelDetail.travel_Cost = Math.Round(transporterInfo.transporter_Fee * travelDetail.distance_Kilometers, 2);
            }


            return listTravelDetailSession.ToList();
        }

        // GET: /Travel/Delete/5
        [SessionManager("Travel/Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTravel tbTravel = db.tbTravels.Find(id);
            if (tbTravel == null)
            {
                return HttpNotFound();
            }
            return View(tbTravel);
        }

        // POST: /Travel/Delete/5
        [SessionManager("Travel/Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                tbTravel tbTravel = db.tbTravels.Find(id);

                using (TransactionScope Tran = new TransactionScope())
                {
                    var TravelDelete = db.UDP_Gral_tbTravel_With_tbTravelDetail_Delete(id).FirstOrDefault();

                    if (TravelDelete is null || TravelDelete.ErrorMessage.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "No se pudo agregar el registro");
                        return View(tbTravel);
                    }
                    Tran.Complete();
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception message
                Console.WriteLine("Exception: " + ex.Message);
                throw; // Rethrow the exception to prevent silent failure
            }
            return RedirectToAction("Index");
        }

        #endregion ActionResult

        #region JsonResult

        [HttpPost]
        public JsonResult GenerateReportJS(int transporter_ID, string InitialDate, string FinalDate)
        {
            try
            {
                var vInitialDate = DateTime.SpecifyKind(Convert.ToDateTime(InitialDate), DateTimeKind.Local);
                var vFinalDate = DateTime.SpecifyKind(Convert.ToDateTime(FinalDate), DateTimeKind.Local);

                var query = from travel in db.tbTravels
                            where travel.transporter_ID == transporter_ID &&
                                  travel.departure_Date_and_Time >= vInitialDate &&
                                  travel.departure_Date_and_Time <= vFinalDate
                            let totalTravelCost = db.tbTravels.Sum(t => t.total_travel_Cost)
                            from travelDetail in db.tbTravelDetails
                            where travel.travel_ID == travelDetail.travel_ID
                            from employee in db.tbEmployees
                            where travelDetail.employee_ID == employee.employee_ID
                            from subsidiary in db.tbSubsidiaries
                            where travel.subsidiary_ID == subsidiary.subsidiary_ID
                            from transporter in db.tbTransporters
                            where travel.transporter_ID == transporter.transporter_ID
                            select new TravelReportDataModel
                            {
                                Travel_ID = travel.travel_ID,
                                Departure_Date_and_Time = travel.departure_Date_and_Time,
                                Total_distance_Kilometers = travel.distance_Kilometers,
                                Total_travel_Cost_per_Date = totalTravelCost,
                                TotalTravelCost = travel.total_travel_Cost,
                                Subsidiary_Name = subsidiary.subsidiary_Name,
                                Subsidiary_Direction = subsidiary.subsidiary_Direction,
                                Employee_Name = employee.employee_Name,
                                Employee_Direction = employee.employee_Direction,
                                Distance_Kilometers = travelDetail.distance_Kilometers,
                                Transporter_Name = transporter.transporter_Name,
                                Transporter_Fee = transporter.transporter_Fee
                            };

                List<TravelReportDataModel> reportData = query.ToList();

                return Json(reportData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult GetAddress(int? subsidiary_ID)
        {
            try
            {
                tbSubsidiary subsidiary = db.tbSubsidiaries.Where(x => x.subsidiary_ID == subsidiary_ID).FirstOrDefault();

                if (subsidiary is null)
                {
                    return Json(string.Empty);
                }

                return Json(subsidiary.subsidiary_Direction.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult GetEmployeesBySubsidiary(int subsidiary_ID)
        {
            try
            {
                var response = (from employee in db.tbEmployees
                                join employeeSubsidiary in db.tbEmployeesSubsidiaries
                                on employee.employee_ID equals employeeSubsidiary.employee_ID
                                where employeeSubsidiary.subsidiary_ID == subsidiary_ID
                                select new
                                {
                                    employee.employee_ID,
                                    employee.employee_Name,
                                    employee.employee_Direction,
                                    employee.position_ID,
                                    employeeSubsidiary.tbSubsidiary.subsidiary_Name,
                                    employeeSubsidiary.employeeSubsidiary_DistanceKM
                                }).ToList();

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message.ToString(), JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult AddEmployeeTravel(int Employee, int travelDetail_ID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Return a JSON object with success false and errors
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                var listTravelDetailsSession = (List<tbTravelDetail>)Session["travelDetails"];

                if (listTravelDetailsSession == null)
                {
                    listTravelDetailsSession = new List<tbTravelDetail>();
                    Session["travelDetails"] = listTravelDetailsSession;
                }

                listTravelDetailsSession.Add(new tbTravelDetail { employee_ID = Employee });
                Modified = true;

                // Return a JSON object with success true
                return Json(new { success = true, travelDetail_ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes

                // Return a JSON object with success false and an error message
                return Json(new { success = false, error = $"No se pudo asignar el empleado al viaje, Error: {ex.Message}" }, JsonRequestBehavior.DenyGet);
            }
        }



        [HttpPost]
        public ActionResult RemoveEmployeeTravel(int Employee, int travelDetail_ID)
        {
            try
            {
                var listTravelDetailsSession = (List<tbTravelDetail>)Session["travelDetails"];
                var listTravelDetailsDeletedSession = (List<tbTravelDetail>)Session["travelDetailsDeleted"];

                if (listTravelDetailsDeletedSession is null)
                {
                    listTravelDetailsDeletedSession = new List<tbTravelDetail>();
                    Session["travelDetailsDeleted"] = listTravelDetailsDeletedSession;
                }

                if (listTravelDetailsSession != null)
                {
                    var listTravelDetailsFromDB = db.tbTravelDetails.Find(travelDetail_ID);
                    if (listTravelDetailsFromDB != null)
                    {
                        listTravelDetailsDeletedSession.Add(new tbTravelDetail { travel_Detail_ID = travelDetail_ID, employee_ID = Employee });
                    }

                    listTravelDetailsSession.RemoveAll(item => item.employee_ID == Employee);
                    Modified = true;

                    // Return a JSON object with success and travelDetail_ID
                    return Json(new { success = true, travelDetail_ID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Handle the case where the session list is null
                    // Return a JSON object with success and an error message
                    return Json(new { success = false, error = "Session list is null." }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes

                // Return a JSON object with success false and an error message
                return Json(new { success = false, error = $"No se pudo asignar el empleado al viaje, Error: {ex.Message}" }, JsonRequestBehavior.DenyGet);
            }
        }


        [HttpPost]
        public JsonResult GetTransporterInfo(int Transporter)
        {
            try
            {
                var response = db.tbTransporters.Where(x => x.transporter_ID == Transporter).Select(x => new
                {
                    x.transporter_ID,
                    x.transporter_Name,
                    x.transporter_Fee
                }).FirstOrDefault();

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult EmployeeTravelToday(int Employee)
        {
            try
            {
                bool hasRowsToday = false;
                DateTime today = DateTime.Today;

                // Query the table and check if any rows match the current date

                var query = from travelDetail in db.tbTravelDetails
                            join travel in db.tbTravels on travelDetail.travel_ID equals travel.travel_ID
                            where travelDetail.employee_ID == Employee
                               && DbFunctions.TruncateTime(travel.departure_Date_and_Time) == today
                            select travelDetail.travel_ID;

                hasRowsToday = query.Any();

                return Json(hasRowsToday, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion JsonResult

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}