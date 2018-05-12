using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

    public class CheckHolidayTable
    {
        public static String isHoliday(String company_name, DateTime working_date)
        {
        Database db = Database.Open("InvoiceSysDBContext");
        var company_state = db.QueryValue("SELECT TOP 1 state FROM [InvoiceSys].[dbo].[Customer_PrivateInfo] WHERE company_name=@0",company_name);
            if (!String.IsNullOrEmpty(company_state))
            {
                var holiday = db.QueryValue("SELECT TOP 1 isHoliday FROM [InvoiceSys].[dbo].[HolidayTableDetails] WHERE state=@0 AND year=@1 AND holidayDate=@2", company_state, working_date.ToString("yyyy"), working_date.ToString("dd/MM"));
                
            if (holiday != null)
            {
                if (holiday)
                {
                    return "NBD";
                }
                else
                {
                    return "BD";
                }

            }
            else
            {
                return "BD";
            }

            }
            else
            {
                    return "BD";
            }
           
            
        }
    }
