using BillingEngine.Models.Ec2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Models.Billing
{
   public class MonthylEc2InstanceUsage
   {
      public string Ec2InstanceId { get; set; }
      public Ec2InstanceType Ec2InstanceType { get; set; }
      public TimeSpan DiscountedHours { get; private set; }

      private readonly List<UsageEvent> _usageEvents;

      public MonthylEc2InstanceUsage()
      {
         _usageEvents = new();
      }

      public MonthylEc2InstanceUsage(string ec2InstanceId, Ec2InstanceType ec2InstanceType, List<UsageEvent> usageEvents)
      {
         Ec2InstanceId = ec2InstanceId;
         Ec2InstanceType = ec2InstanceType;
         _usageEvents = usageEvents;
      }

      public List<UsageEvent> GetUsageEvents()
      {
         return _usageEvents;
      }

      public void AddUsageEvent(UsageEvent usageEvent)
      {
         _usageEvents.Add(usageEvent);
      }

      public void AddUsageEvents(List<UsageEvent> usageEvents)
      {
         _usageEvents.AddRange(usageEvents);
      }

      public TimeSpan GetUsedTime()
      {
         var usedTime = new TimeSpan();

         _usageEvents.ForEach(usageEvent => usedTime = usedTime.Add(usageEvent.UsedUntill - usageEvent.UsedFrom));

         return usedTime;
      }

      public double GetAmountForType(TimeSpan totalBilledTime)
      {
         return Ec2InstanceType.RatePerHour * totalBilledTime.TotalHours;
      }

      public void ApplyDiscount(TimeSpan discountedHours)
      {
         //if (discountedHours >= GetUsedTime())
         //{
         //   DiscountedHours = GetUsedTime();
         //   return discountedHours.Subtract(DiscountedHours);
         //}
         //else
         //{
         //   DiscountedHours = discountedHours;
         //   return DiscountedHours;
         //}

         DiscountedHours = discountedHours;
      }

      public int GetTotalBillableHours()
      {
         return _usageEvents.Select(usage => usage.GetBillableHours()).Sum();
      }
   }
}