using System;
using System.Collections.Generic;
using System.Linq;
using BillingEngine.Models.Billing;
using BillingEngine.Models;
using BillingEngine.Models.ElasticIPService;

namespace BillingEngine
{
   public class Customer
   {
      public string CustomerId { get; set; }
      public string CustomerName { get; set; }

      private readonly List<Ec2Instance> _instances;

      public readonly List<ElasticIP> ElasticIPs;

      public Customer()
      {
         _instances = new();
         ElasticIPs = new();
      }

      public Customer(string customerId, string customerName, List<Ec2Instance> instances,List<ElasticIP> elasticIPs)
      {
         CustomerId = customerId;
         CustomerName = customerName;
         _instances = instances;
         ElasticIPs = elasticIPs;
      }

      public void AddInstance(Ec2Instance instance)
      {
         _instances.Add(instance);
      }

      public List<Ec2Instance> GetAllInstances()
      {
         return _instances;
      }

      public List<MonthYear> GetDistinctMonthYear()
      {
         var listOfMonthYear = new List<MonthYear>();

         _instances.ForEach(instance => instance.GetUsageEvents().ForEach(usageEvent =>
         {
            if (!listOfMonthYear.Exists(monthYear => monthYear.Month == usageEvent.UsedFrom.Month && monthYear.Year == usageEvent.UsedFrom.Year))
            {
               listOfMonthYear.Add(new MonthYear(usageEvent.UsedFrom.Month, usageEvent.UsedFrom.Year));
            }
         }));

         return listOfMonthYear;
      }

      public List<MonthylEc2InstanceUsage> GetUsageRecordsInMonth(MonthYear monthYear)
      {
         var list = new List<MonthylEc2InstanceUsage>();

         _instances.ForEach(instance =>
         {
            var usage = instance.GetUsageInMonth(monthYear);
            if (usage != null)
            {
               list.Add(usage);
            }
         });

         return list;
      }

      public DateTime GetJoiningDate()
      {
         return _instances
             .Select(instance => instance.GetMinimumValueOfUsedFrom())
             .Min();
      }
   }
}
