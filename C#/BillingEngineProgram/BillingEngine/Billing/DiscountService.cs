using System;
using System.Collections.Generic;
using System.Linq;
using BillingEngine.Models.Billing;

namespace BillingEngine.Billing
{
   public class DiscountService
   {
      private const int MaxFreeTierEligibleHours = 750;

      public void ApplyDiscounts(
            Customer customer,
            MonthlyBill monthlyBill)
      {
         if (monthlyBill.MonthYear.IsLesserThan(customer.GetJoiningDate().AddYears(1)))
         {
            var freeTierEligibleLinuxInstances = monthlyBill.GetFreeTierEligibleInstanceUsagesOfType("Linux");
            var freeTierEligibleWindowsInstances = monthlyBill.GetFreeTierEligibleInstanceUsagesOfType("Windows");

            DistributeFreeTierEligibleHoursAcrossInstances(
                freeTierEligibleLinuxInstances,
                MaxFreeTierEligibleHours);

            DistributeFreeTierEligibleHoursAcrossInstances(
                freeTierEligibleWindowsInstances,
                MaxFreeTierEligibleHours);
         }
      }

      private void DistributeFreeTierEligibleHoursAcrossInstances(
            List<MonthylEc2InstanceUsage> monthlyFreeTierEligibleInstanceUsages,
            int maxFreeTierEligibleHours)
      {
         int remainingFreeTierEligibleHours = maxFreeTierEligibleHours;

         for (int i = 0; i < monthlyFreeTierEligibleInstanceUsages.Count && remainingFreeTierEligibleHours > 0; ++i)
         {
            if (monthlyFreeTierEligibleInstanceUsages[i].Ec2InstanceType.IsOnDemand)
            {
               var freeTierEligibleInstance = monthlyFreeTierEligibleInstanceUsages[i];
               var discountedHours = CalculateDiscountedHoursFor(
                   freeTierEligibleInstance,
                   remainingFreeTierEligibleHours
               );

               freeTierEligibleInstance.ApplyDiscount(TimeSpan.FromHours(discountedHours));

               remainingFreeTierEligibleHours -= discountedHours;
            }
         }
      }

      private int CalculateDiscountedHoursFor(
          MonthylEc2InstanceUsage monthlyFreeTierEligibleInstanceUsage,
          int availableFreeTierEligibleHours)
      {
         var totalBillableHours = monthlyFreeTierEligibleInstanceUsage.GetTotalBillableHours();
         return totalBillableHours < availableFreeTierEligibleHours
             ? totalBillableHours
             : availableFreeTierEligibleHours;
      }
   }
}
