using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_PROJECT.Model
{
    public class EMPLOYEE_MODEL
    {
        public int EMPLOYEE_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string  PHONE_NUMBER { get; set; }
        public DateTime HIRE_DATE { get; set; }
        public string JOB_ID { get; set; }
        public  decimal? SALARY { get; set; }
        public decimal? COMMISION_PCT { get; set; }
        public int? MANAGER_ID { get; set; }
        public int? DEPARTMENT_ID { get; set; }
        
    }
}