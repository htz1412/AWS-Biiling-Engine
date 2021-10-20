using System;

namespace BillingEngine.Parser.Models
{
   public class ParsedElasticIpAllocationRecord
   {
      public string CustomerId { get; set; }
      public string Region { get; set; }
      public string IPAddress { get; set; }
      public DateTime UsedFrom { get; set; }
      public DateTime UsedUntil { get; set; }
      public bool IsYourOwnIP { get; set; }
   }
}
