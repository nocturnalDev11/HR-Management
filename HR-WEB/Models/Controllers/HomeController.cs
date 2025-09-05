using HR_WEB.HRService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HR_WEB.Controllers
{
    public class HomeController : Controller
    {
        HR_SERVICE1Client client = new HRService.HR_SERVICE1Client();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> GetAllEmployees()
        {
            var employees = await client.GetAllEmployeesAsync();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> InsertEmployee(EMPLOYEE_MODEL _employeeModel)
        {
            var result = await client.InsertEmployeeAsync(_employeeModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}