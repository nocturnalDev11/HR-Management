using HR_PROJECT.BusinessLayers;
using HR_PROJECT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HR_PROJECT
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HR_SERVICE1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HR_SERVICE1.svc or HR_SERVICE1.svc.cs at the Solution Explorer and start debugging.
    public class HR_SERVICE1 : IHR_SERVICE1
    {
        public List<EMPLOYEE_MODEL> GetAllEmployees()
        {
            EMPLOYEE_BL empBL = new EMPLOYEE_BL();
            return empBL.GetAllEmployees();
        }

        public EMPLOYEE_MODEL GetEmployeeById(int employeeId)
        {
            EMPLOYEE_BL empBL = new EMPLOYEE_BL();
            return empBL.GetEmployeeById(employeeId);
        }

        public string InsertEmployee(EMPLOYEE_MODEL emp)
        {
            EMPLOYEE_BL empBL = new EMPLOYEE_BL();
            return empBL.InsertEmployee(emp);
        }

        public string UpdateEmployee(EMPLOYEE_MODEL emp)
        {
            EMPLOYEE_BL empBL = new EMPLOYEE_BL();
            return empBL.UpdateEmployee(emp);
        }

        public string DeleteEmployee(int employeeId)
        {
            EMPLOYEE_BL empBL = new EMPLOYEE_BL();
            return empBL.DeleteEmployee(employeeId);
        }
    }
}
