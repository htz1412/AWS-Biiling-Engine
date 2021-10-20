using System;

namespace BillingEngine
{
   public class MonthYear
   {
      public int Month { get; set; }
      public int Year { get; set; }

      public MonthYear(int month,int year)
      {
         Month = month;
         Year = year;
      }

      public bool IsLesserThan(DateTime dateTime)
      {
         if (Year < dateTime.Year)
         {
            return true;
         }

         if (Year > dateTime.Year)
         {
            return false;
         }

         return Month < dateTime.Month;
      }
   }
}
