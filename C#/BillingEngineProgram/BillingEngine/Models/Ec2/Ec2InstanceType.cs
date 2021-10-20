namespace BillingEngine.Models.Ec2
{
   public class Ec2InstanceType
   {
      public string InstanceType { get; set; }
      public string Region { get; set; }
      public string Os { get; set; }
      public double RatePerHour { get; set; }
      public bool IsOnDemand { get; set; }
      public bool IsFreeTierEligible { get; set; }

      public Ec2InstanceType(string instanceType, double ratePerHour,string region,string os,bool isOnDemand,bool isFreeTierEligible)
      {
         InstanceType = instanceType;
         RatePerHour = ratePerHour;
         Region = region;
         Os = os;
         IsOnDemand = isOnDemand;
         IsFreeTierEligible = isFreeTierEligible;
      }
   }
}
