using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class SAPermissionChecker
    {
        public static bool isSuperAdmin(String permissionList)
        {
            String[] permission_List = permissionList.Split(',');
            if (permission_List.Contains("SuperAdmin_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
