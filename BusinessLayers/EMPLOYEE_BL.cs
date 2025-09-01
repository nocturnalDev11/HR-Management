using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using HR_PROJECT.Model;

namespace HR_PROJECT.BusinessLayers
{
    public class EMPLOYEE_BL
    {
        private readonly string DBCONN = ConfigurationManager.AppSettings["DBCONN"];

        // ==============================
        // READ - Get ALL employees
        // ==============================
        public List<EMPLOYEE_MODEL> GetAllEmployees()
        {
            List<EMPLOYEE_MODEL> _employees = new List<EMPLOYEE_MODEL>();

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                conn.Open();

                string qry = @"
                    SELECT EMPLOYEE_ID, FIRST_NAME, LAST_NAME, EMAIL, PHONE_NUMBER,
                           HIRE_DATE, JOB_ID, SALARY, COMMISSION_PCT, MANAGER_ID, DEPARTMENT_ID
                    FROM EMPLOYEES";

                using (OracleCommand cmd = new OracleCommand(qry, conn))
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        EMPLOYEE_MODEL emp = new EMPLOYEE_MODEL()
                        {
                            EMPLOYEE_ID = dr["EMPLOYEE_ID"] != DBNull.Value ? Convert.ToInt32(dr["EMPLOYEE_ID"]) : 0,
                            FIRST_NAME = dr["FIRST_NAME"] != DBNull.Value ? dr["FIRST_NAME"].ToString() : null,
                            LAST_NAME = dr["LAST_NAME"] != DBNull.Value ? dr["LAST_NAME"].ToString() : null,
                            EMAIL = dr["EMAIL"] != DBNull.Value ? dr["EMAIL"].ToString() : null,
                            PHONE_NUMBER = dr["PHONE_NUMBER"] != DBNull.Value ? dr["PHONE_NUMBER"].ToString() : null,
                            HIRE_DATE = dr["HIRE_DATE"] != DBNull.Value ? Convert.ToDateTime(dr["HIRE_DATE"]) : DateTime.MinValue,
                            JOB_ID = dr["JOB_ID"] != DBNull.Value ? dr["JOB_ID"].ToString() : null,
                            SALARY = dr["SALARY"] != DBNull.Value ? Convert.ToDecimal(dr["SALARY"]) : (decimal?)null,
                            COMMISION_PCT = dr["COMMISSION_PCT"] != DBNull.Value ? Convert.ToDecimal(dr["COMMISSION_PCT"]) : (decimal?)null,
                            MANAGER_ID = dr["MANAGER_ID"] != DBNull.Value ? Convert.ToInt32(dr["MANAGER_ID"]) : (int?)null,
                            DEPARTMENT_ID = dr["DEPARTMENT_ID"] != DBNull.Value ? Convert.ToInt32(dr["DEPARTMENT_ID"]) : (int?)null
                        };

                        _employees.Add(emp);
                    }
                }
            }

            return _employees;
        }

        // ==============================
        // CREATE - Insert Employee
        // ==============================
        public string InsertEmployee(EMPLOYEE_MODEL emp)
        {
            string result = "Failed to insert employee";

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                try
                {
                    conn.Open();

                    string qry = @"
                        INSERT INTO EMPLOYEES
                        (EMPLOYEE_ID, FIRST_NAME, LAST_NAME, EMAIL, PHONE_NUMBER,
                         HIRE_DATE, JOB_ID, SALARY, COMMISSION_PCT, MANAGER_ID, DEPARTMENT_ID)
                        VALUES
                        (:EMPLOYEE_ID, :FIRST_NAME, :LAST_NAME, :EMAIL, :PHONE_NUMBER,
                         :HIRE_DATE, :JOB_ID, :SALARY, :COMMISSION_PCT, :MANAGER_ID, :DEPARTMENT_ID)";

                    using (OracleCommand cmd = new OracleCommand(qry, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", emp.EMPLOYEE_ID));
                        cmd.Parameters.Add(new OracleParameter("FIRST_NAME", emp.FIRST_NAME));
                        cmd.Parameters.Add(new OracleParameter("LAST_NAME", emp.LAST_NAME));
                        cmd.Parameters.Add(new OracleParameter("EMAIL", emp.EMAIL));
                        cmd.Parameters.Add(new OracleParameter("PHONE_NUMBER", emp.PHONE_NUMBER));
                        cmd.Parameters.Add(new OracleParameter("HIRE_DATE", emp.HIRE_DATE));
                        cmd.Parameters.Add(new OracleParameter("JOB_ID", emp.JOB_ID));
                        cmd.Parameters.Add(new OracleParameter("SALARY", emp.SALARY ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("COMMISSION_PCT", emp.COMMISION_PCT ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MANAGER_ID", emp.MANAGER_ID ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("DEPARTMENT_ID", emp.DEPARTMENT_ID ?? (object)DBNull.Value));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = "Employee inserted successfully";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        // ==============================
        // UPDATE - Update Employee
        // ==============================
        public string UpdateEmployee(EMPLOYEE_MODEL emp)
        {
            if (emp.EMPLOYEE_ID == 0)
                return "Error: EMPLOYEE_ID is required";

            string result = "Failed to update employee";

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                try
                {
                    conn.Open();

                    var setClauses = new List<string>();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;

                    // Helper method to add parameters for non-nullable fields
                    void AddNonNullableParam(string column, string paramName, object value)
                    {
                        if (value != null)
                        {
                            // For strings, check if it's not empty/whitespace
                            if (value is string strValue)
                            {
                                if (!string.IsNullOrWhiteSpace(strValue))
                                {
                                    setClauses.Add($"{column} = :{paramName}");
                                    cmd.Parameters.Add(new OracleParameter(paramName, value));
                                }
                            }
                            else
                            {
                                setClauses.Add($"{column} = :{paramName}");
                                cmd.Parameters.Add(new OracleParameter(paramName, value));
                            }
                        }
                        // If value is null, skip it (don't update non-nullable field to NULL)
                    }

                    // Helper method to add parameters for nullable fields
                    void AddNullableParam(string column, string paramName, object value)
                    {
                        if (value != null)
                        {
                            // For strings, check if it's not empty/whitespace
                            if (value is string strValue)
                            {
                                if (!string.IsNullOrWhiteSpace(strValue))
                                {
                                    setClauses.Add($"{column} = :{paramName}");
                                    cmd.Parameters.Add(new OracleParameter(paramName, value));
                                }
                                else
                                {
                                    // Allow setting to NULL for empty strings
                                    setClauses.Add($"{column} = NULL");
                                }
                            }
                            else
                            {
                                setClauses.Add($"{column} = :{paramName}");
                                cmd.Parameters.Add(new OracleParameter(paramName, value));
                            }
                        }
                        else
                        {
                            // Allow setting to NULL
                            setClauses.Add($"{column} = NULL");
                        }
                    }

                    // NON-NULLABLE FIELDS (from your schema)
                    AddNonNullableParam("FIRST_NAME", "FIRST_NAME", emp.FIRST_NAME);
                    AddNonNullableParam("LAST_NAME", "LAST_NAME", emp.LAST_NAME);
                    AddNonNullableParam("EMAIL", "EMAIL", emp.EMAIL);
                    AddNonNullableParam("JOB_ID", "JOB_ID", emp.JOB_ID);

                    // HIRE_DATE - non-nullable, only update if not default/min value
                    if (emp.HIRE_DATE != DateTime.MinValue)
                    {
                        setClauses.Add("HIRE_DATE = :HIRE_DATE");
                        cmd.Parameters.Add(new OracleParameter("HIRE_DATE", emp.HIRE_DATE));
                    }

                    // NULLABLE FIELDS (from your schema)
                    AddNullableParam("PHONE_NUMBER", "PHONE_NUMBER", emp.PHONE_NUMBER);
                    AddNullableParam("SALARY", "SALARY", emp.SALARY);
                    AddNullableParam("COMMISSION_PCT", "COMMISSION_PCT", emp.COMMISION_PCT);
                    AddNullableParam("MANAGER_ID", "MANAGER_ID", emp.MANAGER_ID);
                    AddNullableParam("DEPARTMENT_ID", "DEPARTMENT_ID", emp.DEPARTMENT_ID);

                    if (setClauses.Count == 0)
                        return "No valid fields to update";

                    cmd.CommandText = $@"
                UPDATE EMPLOYEES
                SET {string.Join(", ", setClauses)}
                WHERE EMPLOYEE_ID = :EMPLOYEE_ID";

                    cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", emp.EMPLOYEE_ID));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0 ? "Employee updated successfully" : "Employee not found";
                }
                catch (Exception ex)
                {
                    result = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        // ==============================
        // DELETE - Delete Employee
        // ==============================
        public string DeleteEmployee(int employeeId)
        {
            string result = "Failed to delete employee";

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                try
                {
                    conn.Open();

                    string qry = "DELETE FROM EMPLOYEES WHERE EMPLOYEE_ID = :EMPLOYEE_ID";

                    using (OracleCommand cmd = new OracleCommand(qry, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = "Employee deleted successfully";
                        }
                        else
                        {
                            result = "Employee not found";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        // Delete Employee Including Children (haha)
        public string DeleteEmployeeIncludingChildren(int employeeId)
        {
            string result = "Failed to delete employee";

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Set MANAGER_ID to NULL for employees managed by this employee
                    using (OracleCommand cmd = new OracleCommand("UPDATE EMPLOYEES SET MANAGER_ID = NULL WHERE MANAGER_ID = :EMPLOYEE_ID", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Optional: Handle DEPARTMENT_ID references (if departments should be deleted or set NULL)
                    using (OracleCommand cmd = new OracleCommand("UPDATE EMPLOYEES SET DEPARTMENT_ID = NULL WHERE DEPARTMENT_ID = (SELECT DEPARTMENT_ID FROM EMPLOYEES WHERE EMPLOYEE_ID = :EMPLOYEE_ID)", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Optional: Handle JOB_ID references
                    using (OracleCommand cmd = new OracleCommand("UPDATE EMPLOYEES SET JOB_ID = NULL WHERE JOB_ID = (SELECT JOB_ID FROM EMPLOYEES WHERE EMPLOYEE_ID = :EMPLOYEE_ID)", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    // 4. Delete the employee
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM EMPLOYEES WHERE EMPLOYEE_ID = :EMPLOYEE_ID", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));
                        cmd.Transaction = transaction;
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            transaction.Commit();
                            result = "Employee deleted successfully (children updated)";
                        }
                        else
                        {
                            transaction.Rollback();
                            result = "Employee not found";
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        // ==============================
        // READ - Get Employee by ID
        // ==============================
        public EMPLOYEE_MODEL GetEmployeeById(int employeeId)
        {
            EMPLOYEE_MODEL emp = null;

            using (OracleConnection conn = new OracleConnection(DBCONN))
            {
                conn.Open();

                string qry = @"
                    SELECT EMPLOYEE_ID, FIRST_NAME, LAST_NAME, EMAIL, PHONE_NUMBER,
                           HIRE_DATE, JOB_ID, SALARY, COMMISSION_PCT, MANAGER_ID, DEPARTMENT_ID
                    FROM EMPLOYEES
                    WHERE EMPLOYEE_ID = :EMPLOYEE_ID";

                using (OracleCommand cmd = new OracleCommand(qry, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("EMPLOYEE_ID", employeeId));

                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            emp = new EMPLOYEE_MODEL()
                            {
                                EMPLOYEE_ID = Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                FIRST_NAME = dr["FIRST_NAME"]?.ToString(),
                                LAST_NAME = dr["LAST_NAME"]?.ToString(),
                                EMAIL = dr["EMAIL"]?.ToString(),
                                PHONE_NUMBER = dr["PHONE_NUMBER"]?.ToString(),
                                HIRE_DATE = dr["HIRE_DATE"] != DBNull.Value ? Convert.ToDateTime(dr["HIRE_DATE"]) : DateTime.MinValue,
                                JOB_ID = dr["JOB_ID"]?.ToString(),
                                SALARY = dr["SALARY"] != DBNull.Value ? Convert.ToDecimal(dr["SALARY"]) : (decimal?)null,
                                COMMISION_PCT = dr["COMMISSION_PCT"] != DBNull.Value ? Convert.ToDecimal(dr["COMMISSION_PCT"]) : (decimal?)null,
                                MANAGER_ID = dr["MANAGER_ID"] != DBNull.Value ? Convert.ToInt32(dr["MANAGER_ID"]) : (int?)null,
                                DEPARTMENT_ID = dr["DEPARTMENT_ID"] != DBNull.Value ? Convert.ToInt32(dr["DEPARTMENT_ID"]) : (int?)null
                            };
                        }
                    }
                }
            }
            return emp;
        }
    }
}
