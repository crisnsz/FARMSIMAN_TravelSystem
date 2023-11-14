using ERP_GMEDINA.Attribute;
using ERP_GMEDINA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using System.Web;


namespace ERP_GMEDINA.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly FARSIMANEntities db = new FARSIMANEntities();

        private static List<tbEmployeesSubsidiary> ListEmployeesSubsidiariesStatic { get; set; } = new List<tbEmployeesSubsidiary>();

        public static bool Modified { get; set; }

        private void CleanVariables()
        {

            Modified = false;
            Session["employeesSubsidiary"] = null;

        }
        // GET: /Employee/
        [SessionManager("Employee/Index")]
        public ActionResult Index()
        {

            CleanVariables();

            return View(db.tbEmployees.ToList());
        }

        // GET: /Employee/Details/5

        [SessionManager("Employee/Details")]
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            CleanVariables();

            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            if (tbEmployee == null)
            {
                return HttpNotFound();
            }



            ViewBag.position_ID = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);

            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);

            ListEmployeesSubsidiariesStatic = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

            ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return View(tbEmployee);







        }

        // GET: /Employee/Create

        [SessionManager("Employee/Create")]
        public ActionResult Create()
        {

            CleanVariables();
            Session["employeesSubsidiary"] = null;


            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");

            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Employee/Create")]
        public ActionResult Create([Bind(Include = "employee_ID,employee_Name,employee_Direction,position_ID")] tbEmployee tbEmployee)
        {

            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name");
            if (!ModelState.IsValid)
            {
                return View(tbEmployee);
            }
            using (TransactionScope Tran = new TransactionScope())
            {
                try
                {
                    var EmployeeInsert = db.UDP_Gral_tbEmployees_Insert(tbEmployee.employee_Name, tbEmployee.employee_Direction, tbEmployee.position_ID).FirstOrDefault();


                    if (EmployeeInsert is null || EmployeeInsert.ErrorMessage.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "Error al resgistrar el empleado");
                        return View(tbEmployee);
                    }

                    var idOrError = Convert.ToInt32(EmployeeInsert.ErrorMessage);

                    if (idOrError <= 0)
                    {
                        ModelState.AddModelError("", "Error al resgistrar el empleado");
                        Session["employeesSubsidiary"] = null;
                        return View(tbEmployee);
                    }

                    var ListEmployeesSubsidiary = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];
                    if (ListEmployeesSubsidiary != null && ListEmployeesSubsidiary.Any())
                    {
                        foreach (tbEmployeesSubsidiary employeesSubsidiary in ListEmployeesSubsidiary)
                        {
                            var EmployeesSubsidaries = db.UDP_Gral_tbEmployeesSubsidiaries_Insert(
                                idOrError, employeesSubsidiary.subsidiary_ID, employeesSubsidiary.employeeSubsidiary_DistanceKM)
                                .FirstOrDefault();

                            if (EmployeesSubsidaries is null || EmployeesSubsidaries.ErrorMessage.StartsWith("-1"))
                            {
                                ModelState.AddModelError("", "No se pudo agregar el registro detalle");
                                return View(tbEmployee);
                            }
                        }
                    }

                    Tran.Complete();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(tbEmployee);
                }
            }

        }

        // GET: /Employee/Edit/5
        [SessionManager("Employee/Edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CleanVariables();

            tbEmployee tbEmployee = db.tbEmployees.Find(id);

            if (tbEmployee == null)
            {
                return HttpNotFound();
            }

            ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);

            var listEmployeesSubsidiarySession = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);

            Session["employeesSubsidiary"] = listEmployeesSubsidiarySession.ToList();

            ViewBag.tbEmployeesSubsidiaries = listEmployeesSubsidiarySession.ToList();

            ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !listEmployeesSubsidiarySession.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return View(tbEmployee);
        }

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionManager("Employee/Edit")]
        public ActionResult Edit([Bind(Include = "employee_ID,employee_Name,employee_Direction,position_ID")] tbEmployee tbEmployee)
        {
            try
            {
                var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries
                                                .Where(x => x.employee_ID == tbEmployee.employee_ID);


                if (!ModelState.IsValid)
                {

                    CleanVariables();
                    ViewBag.tbEmployeesSubsidiaries = tbEmployeesSubsidiaries.ToList();

                    ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

                    ViewBag.Positions = new SelectList(db.tbPositions, "position_ID", "position_Name", tbEmployee.position_ID);

                    var listEmployeesSubsidiarySession = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == tbEmployee.employee_ID);


                    Session["employeesSubsidiary"] = listEmployeesSubsidiarySession.ToList();
                    ViewBag.tbSubsidiary = db.tbSubsidiaries.Where(x => !listEmployeesSubsidiarySession.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();


                    ViewBag.tbEmployeesSubsidiaries = listEmployeesSubsidiarySession.ToList();
                    return View(tbEmployee);
                }

                using (TransactionScope Tran = new TransactionScope())
                {

                    if (Modified)
                    {
                        var EmployeeUpdate = db.UDP_Gral_tbEmployees_Update(tbEmployee.employee_ID, tbEmployee.employee_Name, tbEmployee.employee_Direction, tbEmployee.position_ID).FirstOrDefault();


                        if (EmployeeUpdate is null || EmployeeUpdate.ErrorMessage.StartsWith("-1"))
                        {
                            ModelState.AddModelError("", "Error al resgistrar el empleado");
                            return View(tbEmployee);
                        }

                        var idOrError = Convert.ToInt32(EmployeeUpdate.ErrorMessage);

                        DetectChanges(tbEmployeesSubsidiaries.ToList(), GetEmployeesSubsidiaryFromSession(idOrError));

                    }
                    Tran.Complete();
                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                // Log the exception or handle it in a meaningful way
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View(tbEmployee);
            }

        }



        #region Actions
        private void DetectChanges(List<tbEmployeesSubsidiary> database, List<tbEmployeesSubsidiary> local)
        {
            // Find new and updated rows
            var newAndUpdatedRows = local.Where(localRow =>
                                 database.Exists(dbRow => dbRow.employee_ID == localRow.employee_ID &&
                                   !AreRowsEqual(localRow, dbRow)));

            foreach (var row in newAndUpdatedRows)
            {
                var existingInDatabase = database.Find(dbRow => dbRow.employeeSubsidiary_ID == row.employeeSubsidiary_ID);

                if (existingInDatabase == null)
                {
                    // Row with ID not found in database, so it is a new row
                    Console.WriteLine($"Row with ID {row.employeeSubsidiary_ID} is inserted.");
                    _ = db.UDP_Gral_tbEmployeesSubsidiaries_Insert(row.employee_ID, row.subsidiary_ID, row.employeeSubsidiary_DistanceKM).FirstOrDefault();
                }
                else
                {
                    // Row with ID found in database, so it is an updated row
                    Console.WriteLine($"Row with ID {row.employeeSubsidiary_ID} is updated.");
                    _ = db.UDP_Gral_tbEmployeesSubsidiaries_Update(row.employeeSubsidiary_ID, row.employee_ID, row.subsidiary_ID, row.employeeSubsidiary_DistanceKM).FirstOrDefault();
                }
            }

            // Find deleted rows
            var deletedRows = database.Where(dbRow =>
                !local.Exists(localRow => localRow.employeeSubsidiary_ID == dbRow.employeeSubsidiary_ID));

            foreach (var employeeSubsidiary_ID in deletedRows.Select(x => x.employeeSubsidiary_ID))
            {
                Console.WriteLine($"Row with ID {employeeSubsidiary_ID} is deleted.");
                var EmployeesSubsidiary = db.UDP_Gral_tbEmployeesSubsidiaries_Delete(employeeSubsidiary_ID).FirstOrDefault();

                if (EmployeesSubsidiary is null || EmployeesSubsidiary.ErrorMessage.StartsWith("-1"))
                {
                    ModelState.AddModelError("", "Error al eliminar la subsidiaria.");
                }
            }
        }

        private bool AreRowsEqual(tbEmployeesSubsidiary row1, tbEmployeesSubsidiary row2)
        {
            // Compare properties to check for equality
            return row1.employeeSubsidiary_ID == row2.employeeSubsidiary_ID &&
                   row1.employeeSubsidiary_DistanceKM == row2.employeeSubsidiary_DistanceKM;
            // Add more properties as needed
        }


        public List<tbEmployeesSubsidiary> GetEmployeesSubsidiaryFromSession(int employee_ID)
        {

            var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

            if (listEmployeesSubsidiarySession == null)
            {
                listEmployeesSubsidiarySession = new List<tbEmployeesSubsidiary>();
                Session["employeesSubsidiary"] = listEmployeesSubsidiarySession;
            }

            listEmployeesSubsidiarySession.ForEach(x => x.employee_ID = employee_ID);

            return listEmployeesSubsidiarySession.ToList();
        }

        [HttpPost]
        public JsonResult AddSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }
                var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

                if (listEmployeesSubsidiarySession == null)
                {
                    listEmployeesSubsidiarySession = new List<tbEmployeesSubsidiary>();
                    Session["employeesSubsidiary"] = listEmployeesSubsidiarySession;
                }

                listEmployeesSubsidiarySession.Add(tbEmployeesSubsidiary);
                Modified = true;

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return Json(new { success = false, error = $"No se pudo asignar el empleado a la subsidiaria {tbEmployeesSubsidiary.tbSubsidiary.subsidiary_Name}.", exception = ex.Message.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }


        [HttpPost]
        public JsonResult RemoveSubsidiary(tbEmployeesSubsidiary tbEmployeesSubsidiary)
        {
            try
            {
                var listEmployeesSubsidiarySession = (List<tbEmployeesSubsidiary>)Session["employeesSubsidiary"];

                if (listEmployeesSubsidiarySession != null)
                {
                    listEmployeesSubsidiarySession.RemoveAll(item => item.subsidiary_ID == tbEmployeesSubsidiary.subsidiary_ID);

                    Modified = true;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Handle the case where the session list is null
                    return Json(new { success = false, error = "Session list is null." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return Json(new { success = false, error = $"No se pudo eliminar la subsidiaria asiganda: {tbEmployeesSubsidiary.tbSubsidiary.subsidiary_Name}.", exception = ex.Message.ToString() }, JsonRequestBehavior.DenyGet);
            }
        }


        public List<tbPosition> GetPositions()
        {
            List<tbPosition> ListPositions = db.tbPositions.ToList();

            return ListPositions;
        }

        public List<tbSubsidiary> GetSubsidiaries()
        {
            List<tbSubsidiary> SubsidiariesList = db.tbSubsidiaries.ToList();

            return SubsidiariesList;
        }

        public List<tbSubsidiary> GetSubsidiaries(int Employee_ID)
        {

            var tbEmployeesSubsidiaries = db.tbEmployeesSubsidiaries.Where(x => x.employee_ID == Employee_ID);

            List<tbSubsidiary> SubsidiariesList = db.tbSubsidiaries.Where(x => !tbEmployeesSubsidiaries.Select(y => y.subsidiary_ID).Contains(x.subsidiary_ID)).ToList();

            return SubsidiariesList;
        }

        public List<tbEmployeesSubsidiary> GetEmployeesSubsidiary()
        {
            List<tbEmployeesSubsidiary> EmployeesSubsidiaries = db.tbEmployeesSubsidiaries.ToList();

            return EmployeesSubsidiaries;
        }

        #endregion
        // GET: /Employee/Delete/5
        [SessionManager("Employee/Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbEmployee tbEmployee = db.tbEmployees.Find(id);
            if (tbEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tbEmployee);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {

                tbEmployee tbEmployee = db.tbEmployees.Find(id);

                using (TransactionScope Tran = new TransactionScope())
                {
                    var EmployeeDelete = db.UDP_Gral_tbEmployees_Delete(id).FirstOrDefault();

                    if (EmployeeDelete is null || EmployeeDelete.ErrorMessage.StartsWith("-1"))
                    {
                        ModelState.AddModelError("", "No se pudo agregar el registro");
                        return View(tbEmployee);
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