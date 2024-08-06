using CIN.Application.OperationsMgtDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.Common
{
    public static class PaginationFilterExt
    {
        // public static PaginationDdlFilterDto DdlValues(this PaginationDdlFilterDto filter) => PValues(filter);
        public static PaginationFilterDto Values(this PaginationFilterDto filter)
        {
            filter.Query = filter.Query ?? string.Empty;
            filter.Approval = filter.Approval ?? string.Empty;

            return filter;
        }
        public static OprPaginationFilterDto Values(this OprPaginationFilterDto filter)
        {
            filter.Query = filter.Query ?? string.Empty;
            filter.Approval = filter.Approval ?? string.Empty;
            filter.ListType = filter.ListType ?? string.Empty;

            return filter;
        }

        public static PaginationParametersDto Values(this PaginationParametersDto filter)
        {
            filter.Query = filter.Query ?? string.Empty;
            filter.Approval = filter.Approval ?? string.Empty;

            return filter;
        }


    }
    public class ReportListDto<T>
    {
        public List<T> List { get; set; }
        public int ReportCount { get; set; }
    }
    public class UserIdentityDto
    {
        public string Culture { get; set; } = "en-US";
        public int UserId { get; set; }
        public string UserName { get; set; }
        public long RoleId { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        //public string ConnectionString { get; set; }
        public string ModuleCodes { get; set; }
    }
    public class UserMobileIdentityDto : UserIdentityDto
    {
        public long EmployeeId { get; set; }
        public string SiteCode { get; set; }
    }
    public class PaginationFilterDto
    {
        public int Id { get; set; }
        public int Page { get; set; } = 0;
        public int PageCount { get; set; }
        //private int _pageCount = maxPageCount;
        //public int PageCount
        //{
        //    get { return _pageCount; }
        //    set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        //}

        public string Query { get; set; } = string.Empty;

        public string OrderBy { get; set; }

        public string Approval { get; set; }
        public short? StatusId { get; set; }
        public string StuAdmNum { get; set; }
        public string Code { get; set; }
        public string TeacherCode { get; set; }
    }
    public class OprPaginationFilterDto
    {
        public int Id { get; set; }
        public int Page { get; set; } = 0;
        public int PageCount { get; set; }
        //private int _pageCount = maxPageCount;
        //public int PageCount
        //{
        //    get { return _pageCount; }
        //    set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        //}

        public string Query { get; set; } = string.Empty;

        public string OrderBy { get; set; }

        public string Approval { get; set; }
        public short? StatusId { get; set; }

        public string Code { get; set; }
        public string ListType { get; set; }



    }

    public class OprFilter
    {
        public string CustomerCode { get; set; }
        public string BranchCode { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public bool? IsActive { get; set; }
    }

    //public class PaginationDdlFilterDto : PaginationFilterDto
    //{
    //    public string Approval { get; set; }
    //}

    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }       

        //public bool HasPreviousPage => PageIndex > 1;

        //public bool HasNextPage => PageIndex < TotalPages;

        //public static async Task<PaginatedList<T>> PaginationListAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
        //{
        //    var count = await source.CountAsync();
        //    var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        //    return new PaginatedList<T>(items, count, pageIndex, pageSize);
        //}
    }

    public static class PaginationExt
    {

        public static IQueryable<T> Pagination<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            //return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return source.Skip((pageIndex) * pageSize).Take(pageSize);

        }
        public static async Task<PaginatedList<T>> PaginationListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
