using System.Collections.Generic;
using BillingEngine.Parser.Models;
using BillingEngine.Parser;
using System.Linq;
using System;
using BillingEngine.Models.Ec2;
using BillingEngine.Models.ElasticIPService;
using BillingEngine.DomainModelGenerators;

namespace BillingEngine.Models
{
   public class CustomerModelGenerator
   {
      private readonly ElasticIPModelGenerator _elasticIPModelGenerator;

      public CustomerModelGenerator()
      {
         _elasticIPModelGenerator = new();
      }

      public List<Customer> GenerateCustomerModels(
         List<ParsedCustomerRecord> parsedCustomerRecords,
         List<ParsedInstanceType> parsedInstanceTypes,
         List<ParsedResourceUsageRecord> parsedResourceUsageRecords,
         List<ParsedRegionRecord> parsedRegionRecords,
         List<ParsedElasticIpAllocationRecord> parsedElasticIpAllocationRecords,
         List<ParsedElasticIPAssociationRecord> parsedElasticIPAssociationRecords,
         List<ParsedElasticIPRateRecord> parsedElasticIPRateRecords
         )
      {
         return parsedCustomerRecords.Select(parsedCustomerRecord =>
            GenerateCustomerModel(
               parsedCustomerRecord,
               parsedInstanceTypes,
               parsedRegionRecords,
               parsedResourceUsageRecords.FindAll(usage => usage.CustomerId == parsedCustomerRecord.CustomerId),
               parsedElasticIpAllocationRecords.FindAll(record => record.CustomerId == parsedCustomerRecord.CustomerId),
               parsedElasticIPAssociationRecords,
               parsedElasticIPRateRecords
            )
         ).ToList();
      }

      public Customer GenerateCustomerModel(
         ParsedCustomerRecord parsedCustomerRecord,
         List<ParsedInstanceType> parsedInstanceTypes,
         List<ParsedRegionRecord> parsedRegionRecords,
         List<ParsedResourceUsageRecord> parsedResourceUsageRecords,
         List<ParsedElasticIpAllocationRecord> parsedElasticIpAllocationRecords,
         List<ParsedElasticIPAssociationRecord> parsedElasticIPAssociationRecords,
         List<ParsedElasticIPRateRecord> parsedElasticIPRateRecords
         )
      {
         var instances = new List<Ec2Instance>();

         parsedResourceUsageRecords.ForEach(usage =>
         {
            if (!instances.Exists(instance => instance.InstanceId == usage.InstanceId && instance.InstanceType.InstanceType == usage.InstanceType && instance.InstanceType.Os == usage.Os))
            {
               var ratePerHour = 0d;
               if (usage.IsOnDemand)
               {
                  ratePerHour = parsedInstanceTypes.Find(type => type.InstanceType == usage.InstanceType && type.Region == usage.Region).OnDemandCharge;
               }
               else
               {
                  ratePerHour = parsedInstanceTypes.Find(type => type.InstanceType == usage.InstanceType && type.Region == usage.Region).ReservedCharge;
               }

               instances.Add(new Ec2Instance(
                  usage.InstanceId,
                  new Ec2InstanceType(usage.InstanceType, ratePerHour, usage.Region, usage.Os, usage.IsOnDemand,
                     parsedRegionRecords.Exists(record => record.FreeTierEligible == usage.InstanceType && record.Region == usage.Region))
                  )
               );
            }
         });

         instances.ForEach(instance =>
         {
            parsedResourceUsageRecords
            .FindAll(usage => usage.InstanceId == instance.InstanceId && usage.InstanceType == instance.InstanceType.InstanceType && instance.InstanceType.Os == usage.Os)
            .ForEach(record => instance.GetUsageEvents().AddRange(SplitUsageEventInMonths(record.UsedFrom, record.UsedUntil,record.IsOnDemand)));
         });

         var elasticIPs = _elasticIPModelGenerator.GenerateElasticIPRecords(parsedCustomerRecord.CustomerId, instances, parsedElasticIpAllocationRecords, parsedElasticIPAssociationRecords, parsedElasticIPRateRecords);

         var customer = new Customer(parsedCustomerRecord.CustomerId, parsedCustomerRecord.CustomerName, instances,elasticIPs);

         return customer;
      }

      private List<UsageEvent> SplitUsageEventInMonths(DateTime usedFrom, DateTime usedUntil,bool isOnDemand)
      {
         var usageEvents = new List<UsageEvent>();

         var monthDiff = ((usedUntil.Year - usedFrom.Year) * 12) + usedUntil.Month - usedFrom.Month;

         if (monthDiff > 0)
         {
            while (monthDiff-- != 0)
            {
               var newMonth = usedFrom.Month + 1;
               var newYear = usedFrom.Year;

               if (newMonth > 12)
               {
                  newMonth %= 12;
                  newYear++;
               }

               var endOfMonth = new DateTime(newYear, newMonth, 1);
               var duration = endOfMonth - usedFrom;

               var newEvent = new UsageEvent();

               newEvent.UsedFrom = usedFrom;
               usedFrom = usedFrom.Add(duration);
               newEvent.UsedUntill = usedFrom;

               usageEvents.Add(newEvent);

               if (usedFrom.Month == usedUntil.Month && usedFrom.Year == usedUntil.Year)
               {
                  duration = usedUntil - usedFrom;

                  if (!isOnDemand)
                  {
                     duration = duration.Add(new TimeSpan(24, 0, 0));
                  }

                  if (duration.TotalHours > 0)
                  {
                     newEvent = new UsageEvent();

                     newEvent.UsedFrom = usedFrom;
                     usedFrom = usedFrom.Add(duration);
                     newEvent.UsedUntill = usedFrom;

                     usageEvents.Add(newEvent);
                  }
               }
            }
         }
         else
         {
            usageEvents.Add(new UsageEvent(usedFrom, usedUntil));
         }

         return usageEvents;
      }
   }
}
