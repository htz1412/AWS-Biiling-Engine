using BillingEngine.Parser.Models;
using BillingEngine.Parser;
using BillingEngine.Models;
using BillingEngine.Models.Billing;
using System.Collections.Generic;
using System.Linq;

namespace BillingEngine.Billing
{
   public class BillingService
   {
      private readonly CustomerCsvParser _customerCsvParser;
      private readonly InstanceTypeCsvParse _instanceTypeCsvParse;
      private readonly ResourceUsageRecordCsvParser _resourceUsageRecordCsvParser;
      private readonly RegionCsvParser _regionCsvParser;
      private readonly ElasticIPAllocationCsvParser _elasticIPAllocationCsvParser;
      private readonly ElasticIPAssociationCsvParser _elasticIPAssociationCsvParser;
      private readonly ElasticIPRatesCsvParser _elasticIPRateCsvParser;

      private readonly CustomerModelGenerator _customerModelGenerator;
      private readonly DiscountService _discountService;

      public BillingService()
      {
         _customerCsvParser = new();
         _instanceTypeCsvParse = new();
         _resourceUsageRecordCsvParser = new();
         _regionCsvParser = new();
         _elasticIPAllocationCsvParser = new();
         _elasticIPAssociationCsvParser = new();
         _elasticIPRateCsvParser = new();

         _customerModelGenerator = new();
         _discountService = new();
      }

      public List<MonthlyBill> GenerateMonthlyBills(
         string customerCsvPath,
         string instanceTypeCsvPath,
         string onDemandResourceUageRecordCsvPath,
         string reservedResourceUageRecordCsvPath,
         string regionCsvPath,
         string elasticIPAllocationCsvPath,
         string elasticIPAssociationCsvPath,
         string elasticIPRatesCsvPath
         )
      {
         // Parsing from csv file
         var parsedCustomerRecords = _customerCsvParser.Parse(customerCsvPath);
         var parsedInstanceTypeRecords = _instanceTypeCsvParse.Parse(instanceTypeCsvPath);
         var parsedResourceUsageRecords = _resourceUsageRecordCsvParser.Parse(onDemandResourceUageRecordCsvPath, true);
         parsedResourceUsageRecords.AddRange(_resourceUsageRecordCsvParser.Parse(reservedResourceUageRecordCsvPath, false));
         var parsedRegionRecords = _regionCsvParser.Parse(regionCsvPath);
         var parsedElasticIPAllocationRecords = _elasticIPAllocationCsvParser.Parse(elasticIPAllocationCsvPath);
         var parsedElasticIPAssociationRecords = _elasticIPAssociationCsvParser.Parse(elasticIPAssociationCsvPath);
         var parsedElasticIPRateRecords = _elasticIPRateCsvParser.Parse(elasticIPRatesCsvPath);

         var sortedUsageRecords = (from record in parsedResourceUsageRecords orderby record.UsedFrom.Year, record.UsedFrom.Month select record).ToList();

         var customers = _customerModelGenerator.GenerateCustomerModels(
            parsedCustomerRecords, 
            parsedInstanceTypeRecords, 
            sortedUsageRecords, 
            parsedRegionRecords,
            parsedElasticIPAllocationRecords,
            parsedElasticIPAssociationRecords,
            parsedElasticIPRateRecords
            );

         return customers.SelectMany(GenerateMonthlyBillForCustomer).ToList();
      }

      public List<MonthlyBill> GenerateMonthlyBillForCustomer(Customer customer)
      {
         var distinctMonthYear = customer.GetDistinctMonthYear();

         return distinctMonthYear.Select(monthYear => GenerateBillForMonth(customer, monthYear)).ToList();
      }

      public MonthlyBill GenerateBillForMonth(Customer customer, MonthYear monthYear)
      {
         var monthlyBill = new MonthlyBill(customer.CustomerId, customer.CustomerName, monthYear);

         monthlyBill.AddMonthlyEc2UsageRecords(customer.GetUsageRecordsInMonth(monthYear));

         _discountService.ApplyDiscounts(customer, monthlyBill);

         return monthlyBill;
      }
   }
}