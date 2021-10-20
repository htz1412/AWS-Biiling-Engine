using System;
using System.Collections.Generic;
using System.Linq;
using BillingEngine.Models.Ec2;

namespace BillingEngine.Models.Billing
{
   public class MonthlyBill
   {
      public string CustomerId { get; set; }
      public string CustomerName { get; set; }

      public MonthYear MonthYear { get; set; }

      private readonly List<MonthylEc2InstanceUsage> _monthlyEc2InstanceUsages;

      public MonthlyBill()
      {
         _monthlyEc2InstanceUsages = new();
      }

      public MonthlyBill(string customerId, string customerName, MonthYear monthYear)
      {
         CustomerId = customerId;
         CustomerName = customerName;
         MonthYear = monthYear;
         _monthlyEc2InstanceUsages = new();
      }

      public List<MonthylEc2InstanceUsage> GetMonthylEc2InstanceUsages()
      {
         return _monthlyEc2InstanceUsages;
      }

      public void AddMonthlyEc2UsageRecords(List<MonthylEc2InstanceUsage> monthlyEc2UsageRecords)
      {
         _monthlyEc2InstanceUsages.AddRange(monthlyEc2UsageRecords);
      }

      public List<AggregatedMonthlyEc2Usage> GetAggregatedMonthlyEc2Usages()
      {
         //Using MonthlyEc2InstanceUsages, compute List<AggregatedMonthlyEc2Usage>
         var list = new List<AggregatedMonthlyEc2Usage>();

         foreach (var usage in GetDistinctTypes())
         {
            var type = usage.Ec2InstanceType;
            var totalResources = GetTotalResourcesForType(type);
            var totalUsedTime = GetTotalUsedTime(type);

            if (totalUsedTime.TotalHours == 0)
            {
               continue;
            }

            var totalBilledTime = GetTotalBilledTime(totalUsedTime);
            var totalAmount = GetTotalAmountForEc2InstanceType(type);
            var discount = GetTotalDiscountForType();

            list.Add(new AggregatedMonthlyEc2Usage
            {
               Region = type.Region,
               ResourceType = type.InstanceType,
               TotalResources = totalResources,
               TotalUsedTime = totalUsedTime,
               TotalBilledTime = totalBilledTime,
               TotalAmount = totalAmount,
               TotalDiscount = discount,
               TotalDiscountedTime = GetTotalDiscountedTime(),
            });
         }

         return list;
      }

      private List<MonthylEc2InstanceUsage> GetDistinctTypes()
      {
         var distinctTypes = new List<MonthylEc2InstanceUsage>();

         _monthlyEc2InstanceUsages.ForEach(usage =>
         {
            if (!distinctTypes.Exists(record => record.Ec2InstanceType.InstanceType == usage.Ec2InstanceType.InstanceType && record.Ec2InstanceType.Region == usage.Ec2InstanceType.Region))
            {
               distinctTypes.Add(usage);
            }
         });

         return distinctTypes;
      }

      public List<MonthylEc2InstanceUsage> GetFreeTierEligibleInstanceUsagesOfType(string os)
      {
         return _monthlyEc2InstanceUsages
             .Where(instanceUsage => instanceUsage.Ec2InstanceType.IsFreeTierEligible)
             .Where(instanceUsage => instanceUsage.Ec2InstanceType.Os.Equals(os))
             .ToList();
      }

      public int GetTotalResourcesForType(Ec2InstanceType ec2InstanceType)
      {
         int count = 0;

         _monthlyEc2InstanceUsages.ForEach(usage =>
         {
            if (ec2InstanceType.InstanceType == usage.Ec2InstanceType.InstanceType && ec2InstanceType.Region == usage.Ec2InstanceType.Region)
            {
               count++;
            }
         });

         return count;
      }

      public TimeSpan GetTotalUsedTime(Ec2InstanceType ec2InstanceType)
      {
         var totalUsedTime = new TimeSpan();

         _monthlyEc2InstanceUsages.ForEach(usage =>
         {
            if (usage.Ec2InstanceType.InstanceType == ec2InstanceType.InstanceType && usage.Ec2InstanceType.Region == ec2InstanceType.Region)
            {
               totalUsedTime = totalUsedTime.Add(usage.GetUsedTime());
            }
         });

         return totalUsedTime;
      }

      public TimeSpan GetTotalBilledTime(TimeSpan totalUsedTime)
      {
         if(totalUsedTime.Minutes > 0 || totalUsedTime.Seconds > 0)
         {
            return new TimeSpan(totalUsedTime.Days, totalUsedTime.Hours + 1, 0, 0);
         }

         return totalUsedTime;
      }

      private double GetTotalAmountForEc2InstanceType(Ec2InstanceType ec2InstanceType)
      {
         var totalAmount = 0d;

         _monthlyEc2InstanceUsages.ForEach(usage =>
         {
            if (usage.Ec2InstanceType.InstanceType == ec2InstanceType.InstanceType && usage.Ec2InstanceType.Region == ec2InstanceType.Region)
            {
               totalAmount += (GetTotalBilledTime(usage.GetUsedTime()).TotalHours * usage.Ec2InstanceType.RatePerHour);
            }
         });

         return totalAmount;
      }

      public double GetTotalAmount(List<AggregatedMonthlyEc2Usage> aggregatedMonthlyEc2Usages)
      {
         var totalAmount = 0d;

         aggregatedMonthlyEc2Usages.ForEach(usage => totalAmount += usage.TotalAmount);

         return totalAmount;
      }
      
      private double GetTotalDiscountForType()
      {
         var discount = 0d;

         _monthlyEc2InstanceUsages.ForEach(usage => discount += (usage.DiscountedHours.TotalHours * usage.Ec2InstanceType.RatePerHour));

         return discount;
      }

      public TimeSpan GetTotalDiscountedTime()
      {
         var discountedHours = new TimeSpan();

         _monthlyEc2InstanceUsages.ForEach(usage => discountedHours = discountedHours.Add(usage.DiscountedHours));

         return discountedHours;
      }

      public double GetTotalDiscount(List<AggregatedMonthlyEc2Usage> aggregatedMonthlyEc2Usages)
      {
         var totalDiscount = 0d;

         aggregatedMonthlyEc2Usages.ForEach(usage => totalDiscount += usage.TotalDiscount);

         return totalDiscount;
      }

      public double GetAmountToBePaid(List<AggregatedMonthlyEc2Usage> aggregatedMonthlyEc2Usages)
      {
         return GetTotalAmount(aggregatedMonthlyEc2Usages) - GetTotalDiscount(aggregatedMonthlyEc2Usages);
      }
   }
}
