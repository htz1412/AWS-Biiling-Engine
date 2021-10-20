using System.Collections.Generic;

namespace BillingEngine.Models.ElasticIPService
{
   public class ElasticIP
   {
      public string IPAddress { get;set;}
      public string Region { get; set; }
      public List<UsageEvent> ElasticIPAllocationEvents { get; set; }
      public bool IsYourOwnIP { get; set; }
      public double RatePerHour { get; set; }
      public List<ElasticIPAssociation> ElasticIPAssociations { get; set; }

      public ElasticIP()
      {
         ElasticIPAllocationEvents = new();
         ElasticIPAssociations = new();
      }

      public ElasticIP(string ipAddress, string region, List<UsageEvent> elasticIPAllocationEvents, bool isYourOwnIP, double ratePerHour, List<ElasticIPAssociation> elasticIPAssociations)
      {
         IPAddress = ipAddress;
         Region = region;
         ElasticIPAllocationEvents = elasticIPAllocationEvents;
         IsYourOwnIP = isYourOwnIP;
         RatePerHour = ratePerHour;
         ElasticIPAssociations = elasticIPAssociations;
      }
   }
}
