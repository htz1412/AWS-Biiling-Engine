using System;

namespace BillingEngine.Parser
{
   public class ParsedResourceUsageRecord
   {
      public string CustomerId { get; set; }
      public string InstanceId { get; set; }
      public string InstanceType { get; set; }
      public DateTime UsedFrom { get; set; }
      public DateTime UsedUntil { get; set; }
      public string Region { get; set; }
      public string Os { get; set; }
      public bool IsOnDemand { get; set; }
   }
}
