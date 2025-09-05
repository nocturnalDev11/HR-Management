using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR_WEB.HRService;

namespace HR_WEB.Controllers
{
    public class EmployeesController : Controller
    {
        HR_SERVICE1Client client = new HR_SERVICE1Client();

        public ActionResult InsertEmployee()
        {
            var model = new EMPLOYEE_MODEL();
            return PartialView("InsertEmployee", model);
        }

        [HttpPost]
        public ActionResult InsertEmployee(HR_WEB.HRService.EMPLOYEE_MODEL employeeModel)
        {
            System.Diagnostics.Debug.WriteLine("FIRST_NAME: " + employeeModel.FIRST_NAME);
            System.Diagnostics.Debug.WriteLine("LAST_NAME: " + employeeModel.LAST_NAME);

            var result = client.InsertEmployee(employeeModel);


            if (!string.IsNullOrEmpty(result) && result.ToUpper().Contains("SUCCESS"))
            {
                TempData["Message"] = "Employee inserted successfully";
            }
            else
            {
                TempData["Message"] = "Error: could not insert employee";
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpdateEmployee(int id)
        {
            var emp = client.GetEmployeeById(id);
            if (emp == null) return HttpNotFound();
            return PartialView("UpdateEmployee", emp);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(EMPLOYEE_MODEL employeeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeModel);
            }

            var result = client.UpdateEmployee(employeeModel);

            TempData["Message"] = (!string.IsNullOrEmpty(result) && result.ToUpper().Contains("SUCCESS"))
                ? "Employee updated successfully"
                : "Error: could not update employee";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteEmployee(int id)
        {
            var emp = client.GetEmployeeById(id);
            if (emp == null) return HttpNotFound();
            return PartialView("DeleteEmployee", emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(EMPLOYEE_MODEL model)
        {
            var result = client.DeleteEmployee(model.EMPLOYEE_ID);
            TempData["Message"] = (!string.IsNullOrEmpty(result) && result.ToUpper().Contains("SUCCESS"))
                ? "Employee deleted successfully"
                : "Error: could not update employee";

            return RedirectToAction("Index", "Home");
        }
    }
}