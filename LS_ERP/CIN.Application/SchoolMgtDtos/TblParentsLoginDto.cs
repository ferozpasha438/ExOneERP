using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SchoolMgtDto
{
    [AutoMap(typeof(TblParentsLogin))]
  public  class TblParentsLoginDto: AuditableActiveEntityDto<int>
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string RegisteredPhone { get; set; }
        [StringLength(50)]
        public string SchoolId { get; set; }
        [StringLength(50)]
        public string RegisteredEmail { get; set; }
        public string Password { get; set; }
        public DateTime InactiveOn { get; set; }
        public bool IsApprove { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RegistedDate { get; set; }
        public bool CurrentLogin { get; set; }
        public String WardNumber { get; set; }
    }


    public class CheckCINServerDto
    {
        [Required(ErrorMessage = "*")]
        public string CINNumber { get; set; }
    }

    public class CheckCINParentServerDto
    {
        [Required(ErrorMessage = "*")]
        public string CINNumber { get; set; }
    }

    public class CheckParentLoginMetaDataDto 
    {
        
        [Required(ErrorMessage = "*")]
        public string CINNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }

    public class ParentLoginMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public string CurrencyCode { get; set; }
        public string StartWeekDay { get; set; }
        public int NumberOfWeekDays { get; set; }
        public string WeekOff1 { get; set; }
        public string WeekOff2 { get; set; }
        public string PrivacyPolicy { get; set; }
        public string Website { get; set; }
        

    }

    public class ParentLoginSuccessMessageDto
    {
        public string Message { get; set; }
        public string Connectionstring { get; set; }
        public string BaseUrl { get; set; }
        public bool Status { get; set; }

    }

    //public class TeacherLoginSuccessMessageDto
    //{
    //    public string Message { get; set; }
    //    public string Connectionstring { get; set; }
    //    public string BaseUrl { get; set; }
    //    public bool Status { get; set; }

    //}

    public class CheckTeacherLoginMetaDataDto
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }

    public class ValidateCinMessageDto
    {
        public string Message { get; set; }
        public string Connectionstring { get; set; }
        public string AdmUrl { get; set; }
        public string FinUrl { get; set; }
        public string OpmUrl { get; set; }
        public string InvUrl { get; set; }
        public string SndUrl { get; set; }
        public string PopUrl { get; set; }
        public string HrmUrl { get; set; }
        public string HraUrl { get; set; }
        public string HrsUrl { get; set; }
        public string FltUrl { get; set; }
        public string SchUrl { get; set; }
        public string ScpUrl { get; set; }
        public string SctUrl { get; set; }
        public string PosUrl { get; set; }
        public string MfgUrl { get; set; }
        public string CrmUrl { get; set; }
        public string UtlUrl { get; set; }
        public bool Status { get; set; }
    }

    public class ValidateCinFailedMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }


    }

    public class ForgotPasswordDto 
    {
       public string Password { get; set; }
       public bool Status { get; set; }

    }

    public class OTPMobileDto
    {
        public int OTP { get; set; }
        public string Mobile { get; set; }
        public bool Status { get; set; }

    }

    public class TblWardDetailsDto : AuditableActiveEntityDto<int>
    {
        public int WardNumber { get; set; }
        public String Name { get; set; }
    }

    public class TblParentAddRequestDto
    {
        public int Id { get; set; }
        public string RegisteredMobile { get; set; }
        public string RegisteredEmail { get; set; }
        public string StuAdmNum { get; set; }
        public string Notes { get; set; }
    }


    public class UpdateNotificationSuccessDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }

    }


}
