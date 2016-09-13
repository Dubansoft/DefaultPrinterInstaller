//  Copyright 2015 Jhorman Duban Rodríguez Pulgarín
//  
//  This file is part of InkAlert.
//  
//  InkAlert is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  InkAlert is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with InkAlert.  If not, see <http://www.gnu.org/licenses/>.
//  
//  Jhorman Duban Rodríguez., hereby disclaims all copyright interest in 
//  the program "InkAlert" (which makes passes at 
//  compilers) written by Jhorman Duban Rodríguez.
//  
//  Jhorman Duban Rodríguez,
//  5 January 2016
//  For more information, visit <http://www.codigoinnovador.com/projects/inkalert/>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefaultPrinterInstaller
{
    class DateEngine
    {
        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        private object date;
        public object Date
        {
            set { date = value; }
        }

        /// <summary>
        /// Example: domingo, 10 de enero de 2016 06:08:02 p.m.
        /// </summary>
        public static string CurrentDateTime
        {
            get
            {
                return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            }
        }

        /// <summary>
        /// Example: 42367,456444
        /// </summary>
        public static string CurrentDateTimeDouble
        {
            get { return "" + DateTime.Now.ToOADate().ToString() + ""; }
        }

        /// <summary>
        /// Example: 42768
        /// </summary>
        public static string CurrentDateInteger
        {
            get { return "" + DateTime.Today.ToOADate().ToString() + ""; }
        }

        /// <summary>
        /// Example: 01/01/1990 11:00:15 a.m.
        /// </summary>
        public static string CurrentDateTimeShort {

            get { return DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); }

        }

        /// <summary>
        /// Example: 01/01/1990
        /// </summary>
        public static string CurrentDateShort
        {

            get { return DateTime.Today.ToShortDateString(); }

        }

        /// <summary>
        /// Example: 11:00:15 a.m.
        /// </summary>
        public static string CurrentTimeShort {

            get { return DateTime.Now.ToShortTimeString(); }
        }

        public DateEngine(object myDate)
        {
            Date = myDate;
        }

        public DateEngine() { }

        public string FromDoubleToStringDate(object date)
        {
            this.date = date;
            return FromDoubleToStringDate();
        }

        public string FromDoubleToStringDate()
        {
            try
            {
                double doubleDate = Convert.ToDouble(date);
                DateTime baseDate = DateTime.FromOADate(doubleDate);
                return baseDate.ToShortDateString() + " " + baseDate.ToShortTimeString () ;
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                return "";
            }
            
        }

        public string FromDoubleToShortStringDate(object date)
        {
            this.date = date;
            return FromDoubleToShortStringDate();
        }

        public string FromDoubleToShortStringDate()
        {
            try
            {
                double doubleDate = Convert.ToDouble(date);
                DateTime baseDate = DateTime.FromOADate(doubleDate);
                return baseDate.ToShortDateString();
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                return "";
            }

        }

        public int FromValueDateToInteger(DateTime dateValue)
        {
            return (int)dateValue.ToOADate();
        }
    }
}
