using System.Collections.Generic;
using BillingEngine.Models.Ec2;
using BillingEngine.Models.Billing;
using System.Linq;
using System;

namespace BillingEngine
{
   public class Ec2Instance
   {
      public string InstanceId { get; set; }
      public Ec2InstanceType InstanceType { get; set; }

      private readonly List<UsageEvent> _usageEvents;

      public Ec2Instance()
      {
         _usageEvents = new();
      }

      public Ec2Instance(string instanceId, Ec2InstanceType instanceType)
      {
         InstanceId = instanceId;
         InstanceType = instanceType;
         _usageEvents = new();
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

      public MonthylEc2InstanceUsage GetUsageInMonth(MonthYear monthYear)
      {
         var events = _usageEvents.FindAll(usageEvent => usageEvent.UsedFrom.Month == monthYear.Month && usageEvent.UsedFrom.Year == monthYear.Year);

         if(events.Count != 0)
         {
            var instance = new MonthylEc2InstanceUsage(InstanceId, InstanceType,events);
            return instance;
         }
         
         return null;
      }

      public DateTime GetMinimumValueOfUsedFrom()
      {
         return _usageEvents.Select(usage => usage.UsedFrom).Min();
      }
   }
}
