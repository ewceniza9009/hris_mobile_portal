using System;
using System.Collections.Generic;
using System.Text;

namespace mhris.Models
{
    public class PaySlipRecord
    {
        public bool IsLocked { get; set; }
        public int PayrollId { get; set; }
        public int PayrollOtherDeductionId { get; set; }
        public string PayrollNumber { get; set; }
        public DateTime PayrollDate { get; set; }
        public string Remarks { get; set; }
        public string Company { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OtherSalary { get; set; }
        public decimal TotalSalaryAmount { get; set; }
        public decimal TotalTardyAmount { get; set; }
        public decimal TotalAbsentAmount { get; set; }
        public decimal TotalNetSalaryAmount { get; set; }
        public decimal TotalLegalHolidayWorkingAmount { get; set; }
        public decimal TotalSpecialHolidayWorkingAmount { get; set; }
        public decimal TotalRegularNightAmount { get; set; }
        public decimal TotalRegularOvertimeAmount { get; set; }
        public decimal ComputedRegularRestdayAmount { get; set; }
        public decimal TotalOtherIncomeTaxable { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal TotalOtherIncomeNonTaxable { get; set; }
        public decimal GrossIncomeWithNonTaxable { get; set; }
        public decimal SSSContribution { get; set; }
        public decimal PHICContribution { get; set; }
        public decimal HDMFContribution { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal TotalOtherDeduction { get; set; }
        public string OtherDeductionBreakdown { get; set; }
        public decimal NetIncome { get; set; }
        public int PreparedBy { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal? LeaveBalance { get; set; }
        public decimal? LoanBalance { get; set; }
    }
}
