using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(HRM_DEF_EmployeeShift))]
    public class HRM_DEF_EmployeeShiftDto
    {
        public int ID { get; set; }
        public long EmployeeID { get; set; }
        public short MondayShiftId { get; set; }
        public short TuesdayShiftId { get; set; }
        public short WednesdayShiftId { get; set; }
        public short ThursdayShiftId { get; set; }
        public short FridayShiftId { get; set; }
        public short SaturdayShiftId { get; set; }
        public short SundayShiftId { get; set; }
    }
}
