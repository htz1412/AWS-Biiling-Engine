using System;

namespace BillingEngine.Models.Billing
{
   public class AggregatedMonthlyEc2Usage
   {
      public string Region { get; set; }
      public string ResourceType { get; set; }
      public int TotalResources { get; set; }

      public TimeSpan TotalUsedTime { get; set; }
      public TimeSpan TotalBilledTime { get; set; }
      public TimeSpan TotalDiscountedTime { get; set; }

      public double TotalDiscount{get;set;}
      public double TotalAmount { get; set; }

      public AggregatedMonthlyEc2Usage()
      {
         TotalUsedTime = new();
         TotalBilledTime = new();
      }

      public double GetActualAmount()
      {
         return TotalAmount - TotalDiscount;
      }
   }
}