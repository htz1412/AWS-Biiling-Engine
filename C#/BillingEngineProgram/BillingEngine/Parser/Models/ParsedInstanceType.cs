namespace BillingEngine.Parser.Models
{
   public class ParsedInstanceType
   {
      public string InstanceType { get; set; }
      public double RatePerHour { get; set; }
      public double OnDemandCharge { get; set; }
      public double ReservedCharge { get; set; }
      public string Region { get; set; }
   }
}
