using AutoMapper;
using CIN.Application.Common;
using CIN.Application.HumanResource.Utility;
using CIN.Application.Payroll.Management.Dtos;
using CIN.Application.Payroll.Utility;
using CIN.DB;
using CIN.Domain.Payroll.Management;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.Management.Query
{
    #region GetEmployeePaySlip

    public class GetEmployeePaySlip : IRequest<BaseEmployeePaySlipDto>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeePaySlipHandler : IRequestHandler<GetEmployeePaySlip, BaseEmployeePaySlipDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeePaySlipHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseEmployeePaySlipDto> Handle(GetEmployeePaySlip request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info GetEmployeePaySlip method start----");

                    bool isArab = request.User.Culture.IsArab();
                    BaseEmployeePaySlipDto objBaseEmployeePaySlip = new BaseEmployeePaySlipDto();

                    //Retrieve Header details.
                    var employeeContractInfo = await (from contract in _context.EmployeeContracts
                                                      join personalInformation in _context.PersonalInformation on contract.EmployeeID equals personalInformation.Id
                                                      join companyBranches in _context.CompanyBranches on contract.BranchCode equals companyBranches.BranchCode
                                                      join grade in _context.Grades on contract.GradeCode equals grade.GradeCode
                                                      join position in _context.Positions on contract.PositionCode equals position.PositionCode
                                                      join payrollgroup in _context.PayrollGroups on contract.PayrollGroupCode equals payrollgroup.PayrollGroupCode
                                                      select new
                                                      {
                                                          EmployeeID = contract.EmployeeID,
                                                          EmployeeNumber = personalInformation.EmployeeNumber,
                                                          EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                                          BranchName = companyBranches.BranchName,
                                                          GradeName = isArab ? grade.GradeNameAr : grade.GradeNameEn,
                                                          PositionName = isArab ? position.PositionNameAr : position.PositionNameEn,
                                                          PayrollMonth = payrollgroup.PayrollGroupEndDate.ToString("MMMM yyyy"),
                                                          PayrollGroupCode = contract.PayrollGroupCode,
                                                          PayrollGroupStartDate = payrollgroup.PayrollGroupStartDate,
                                                          PayrollGroupEndDate = payrollgroup.PayrollGroupEndDate.AddDays(1).AddSeconds(-1),
                                                          EmployeeStatusCode = contract.EmployeeStatusCode,
                                                      })
                                                   .AsNoTracking()
                                                   .Where(e => e.EmployeeID == request.EmployeeID && e.EmployeeStatusCode == EmployeeStatus.ACTIVE)
                                                   .FirstOrDefaultAsync(cancellationToken);

                    if (employeeContractInfo is null)
                    {
                        objBaseEmployeePaySlip.StatusMessage = $"EmployeeInfo with EmployeeID: {request.EmployeeID} Not Found";
                        return objBaseEmployeePaySlip;
                    }

                    //Retrieve employee's consolidated attendance.
                    var consolidatedEmployeeAttendance = await _context.ConsolidatedEmployeeAttendance.AsNoTracking()
                        .Where(e => (e.EmployeeID == request.EmployeeID &&
                        e.PayrollPeriodCode.Equals(employeeContractInfo.PayrollMonth) &&
                        e.ShiftNumber == 1)).FirstOrDefaultAsync(cancellationToken);

                    if (consolidatedEmployeeAttendance is null)
                    {
                        objBaseEmployeePaySlip.StatusMessage = $"Consolidated Attendance of Employee: {employeeContractInfo.EmployeeName} for the Month: {employeeContractInfo.PayrollMonth} Not Found";
                        return objBaseEmployeePaySlip;
                    }

                    //Retrieve Payslip Header.
                    EmployeePaySlipHeader paySlipHeader = new EmployeePaySlipHeader()
                    {
                        EmployeeID = employeeContractInfo.EmployeeID,
                        EmployeeNumber = employeeContractInfo.EmployeeNumber,
                        EmployeeName = employeeContractInfo.EmployeeName,
                        BranchName = employeeContractInfo.BranchName,
                        GradeName = employeeContractInfo.GradeName,
                        PositionName = employeeContractInfo.PositionName,
                        PayrollMonth = employeeContractInfo.PayrollMonth,
                        CalandarDays = consolidatedEmployeeAttendance.TotalDays,
                        TotalOffDays = consolidatedEmployeeAttendance.TotalOffDays,
                        TotalHolidays = consolidatedEmployeeAttendance.TotalHolidays,
                        TotalLeaves = consolidatedEmployeeAttendance.TotalLeaves,
                        TotalAbsents = consolidatedEmployeeAttendance.TotalAbsents,
                        NetWorkingDays = consolidatedEmployeeAttendance.NetWorkingDays
                    };

                    //Retrieve Employee's Structured Payroll.
                    var employeeStructuredPayrollDto = await (from employeePayrollStructure in _context.EmployeePayrollStructure
                                                              join payrollComponent in _context.PayrollComponents on employeePayrollStructure.PayrollComponentCode equals payrollComponent.PayrollComponentCode
                                                              select new TblPRLTrnEmployeePayrollProcessDto
                                                              {
                                                                  Id = employeePayrollStructure.Id,
                                                                  EmployeeID = employeePayrollStructure.EmployeeID,
                                                                  PayrollMonth = employeeContractInfo.PayrollMonth,
                                                                  PayrollComponentCode = employeePayrollStructure.PayrollComponentCode,
                                                                  PayrollComponentName = isArab ? payrollComponent.PayrollComponentNameAr : payrollComponent.PayrollComponentNameEn,
                                                                  IsFormula = payrollComponent.IsFormula,
                                                                  PayValue = employeePayrollStructure.PayValue,
                                                                  PayrollComponentType = payrollComponent.PayrollComponentType,
                                                                  FormulaQueryString = employeePayrollStructure.FormulaQueryString
                                                              }).AsNoTracking()
                                   .Where(e => e.EmployeeID == request.EmployeeID)
                                   .OrderByDescending(e => e.Id)
                                   .ToListAsync(cancellationToken);

                    if (employeeStructuredPayrollDto is not null)
                    {

                        //Delete the existing Processed payroll records of the employee for the current PayrollMonth.
                        var existingRecords = await _context.EmployeePayrollProcess
                            .Where(e => e.EmployeeID == request.EmployeeID && e.PayrollMonth.Equals(employeeContractInfo.PayrollMonth) && !e.IsApproved)
                            .ToListAsync();

                        if (existingRecords is not null)
                            _context.EmployeePayrollProcess.RemoveRange(existingRecords);

                        #region Employee Structured Payroll

                        //Insert Employee Structured Payroll with current PayrollMonth into the Employee Payroll Process
                        List<TblPRLTrnEmployeePayrollProcess> employeePayrollProcessEntities = new();
                        employeeStructuredPayrollDto.ForEach(e =>
                        {
                            TblPRLTrnEmployeePayrollProcess employeePayrollProcessEntity = new TblPRLTrnEmployeePayrollProcess()
                            {
                                EmployeeID = e.EmployeeID,
                                PayrollMonth = e.PayrollMonth,
                                PayrollComponentCode = e.PayrollComponentCode,
                                PayValue = e.PayValue,
                                FormulaQueryString = e.FormulaQueryString,
                                IsActive = true,
                                CreatedBy = request.User.UserId,
                                Created = DateTime.Now
                            };
                            employeePayrollProcessEntities.Add(employeePayrollProcessEntity);
                        });

                        if (employeePayrollProcessEntities.Count > 0)
                        {
                            await _context.EmployeePayrollProcess.AddRangeAsync(employeePayrollProcessEntities);
                            await _context.SaveChangesAsync();
                        }

                        #endregion

                        #region Employee Unstructured Payroll

                        if (consolidatedEmployeeAttendance is not null)
                        {
                            decimal TotalEarnings = employeeStructuredPayrollDto.Where(e => e.PayrollComponentType == (int)PayrollComponentType.Earning).Sum(e => e.PayValue);
                            decimal AverageSalaryPerDay = TotalEarnings / paySlipHeader.CalandarDays;

                            //Check if employee has any absents in current Payroll month.
                            if (consolidatedEmployeeAttendance.TotalAbsents > 0)
                            {
                                decimal AbsentRatio = 1;
                                decimal AbsenceDeduction = consolidatedEmployeeAttendance.TotalAbsents * (AbsentRatio * AverageSalaryPerDay);

                                if (AbsenceDeduction > 0)
                                {
                                    //Delete the existing Absence Deductions for the employee.
                                    var existingAbsenceDeductions = await _context.EmployeePayrollUnStructured
                                        .Where(e => (e.EmployeeID == employeeContractInfo.EmployeeID &&
                                        e.PayrollMonth.Equals(employeeContractInfo.PayrollMonth) &&
                                        e.PayrollComponentCode.Equals(PayrollComponent.ABSENT))).ToListAsync(cancellationToken);

                                    _context.EmployeePayrollUnStructured.RemoveRange(existingAbsenceDeductions);

                                    //Insert into Absence Deduction for the employee.
                                    TblPRLTrnEmployeePayrollUnStructured employeePayrollUnStructured = new TblPRLTrnEmployeePayrollUnStructured()
                                    {
                                        EmployeeID = employeeContractInfo.EmployeeID,
                                        PayrollMonth = employeeContractInfo.PayrollMonth,
                                        PayrollComponentCode = PayrollComponent.ABSENT,
                                        PayValue = AbsenceDeduction,
                                        IsActive = true,
                                        CreatedBy = request.User.UserId,
                                        Created = DateTime.Now
                                    };
                                    await _context.EmployeePayrollUnStructured.AddAsync(employeePayrollUnStructured);
                                    await _context.SaveChangesAsync();

                                    //Insert into Employee Payroll Process.
                                    TblPRLTrnEmployeePayrollProcess employeePayrollProcessEntity = new TblPRLTrnEmployeePayrollProcess()
                                    {
                                        EmployeeID = employeeContractInfo.EmployeeID,
                                        PayrollMonth = employeeContractInfo.PayrollMonth,
                                        PayrollComponentCode = PayrollComponent.ABSENT,
                                        PayValue = AbsenceDeduction,
                                        IsActive = true,
                                        CreatedBy = request.User.UserId,
                                        Created = DateTime.Now
                                    };
                                    await _context.EmployeePayrollProcess.AddAsync(employeePayrollProcessEntity);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }

                        #endregion

                        await transaction.CommitAsync();
                    }

                    //Retrieve Employee PaySlip Details.
                    var employeePayrollProcessDto = await (from employeePayrollProcess in _context.EmployeePayrollProcess
                                                           join payrollComponent in _context.PayrollComponents on employeePayrollProcess.PayrollComponentCode equals payrollComponent.PayrollComponentCode
                                                           select new TblPRLTrnEmployeePayrollProcessDto
                                                           {
                                                               Id = employeePayrollProcess.Id,
                                                               EmployeeID = employeePayrollProcess.EmployeeID,
                                                               PayrollMonth = employeePayrollProcess.PayrollMonth,
                                                               PayrollComponentCode = employeePayrollProcess.PayrollComponentCode,
                                                               PayrollComponentName = isArab ? payrollComponent.PayrollComponentNameAr : payrollComponent.PayrollComponentNameEn,
                                                               IsFormula = payrollComponent.IsFormula,
                                                               PayValue = employeePayrollProcess.PayValue,
                                                               PayrollComponentType = payrollComponent.PayrollComponentType,
                                                               FormulaQueryString = employeePayrollProcess.FormulaQueryString,
                                                               IsApproved = employeePayrollProcess.IsApproved
                                                           }).AsNoTracking()
                                   .Where(e => e.EmployeeID == request.EmployeeID && e.PayrollMonth.Equals(employeeContractInfo.PayrollMonth) && !e.IsApproved)
                                   .OrderByDescending(e => e.Id)
                                   .ToListAsync(cancellationToken);

                    objBaseEmployeePaySlip.PaySlipDetails.AddRange(employeePayrollProcessDto);

                    paySlipHeader.Earnings = employeePayrollProcessDto
                        .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.Earning || e.PayrollComponentType == (int)PayrollComponentType.UnStructuredEarning))
                        .Sum(e => e.PayValue);

                    paySlipHeader.Deductions = employeePayrollProcessDto
                        .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.Deduction || e.PayrollComponentType == (int)PayrollComponentType.UnStructuredDeduction))
                        .Sum(e => e.PayValue);

                    paySlipHeader.Netpay = paySlipHeader.Earnings - paySlipHeader.Deductions;

                    objBaseEmployeePaySlip.PaySlipHeader = paySlipHeader;

                    Log.Info("----Info objBaseEmployeePaySlip method end----");
                    return objBaseEmployeePaySlip;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in objBaseEmployeePaySlip Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    throw;
                }
            }
        }
    }

    #endregion    
}
