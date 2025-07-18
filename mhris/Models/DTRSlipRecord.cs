using System;
using System.Collections.Generic;
using System.Text;

namespace mhris.Models
{
    public class DTRSlipRecord
    {
        public int DTRId {get; set;}
        public int PayrollOtherDeductionId {get; set;}
        public string DTRNumber {get; set;}
        public DateTime DateStart {get; set;}
        public DateTime DateEnd {get; set;}
        public string Remarks {get; set;}
        public string Department {get; set;}
        public string Position {get; set;}
        public int EmployeeId {get; set;}
        public string FullName {get; set;}
        public string Date {get; set;}
        public string TimeIn1 {get; set;}
        public string TimeOut1 {get; set;}
        public string TimeIn2 {get; set;}
        public string TimeOut2 {get; set;}
        public bool OfficialBusiness {get; set;}
        public bool OnLeave {get; set;}
        public bool Absent {get; set;}
        public decimal RegularHours {get; set;}
        public decimal NightHours {get; set;}
        public decimal OvertimeHours {get; set;}
        public decimal OvertimeNightHours {get; set;}
        public decimal GrossTotalHours {get; set;}
        public decimal TardyLateHours {get; set;}
        public decimal TardyUndertimeHours {get; set;}
        public decimal NetTotalHours {get; set;}
        public int DayTypeId {get; set;}
        public string DayType {get; set;}
        public bool RestDay { get; set; }
    }
}
