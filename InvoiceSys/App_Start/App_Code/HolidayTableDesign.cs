﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class HolidayTableDesign
    {
        public static int loop(DateTime date)
        {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Monday:
                return 0;
            case DayOfWeek.Tuesday:
                return 1;
            case DayOfWeek.Wednesday:
                return 2;
            case DayOfWeek.Thursday:
                return 3;
            case DayOfWeek.Friday:
                return 4;
            case DayOfWeek.Saturday:
                return 5;
            case DayOfWeek.Sunday:
                return 6;
            default:
                return 0;
        }
    }
    }
