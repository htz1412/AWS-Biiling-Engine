using BillingEngine.Models.ElasticIPService;
using System.Collections.Generic;
using BillingEngine.Parser.Models;
using System.Linq;

namespace BillingEngine.DomainModelGenerators
{
   public class ElasticIPModelGenerator
   {
      public List<ElasticIP> GenerateElasticIPRecords(
         string customerId,
         List<Ec2Instance> ec2Instances,
         List<ParsedElasticIpAllocationRecord> parsedElasticIpAllocationRecords,
         List<ParsedElasticIPAssociationRecord> parsedElasticIPAssociationRecords,
         List<ParsedElasticIPRateRecord> parsedElasticIPRateRecords
         )
      {
         var elasticIPList = new List<ElasticIP>();

         var parsedElasticIpAllocationRecordsForCustomer = parsedElasticIpAllocationRecords.FindAll(record => record.CustomerId == customerId);

         parsedElasticIpAllocationRecordsForCustomer.ForEach(record =>
         {
            if (!elasticIPList.Exists(elasticIP => elasticIP.IPAddress == record.IPAddress && elasticIP.Region == record.Region))
            {
               elasticIPList.Add(new ElasticIP
                  (
                     record.IPAddress,
                     record.Region,
                     new List<UsageEvent>() { new UsageEvent(record.UsedFrom, record.UsedUntil) },
                     record.IsYourOwnIP,
                     parsedElasticIPRateRecords.Find(rateRecord => rateRecord.Region == record.Region).RatePerHour,
                     GetRecordsAssociatedWithIP(record, ec2Instances,parsedElasticIPAssociationRecords.FindAll(associationRecord => associationRecord.IPAddress == record.IPAddress))
                  )
               );
            }
            else
            {
               var elasticIP = elasticIPList.Find(elasticIP => elasticIP.IPAddress == record.IPAddress && elasticIP.Region == record.Region);
               elasticIP.ElasticIPAllocationEvents.Add(new UsageEvent(record.UsedFrom, record.UsedUntil));
               elasticIP.ElasticIPAssociations.AddRange(GetRecordsAssociatedWithIP(record, ec2Instances, parsedElasticIPAssociationRecords.FindAll(associationRecord => associationRecord.IPAddress == record.IPAddress)));
            }
         });

         return elasticIPList;
      }

      private List<ElasticIPAssociation> GetRecordsAssociatedWithIP(
         ParsedElasticIpAllocationRecord parsedElasticIpAllocationRecord,
         List<Ec2Instance> ec2Instances,
         List<ParsedElasticIPAssociationRecord> parsedElasticIPAssociationRecords
         )
      {
         var associationRecords = new List<ElasticIPAssociation>();

         parsedElasticIPAssociationRecords.ForEach(record =>
         {
            if (IsInstanceRunning(record.InstanceId, ec2Instances) && record.AssociatedFrom >= parsedElasticIpAllocationRecord.UsedFrom && record.AssociatedUntil <= parsedElasticIpAllocationRecord.UsedUntil)
            {
               associationRecords.Add(
                  new ElasticIPAssociation(
                     record.InstanceId,
                     new List<UsageEvent> {
                        new UsageEvent(record.AssociatedFrom,record.AssociatedUntil)
                     }
                  )
               );
            }
         });

         return associationRecords;
      }

      private bool IsInstanceRunning(string ec2InstanceId ,List<Ec2Instance> ec2Instances)
      {
         return ec2Instances.Exists(instance => instance.InstanceId == ec2InstanceId);
      }
   }
}
