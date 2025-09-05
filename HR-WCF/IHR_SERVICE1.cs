using HR_PROJECT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HR_PROJECT
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHR_SERVICE1" in both code and config file together.
    [ServiceContract]
    public interface IHR_SERVICE1
    {
        [OperationContract]
        List<EMPLOYEE_MODEL> GetAllEmployees();

        [OperationContract]
        EMPLOYEE_MODEL GetEmployeeById(int employeeId);

        [OperationContract]
        string InsertEmployee(EMPLOYEE_MODEL emp);

        [OperationContract]
        string UpdateEmployee(EMPLOYEE_MODEL emp);

        [OperationContract]
        string DeleteEmployee(int employeeId);
    }
}
