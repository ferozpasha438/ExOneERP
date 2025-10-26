
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using ExcelDataReader;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CIN.DB;
using System.Collections;

namespace LS.API.SM.Controllers.ExcelExport
{
    public class ExcelImportController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly CINDBOneContext _context;
        public ExcelImportController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env, CINDBOneContext context) : base(appSettings)
        {
            _env = env;
            _context = context;
        }


        #region Excel Importing Data

        [HttpPost("importWebStudentRegistration")]
        public async Task<ActionResult> ImportExcelFomAssetMaster(IFormFile file)
        {
            AppCtrollerDto result = new();

            if (file == null || file.Length == 0)
                return BadRequest(result.Message = "FileNotSelected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return BadRequest(new ApiMessageDto
                {
                    Message = "Invalid File"
                });

            var guid = $"{Guid.NewGuid().ToString()}#";
            guid = $"{guid}_{fileExtension}";
            var webRoot = $"{_env.ContentRootPath}/Signaturefiles";

            //var webRoot = $"{_env.ContentRootPath}/files/uploadedastmaster";
            //var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmssfff", CultureInfo.InvariantCulture)}{Path.GetExtension(file.FileName)}";

            //Create directory if not exists.
            if (!Directory.Exists(webRoot))
                Directory.CreateDirectory(webRoot);

            var filePath = Path.Combine(webRoot, guid);
            //var filePath = Path.Combine(webRoot, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                if (fileExtension.ToUpper() == ".XLSX")
                {
                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                    {
                        //ExcelWorksheet workSheet = package.Workbook.Worksheets["Table1"];
                        var workSheet = package.Workbook.Worksheets.First();
                        int totalRows = workSheet.Dimension.Rows;

                        var assetMasterList = new List<TblWebStudentRegistrationDto>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            if (workSheet.Cells[i, 1].Value != null || workSheet.Cells[i, 2].Value != null)
                            {
                                var asstMasterDto = new TblWebStudentRegistrationDto();
                                asstMasterDto.RegNum = Convert.ToString(Convert.ToString(workSheet.Cells[i, 1].Value));
                                asstMasterDto.RegDate = Convert.ToDateTime(Convert.ToString(workSheet.Cells[i, 2].Value));
                                asstMasterDto.FullName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 3].Value));
                                asstMasterDto.NameAr = Convert.ToString(Convert.ToString(workSheet.Cells[i, 4].Value));
                                asstMasterDto.DateOfBirth = Convert.ToString(Convert.ToString(workSheet.Cells[i, 5].Value));
                                asstMasterDto.Age = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 6].Value));
                                asstMasterDto.Grade = Convert.ToString(Convert.ToString(workSheet.Cells[i, 7].Value));
                                asstMasterDto.LangCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 8].Value));
                                asstMasterDto.GenderName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 9].Value));
                                asstMasterDto.IDNumber = Convert.ToString(Convert.ToString(workSheet.Cells[i, 10].Value));
                                asstMasterDto.Nationality = Convert.ToString(Convert.ToString(workSheet.Cells[i, 11].Value));
                                asstMasterDto.ReligionCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 12].Value));
                                asstMasterDto.FatherName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 13].Value));
                                asstMasterDto.MotherName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 14].Value));
                                asstMasterDto.FatherPhoneNumber = Convert.ToString(Convert.ToString(workSheet.Cells[i, 15].Value));
                                asstMasterDto.FatherEmail = Convert.ToString(Convert.ToString(workSheet.Cells[i, 16].Value));
                                asstMasterDto.City = Convert.ToString(Convert.ToString(workSheet.Cells[i, 17].Value));
                                asstMasterDto.IsActive = Convert.ToString(workSheet.Cells[i, 18].Value).ToLower() == "1" ? true : false;
                                assetMasterList.Add(asstMasterDto);
                            }
                        }

                        if (assetMasterList.Count() > 0)
                            result = await Mediator.Send(new ImportExcelStudentRegistration() { Input = assetMasterList });
                        //result = await Mediator.Send(new BulkImportExcelFomAssetMaster() { Input = assetMasterList, User = UserInfo() });

                        FileInfo fi = new FileInfo(filePath);
                        fi.Delete();

                        if (result.Id == assetMasterList.Count())
                        {
                            result.Message = "Successfully imported data";
                            return Ok(result);
                        }
                        else if (result.Id > 0 && result.Id < assetMasterList.Count())
                        {
                            result.Message = "Partially Successful";
                            return Ok(result);
                        }
                        else
                            return BadRequest(new ApiMessageDto
                            {
                                Message = result.Message
                            });
                    }

                }

                ////else if (fileExtension.ToUpper() == ".XLS")
                ////{
                ////    static List<T> DataTableToList<T>(DataTable table) where T : new()
                ////    {
                ////        List<T> list = new List<T>();

                ////        foreach (DataRow row in table.Rows)
                ////        {
                ////            T obj = new T();

                ////            foreach (var prop in typeof(T).GetProperties())
                ////            {
                ////                if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                ////                {
                ////                    prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                ////                }
                ////            }

                ////            list.Add(obj);
                ////        }

                ////        return list;
                ////    }

                ////    List<TblWebStudentRegistrationDto> assetMasterList = null;
                ////    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                ////    {
                ////        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                ////        using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration()
                ////        {
                ////            FallbackEncoding = System.Text.Encoding.GetEncoding(1252)
                ////        }))
                ////        {
                ////            var conf = new ExcelDataSetConfiguration
                ////            {
                ////                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                ////                {
                ////                    UseHeaderRow = true
                ////                }
                ////            };

                ////            var dataSet = reader.AsDataSet(conf);
                ////            var dataTable = dataSet.Tables[0];
                ////            assetMasterList = DataTableToList<TblWebStudentRegistrationDto>(dataTable);

                ////        }
                ////    }

                ////    if (assetMasterList is not null && assetMasterList.Count() > 0)
                ////    {
                ////        assetMasterList.ForEach(item =>
                ////        {

                ////            item.Location = item.ProjectLocation;
                ////            item.ContractCode = item.Project;
                ////            item.IsActive = item.Status.ToLower() == "active" ? true : false;

                ////        });

                ////        if (assetMasterList.Count() > 0)
                ////            result = await Mediator.Send(new ImportExcelFomAssetMaster() { Input = assetMasterList, User = UserInfo() });

                ////        FileInfo fi = new FileInfo(filePath);
                ////        fi.Delete();

                ////        if (result.Id == assetMasterList.Count())
                ////        {
                ////            result.Message = "Successfully imported data";
                ////            return Ok(result);
                ////        }
                ////        else if (result.Id > 0 && result.Id < assetMasterList.Count())
                ////        {
                ////            result.Message = "Partially Successful";
                ////            return Ok(result);
                ////        }
                ////        else
                ////            return BadRequest(new ApiMessageDto
                ////            {
                ////                Message = result.Message
                ////            });
                ////    }
                ////}

                return BadRequest(new ApiMessageDto
                {
                    Message = "Invalid File"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiMessageDto
                {
                    Message = "Can not Process File"
                });
            }

        }



        [HttpPost("importStudentMasters")]
        public async Task<ActionResult> ImportStudentMasters([FromQuery] string pathLocation, IFormFile file)
        {
            AppCtrollerDto result = new();

            if (file == null || file.Length == 0)
                return BadRequest(result.Message = "FileNotSelected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return BadRequest(new ApiMessageDto
                {
                    Message = "Invalid File"
                });

            var guid = $"{Guid.NewGuid().ToString()}#";
            guid = $"{guid}_{fileExtension}";
            var webRoot = $"{_env.ContentRootPath}/Signaturefiles";

            //var webRoot = $"{_env.ContentRootPath}/files/uploadedastmaster";
            //var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmssfff", CultureInfo.InvariantCulture)}{Path.GetExtension(file.FileName)}";

            //Create directory if not exists.
            if (!Directory.Exists(webRoot))
                Directory.CreateDirectory(webRoot);

            var filePath = Path.Combine(webRoot, guid);
            //var filePath = Path.Combine(webRoot, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                if (fileExtension.ToUpper() == ".XLSX")
                {
                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                    {
                        //ExcelWorksheet workSheet = package.Workbook.Worksheets["Table1"];
                        var workSheet = package.Workbook.Worksheets.First();
                        int totalRows = workSheet.Dimension.Rows;

                        var studentMasters = new List<AllStudentMasterDataDto>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            if (workSheet.Cells[i, 1].Value != null || workSheet.Cells[i, 2].Value != null)
                            {
                                var stdMst = new AllStudentMasterDataDto();
                                stdMst.StudentImageFileName = $"{pathLocation}default_thumb.jpg";
                                stdMst.FatherSignatureFileName = $"{pathLocation}default_thumb.jpg";
                                stdMst.MotherSignatureFileName = $"{pathLocation}default_thumb.jpg";

                                //stdMst.StuAdmNum = Convert.ToString(Convert.ToString(workSheet.Cells[i, 1].Value));
                                stdMst.StuAdmDate = Convert.ToDateTime(Convert.ToString(workSheet.Cells[i, 1].Value));
                                stdMst.StuName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 2].Value));
                                stdMst.StuName2 = Convert.ToString(Convert.ToString(workSheet.Cells[i, 3].Value));
                                stdMst.DateofBirth = Convert.ToDateTime(Convert.ToString(workSheet.Cells[i, 4].Value));
                                stdMst.Alias = Convert.ToString(Convert.ToString(workSheet.Cells[i, 5].Value));
                                stdMst.GenderCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 6].Value));
                                stdMst.Age = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 7].Value));
                                stdMst.BranchCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 8].Value));
                                stdMst.GradeCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 9].Value));
                                stdMst.PTGroupCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 10].Value)).Trim();
                                stdMst.GradeSectionCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 11].Value));
                                stdMst.LangCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 12].Value)).Trim();
                                stdMst.NatCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 13].Value)).Trim();
                                stdMst.ReligionCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 14].Value)).Trim();
                                stdMst.StuIDNumber = Convert.ToString(Convert.ToString(workSheet.Cells[i, 15].Value));
                                stdMst.IDNumber = Convert.ToString(Convert.ToString(workSheet.Cells[i, 16].Value));
                                stdMst.MotherToungue = Convert.ToString(Convert.ToString(workSheet.Cells[i, 17].Value)).Trim();
                                stdMst.RegisteredPhone = Convert.ToString(Convert.ToString(workSheet.Cells[i, 18].Value));
                                stdMst.RegisteredEmail = Convert.ToString(Convert.ToString(workSheet.Cells[i, 19].Value));                                                        
                                stdMst.FeeStructCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 20].Value));
                                //stdMst.TaxApplicable = Convert.ToBoolean(Convert.ToString(workSheet.Cells[i, 21].Value));
                                stdMst.TaxApplicable = Convert.ToString(workSheet.Cells[i, 21].Value).ToLower() == "1" ? true : false;
                                stdMst.TotFeeAmount = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 22].Value));
                                stdMst.PaidFees = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 23].Value));
                                stdMst.NetFeeAmount = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 24].Value));
                                //stdMst.TransportationRequired = Convert.ToBoolean(Convert.ToString(workSheet.Cells[i, 25].Value));
                                //stdMst.TransportationRequired = Convert.ToString(workSheet.Cells[i, 25].Value).ToLower() == "1" ? true : false;
                                //stdMst.PickNDropZone = Convert.ToString(Convert.ToString(workSheet.Cells[i, 26].Value));
                                //stdMst.TransportationFee = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 27].Value));
                                //stdMst.VehicleTransport = Convert.ToString(Convert.ToString(workSheet.Cells[i, 28].Value));

                                stdMst.BuildingName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 25].Value));
                                stdMst.PAddress1 = Convert.ToString(Convert.ToString(workSheet.Cells[i, 26].Value));
                                stdMst.City = Convert.ToString(Convert.ToString(workSheet.Cells[i, 27].Value));
                                stdMst.Phone = Convert.ToString(Convert.ToString(workSheet.Cells[i, 28].Value));
                                stdMst.ZipCode = Convert.ToString(Convert.ToString(workSheet.Cells[i, 29].Value));
                                stdMst.Mobile = Convert.ToString(Convert.ToString(workSheet.Cells[i, 30].Value));
                                stdMst.FatherName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 31].Value));
                                stdMst.FatherMobile = Convert.ToString(Convert.ToString(workSheet.Cells[i, 32].Value));
                                stdMst.FatherEmail = Convert.ToString(Convert.ToString(workSheet.Cells[i, 33].Value));
                                stdMst.FatherOccupation = Convert.ToString(Convert.ToString(workSheet.Cells[i, 34].Value));
                                stdMst.FatherDesignation = Convert.ToString(Convert.ToString(workSheet.Cells[i, 35].Value));
                                //asstMasterDto.fatherSignatureFileName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 41].Value));
                                //asstMasterDto.FatherSignature = Convert.ToString(Convert.ToString(workSheet.Cells[i, 42].Value));
                                stdMst.MotherName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 36].Value));
                                stdMst.MotherMobile = Convert.ToString(Convert.ToString(workSheet.Cells[i, 37].Value));
                                stdMst.MotherEmail = Convert.ToString(Convert.ToString(workSheet.Cells[i, 38].Value));
                                stdMst.MotherOccupation = Convert.ToString(Convert.ToString(workSheet.Cells[i, 39].Value));
                                stdMst.MotherDesignation = Convert.ToString(Convert.ToString(workSheet.Cells[i, 40].Value));
                                //asstMasterDto.motherSignatureFileName = Convert.ToString(Convert.ToString(workSheet.Cells[i, 17].Value));
                                //asstMasterDto.MotherSignature = Convert.ToString(Convert.ToString(workSheet.Cells[i, 17].Value));
                                stdMst.BloodGroup = Convert.ToString(Convert.ToString(workSheet.Cells[i, 41].Value));
                                stdMst.Height = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 42].Value));
                                stdMst.Weight = Convert.ToDecimal(Convert.ToString(workSheet.Cells[i, 43].Value));
                                //stdMst.SpecialAssistance = Convert.ToBoolean(Convert.ToString(workSheet.Cells[i, 48].Value));
                                stdMst.SpecialAssistance = Convert.ToString(workSheet.Cells[i, 44].Value).ToLower() == "1" ? true : false;
                                stdMst.SpecialAssistanceNotes = Convert.ToString(Convert.ToString(workSheet.Cells[i, 45].Value));
                                //stdMst.PhysicalDisability = Convert.ToBoolean(Convert.ToString(workSheet.Cells[i, 50].Value));
                                stdMst.PhysicalDisability = Convert.ToString(workSheet.Cells[i, 46].Value).ToLower() == "1" ? true : false;
                                stdMst.PhysicalDisabilityNotes = Convert.ToString(Convert.ToString(workSheet.Cells[i, 47].Value));
                                //stdMst.IDNumber = Convert.ToString(Convert.ToString(workSheet.Cells[i, 53].Value));
                                stdMst.IsActive = Convert.ToString(workSheet.Cells[i, 48].Value).ToLower() == "1" ? true : false;

                                //stdMst.AcademicsScale = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 55].Value));
                                //stdMst.AttentivenessScale = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 56].Value));
                                //stdMst.HomeWorkScale = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 57].Value));
                                //stdMst.ProjectWorkScale = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 58].Value));
                                //stdMst.SportsPhysicalScale = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 59].Value));
                                //stdMst.DiciplineAttitude = Convert.ToInt32(Convert.ToString(workSheet.Cells[i, 60].Value));

                                studentMasters.Add(stdMst);
                            }
                        }

                        if (studentMasters.Count() > 0)
                        {
                            //Check for duplicates
                            var bloodGroups = new List<string>() { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
                            var wrongLanguageCodes = studentMasters.Select(e => e.LangCode.Trim()).Except(_context.SysSchoolLanguages.Select(e => e.LangCode).ToArray().Select(e => e.Trim()));
                            if (wrongLanguageCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong LanguageCodes:  {string.Join(", ", wrongLanguageCodes)}" });

                            var wrongCitys = studentMasters.Select(e => e.City).Except(_context.CityCodes.Select(e => e.CityCode).ToArray());
                            if (wrongCitys.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong CityCodes:  {string.Join(", ", wrongCitys)}" });

                            var wrongMotherToungues = studentMasters.Select(e => e.MotherToungue.Trim()).Except(_context.SysSchoolLanguages.Select(e => e.LangCode).ToArray().Select(e => e.Trim()));
                            if (wrongMotherToungues.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong MotherToungues:  {string.Join(", ", wrongMotherToungues)}" });

                            var wrongNatCodes = studentMasters.Select(e => e.NatCode.Trim()).Except(_context.SysSchoolNationality.Select(e => e.NatCode).ToArray().Select(e => e.Trim()));
                            if (wrongNatCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong NationalityCodes:  {string.Join(", ", wrongNatCodes)}" });

                            var wrongReligionCodes = studentMasters.Select(e => e.ReligionCode.Trim()).Except(_context.SysSchoolReligion.Select(e => e.RegCode).ToArray().Select(e => e.Trim()));
                            if (wrongReligionCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong ReligionCodes:  {string.Join(", ", wrongReligionCodes)}" });

                            var wrongPETCategorys = studentMasters.Select(e => e.PTGroupCode.Trim()).Except(_context.SysSchoolPETCategory.Select(e => e.PETCode).ToArray().Select(e => e.Trim()));
                            if (wrongPETCategorys.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong PhysicalTrainingCategories:  {string.Join(", ", wrongPETCategorys)}" });

                            var wrongbloodGroupss = studentMasters.Select(e => e.BloodGroup).Except(bloodGroups.Select(e => e).ToArray());
                            if (wrongbloodGroupss.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong BloodGroup, must be [ {string.Join(",  ", bloodGroups)} ]" });

                            var wrongBranchCodes = studentMasters.Select(e => e.BranchCode).Except(_context.SchoolBranches.Select(e => e.BranchCode).ToArray());
                            if (wrongBranchCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong BranchCodes:  {string.Join(", ", wrongBranchCodes)}" });

                            var wrongGradeCodes = studentMasters.Select(e => e.GradeCode).Except(_context.SchoolAcedemicClassGrade.Select(e => e.GradeCode).ToArray());
                            if (wrongGradeCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong GradeCodes:  {string.Join(", ", wrongGradeCodes)}" });

                            var wrongGradeSectionCodes = studentMasters.Select(e => e.GradeSectionCode).Except(_context.SchoolGradeSectionMapping.Select(e => e.SectionCode).ToArray());
                            if (wrongGradeSectionCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong GradeSectionCodes:  {string.Join(", ", wrongGradeSectionCodes)}" });


                            var wrongFeeStructCodes = studentMasters.Select(e => e.FeeStructCode).Except(_context.SchoolFeeStructureDetails.Select(e => e.FeeStructCode).ToArray());
                            if (wrongFeeStructCodes.Count() > 0)
                                return BadRequest(new ApiMessageDto { Message = $"Wrong FeeStructCodes:  {string.Join(", ", wrongFeeStructCodes)}" });

                            foreach (var dTO in studentMasters)
                            {
                                int id = await Mediator.Send(new AllSchoolStudentMasterData() { AllStudentMasterDataDto = dTO, User = UserInfo() });
                                if (id > 0)
                                    result.Id++;
                            }
                        }

                        //result = await Mediator.Send(new ImportStudentMasters() { Input = assetMasterList });

                        FileInfo fi = new FileInfo(filePath);
                        fi.Delete();

                        if (result.Id == studentMasters.Count())
                        {
                            result.Message = "Successfully imported data";
                            return Ok(result);
                        }
                        else if (result.Id > 0 && result.Id < studentMasters.Count())
                        {
                            result.Message = "Partially Successful";
                            return Ok(result);
                        }
                        else
                            return BadRequest(ApiMessageInfo.Status(0));
                    }

                }

                return BadRequest(new ApiMessageDto
                {
                    Message = "Invalid File"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiMessageDto
                {
                    Message = "Can not Process File"
                });
            }

        }

        #endregion
    }
}
