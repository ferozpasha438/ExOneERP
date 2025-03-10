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

namespace LS.API.SM.Controllers.ExcelExport
{
    public class ExcelExportController : ApiControllerBase
    {
        public ExcelExportController(IOptions<AppSettingsJson> appSettings)// : base(appSettings)
        {
        }

        [HttpGet("exportStudentRegistrations")]
        public async Task<IActionResult> ExportStudentRegistrations([FromQuery] string action, [FromQuery] PaginationFilterDto filter)
        {
            if (action.HasValue())
            {
                int columnIndex = 1;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("excel_download");
                    string[] columNames = null;
                    //MemoryStream stream = null;

                    if (action == "stdreg")
                    {
                        columNames = new string[] { "Registration Number", "Name", "Name Ar", "Grade", "Phone", "Email", "City" };
                        filter.Page = 0;
                        filter.PageCount = 1000000;
                        var items = await Mediator.Send(new GetWebStudentRegistrationList() { Input = filter, User = null });

                        // Add student data
                        int n = items.Items.Count;
                        Parallel.For(0, n, i =>
                        {
                            worksheet.Cells[i + 2, 1].Value = items.Items[i].RegNum;
                            worksheet.Cells[i + 2, 2].Value = items.Items[i].FullName;
                            worksheet.Cells[i + 2, 3].Value = items.Items[i].NameAr;
                            worksheet.Cells[i + 2, 4].Value = items.Items[i].Grade;
                            worksheet.Cells[i + 2, 5].Value = items.Items[i].FatherPhoneNumber;
                            worksheet.Cells[i + 2, 6].Value = items.Items[i].FatherEmail;
                            worksheet.Cells[i + 2, 7].Value = items.Items[i].City;
                        });

                        //for (int i = 0; i < items.Items.Count; i++)
                        //{
                        //    worksheet.Cells[i + 2, 1].Value = items.Items[i].RegNum;
                        //    worksheet.Cells[i + 2, 2].Value = items.Items[i].FullName;
                        //    worksheet.Cells[i + 2, 3].Value = items.Items[i].NameAr;
                        //    worksheet.Cells[i + 2, 4].Value = items.Items[i].Grade;
                        //    worksheet.Cells[i + 2, 5].Value = items.Items[i].FatherPhoneNumber;
                        //    worksheet.Cells[i + 2, 6].Value = items.Items[i].FatherEmail;
                        //    worksheet.Cells[i + 2, 7].Value = items.Items[i].City;
                        //}

                        //var fileName = "student registration list";
                        //var stream = await GenerateExcel(fileName, columNames, list.Items);
                        //return Ok(stream);

                        //fileName = $"{fileName}.xlsx";
                        //return Ok(File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
                    }
                    else if (action == "stdmst")
                    {
                        //string excelFileName = fileName;
                        columNames = new string[] { "Admission Number", "Name", "Name Ar", "Grade", "Section", "Nationality" };
                        filter.Page = 0;
                        filter.PageCount = 1000000;
                        var items = await Mediator.Send(new GetSchoolStudentManagementList() { Input = filter, User = null });
                        // Add student data
                        int n = items.Items.Count;
                        Parallel.For(0, n, i =>
                        {
                            worksheet.Cells[i + 2, 1].Value = items.Items[i].StuAdmNum;
                            worksheet.Cells[i + 2, 2].Value = items.Items[i].StuName;
                            worksheet.Cells[i + 2, 3].Value = items.Items[i].StuName2;
                            worksheet.Cells[i + 2, 4].Value = items.Items[i].GradeCode;
                            worksheet.Cells[i + 2, 5].Value = items.Items[i].GradeSectionCode;
                            worksheet.Cells[i + 2, 6].Value = items.Items[i].NatCode;
                        });

                        //for (int i = 0; i < items.Items.Count; i++)
                        //{
                        //    worksheet.Cells[i + 2, 1].Value = items.Items[i].StuAdmNum;
                        //    worksheet.Cells[i + 2, 2].Value = items.Items[i].StuName;
                        //    worksheet.Cells[i + 2, 3].Value = items.Items[i].StuName2;
                        //    worksheet.Cells[i + 2, 4].Value = items.Items[i].GradeCode;
                        //    worksheet.Cells[i + 2, 5].Value = items.Items[i].GradeSectionCode;
                        //    worksheet.Cells[i + 2, 6].Value = items.Items[i].NatCode;
                        //}

                    }
                    // Add headers
                    foreach (var column in columNames)
                    {
                        worksheet.Cells[1, columnIndex].Value = column;
                        columnIndex++;
                    }

                    // Style headers
                    using (var range = worksheet.Cells[1, 1, 1, columNames.Count()])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Adjust column widths
                    //worksheet.Column(1).Width = 10;
                    //worksheet.Column(2).Width = 20;
                    //worksheet.Column(3).Width = 30;
                    //worksheet.Column(4).Width = 15;

                    // Export as Excel file
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;
                    return Ok(stream);
                }

                //if (action == "stdreg")
                //{
                //    string[] columNames = new string[] { "Registration Number", "Name", "Name Ar" };
                //    filter.Page = 0;
                //    filter.PageCount = 1000;
                //    var list = await Mediator.Send(new GetWebStudentRegistrationList() { Input = filter, User = null });
                //    var fileName = "student registration list";
                //    var stream = await GenerateExcel(fileName, columNames, list.Items);
                //    return Ok(stream);

                //    //fileName = $"{fileName}.xlsx";
                //    //return Ok(File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
                //}
                //else if (action == "stdmst")
                //{

                //}
            }
            return BadRequest();
        }

        private async Task<MemoryStream> GenerateExcel(string fileName, string[] columns, List<TblWebStudentRegistrationDto> items)
        {
            //ExcelPackage = LicenseContext.NonCommercial;

            string excelFileName = fileName;
            int columnIndex = 1;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(excelFileName);

                // Add headers
                foreach (var column in columns)
                {
                    worksheet.Cells[1, columnIndex].Value = column;
                    columnIndex++;
                }

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Adjust column widths
                //worksheet.Column(1).Width = 10;
                //worksheet.Column(2).Width = 20;
                //worksheet.Column(3).Width = 30;
                //worksheet.Column(4).Width = 15;

                // Add student data

                // Export as Excel file
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }

        //private void SetStudentRegistrationData(ExcelWorksheet worksheet, )
        //{
        //        for (int i = 0; i < items.Count; i++)
        //        {
        //            worksheet.Cells[i + 2, 1].Value = items[i].RegNum;
        //            worksheet.Cells[i + 2, 2].Value = items[i].FullName;
        //            worksheet.Cells[i + 2, 3].Value = items[i].NameAr;
        //        }

        //}

    }
}
