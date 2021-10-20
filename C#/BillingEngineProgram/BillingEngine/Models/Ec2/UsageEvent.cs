using System;

namespace BillingEngine
{
   public class UsageEvent
   {
      public DateTime UsedFrom { get; set; }
      public DateTime UsedUntill { get; set; }

      public UsageEvent()
      {
         UsedFrom = new DateTime();
         UsedUntill = new DateTime();
      }
      public UsageEvent(DateTime usedFrom, DateTime usedUntill)
      {
         UsedFrom = usedFrom;
         UsedUntill = usedUntill;
      }

      public int GetBillableHours()
      {
         var usedHours = UsedUntill.Subtract(UsedFrom).TotalHours;
         return Convert.ToInt32(Math.Ceiling(usedHours));
      }
   }
}
