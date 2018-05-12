using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    public class LeaveFunctionView
    {
        public static bool isWorking(String username, IEnumerable<dynamic> employeeOnLeave)
        {
        employeeOnLeave = employeeOnLeave.Where(r => r.username.Contains(username));
            if (employeeOnLeave.Count() >= 1)
        {
            return true;
        }
            else
        {
            return false;
        }
    }
    }
