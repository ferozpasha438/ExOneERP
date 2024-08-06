using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    public class EditableSurveyFormDataDto
    {
        public TblSndDefServiceEnquiriesDto Enquiry { get; set; }
        public int  EnquiryID { get; set; }
        public TblSndDefCustomerMasterDto Customer { get; set; }
        public TblSndDefServiceEnquiryHeaderDto EnquiryHeader { get; set; }
        public TblSndDefSiteMasterDto Site { get; set; }
        public TblSndDefSurveyFormHeadDto SurveyFormHeader { get; set; }
        public List<TblSndDefSurveyFormDataEntryDto> SurveyFormDataEntries { get; set; }
        public TblSndDefSurveyorDto Surveyor { get; set; }
        public TblSndDefServiceMasterDto Service { get; set; }
        public string  Action { get; set; }
    }
}
