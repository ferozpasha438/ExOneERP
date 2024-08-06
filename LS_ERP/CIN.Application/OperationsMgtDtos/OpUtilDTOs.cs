using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
   
    public class RefreshOffsDto 
    {

        //public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string EmployeeNumber { get; set; }
    }



     public class UpdateShiftCodeForDayDto 
    {

        public int Day { get; set; }        //between 1-31
        public long RoasterId { get; set; }
        public string ShiftCode { get; set; }
    }



     public class InputCustomerProjectSite
    {

        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string CustomerCode { get; set; }
        public bool IsAdendum { get; set; } = false;
    }


    public class CreateUpadteResultDto
    {
        public bool IsSuccess { get; set; }
        public short ErrorId { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class ValidityCheckDto
    {
        public bool IsValidReq { get; set; }
        public int ErrorId { get; set; }             //error id
        public string ErrorMsg { get; set; }         //erro msg

    }

    public class CreateUpdateResultDto
    {
        public bool IsSuccess { get; set; }
        public int ErrorId { get; set; }             //error id
        public string ErrorMsg { get; set; }

    }

    public class UtilDto
    {
        public int? Id { get; set; }
    }

    public class InpuEmployeeSkillsetDto
    {

        public string EmployeeNumber { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? Date { get; set; }
    }
    public class InputEmployeeSingleRoasterDto
    {

        public string EmployeeNumber { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public DateTime? Date { get; set; }
    }


}