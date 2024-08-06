using AutoMapper;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.Common
{

    public class TblErpSysFileUploadDto : PrimaryKeyDto<int>
    {
        public string SourceId { get; set; }

        [StringLength(20)]
        public string Type { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(80)]
        public string FileName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(124)]
        public string UploadedBy { get; set; }
        public bool Status { get; set; }
    }



    #region GetFilesByModulewiseId

    public class GetFilesByModulewiseId : IRequest<List<TblErpSysFileUploadDto>>
    {
        public FileUploadItem Input { get; set; }
    }
    public class GetFilesByModulewiseIdQueryHandler : IRequestHandler<GetFilesByModulewiseId, List<TblErpSysFileUploadDto>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetFilesByModulewiseIdQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblErpSysFileUploadDto>> Handle(GetFilesByModulewiseId request, CancellationToken cancellationToken)
        {
            var input = request.Input;
            if (input == null || !input.Action.HasValue())
                return new List<TblErpSysFileUploadDto>();

            return await _context.FileUploads.Where(e => e.SourceId == input.SourceId && e.Type == input.Action).Select(e =>
            new TblErpSysFileUploadDto
            {
                Id = e.Id,
                //SourceId = e.SourceId,
                FileName = e.FileName,
                Name = e.Name,
                Description = e.Description,
                //Type = e.Type,
                Status = e.Status,
            }).ToListAsync();
        }
    }
    #endregion


    #region UploadingFile
    public class UploadingFile : IRequest<AppCtrollerDto>
    {
        public List<TblErpSysFileUploadDto> Input { get; set; }
    }
    public class UploadingFileQueryHandler : IRequestHandler<UploadingFile, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UploadingFileQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(UploadingFile request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info UploadingFileQuery method start----");

                List<TblErpSysFileUpload> cObjList = new();

                foreach (var obj in request.Input)
                {
                    cObjList.Add(new()
                    {
                        SourceId = obj.SourceId,
                        Type = obj.Type,
                        FileName = obj.FileName,
                        Name = obj.Name,
                        Description = obj.Description,
                        UploadedBy = obj.UploadedBy,
                        Status = true
                    });

                }
                await _context.AddRangeAsync(cObjList);
                await _context.SaveChangesAsync();
                Log.Info("----Info UploadingFileQuery method Exit----");
                return ApiMessageInfo.Status(1, 1);
            }
            catch (Exception ex)
            {
                Log.Error("Error in UploadingFileQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


    #region DeletingFile
    public class DeletingFile : IRequest<AppCtrollerDto>
    {
        public int Id { get; set; }
    }
    public class DeletingFileQueryHandler : IRequestHandler<DeletingFile, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletingFileQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(DeletingFile request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletingFileQuery method start----");
                var fileUpload = await _context.FileUploads.FirstOrDefaultAsync(e => e.Id == request.Id);
                _context.FileUploads.Remove(fileUpload);
                await _context.SaveChangesAsync();
                Log.Info("----Info DeletingFileQuery method Exit----");
                return ApiMessageInfo.Status(1, request.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeletingFileQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0);
            }
        }
    }
    #endregion


}
