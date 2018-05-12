using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class PermissionChecker
    {
    public static bool CheckUserPermission(String permissionList, String permission, String id)
    {
        String[] permission_List = permissionList.Split(',');
        if (permission_List.Contains(permission + "_" + id))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
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


