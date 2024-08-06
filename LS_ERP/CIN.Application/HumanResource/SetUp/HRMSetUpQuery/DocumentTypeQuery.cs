using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList

    public class GetDocumentTypeList : IRequest<PaginatedList<TblHRMSysDocumentTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDocumentTypeListHandler : IRequestHandler<GetDocumentTypeList, PaginatedList<TblHRMSysDocumentTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDocumentTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysDocumentTypeDto>> Handle(GetDocumentTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetDocumentTypeList method start----");
                var search = request.Input.Query;

                var list = await _context.DocumentTypes
                    .AsNoTracking()
                    .ProjectTo<TblHRMSysDocumentTypeDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.DocumentTypeCode.Contains(search) || e.DocumentTypeNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetDocumentTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDocumentTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetDocumentTypeById

    public class GetDocumentTypeById : IRequest<TblHRMSysDocumentTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDocumentTypeByIdHandler : IRequestHandler<GetDocumentTypeById, TblHRMSysDocumentTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDocumentTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysDocumentTypeDto> Handle(GetDocumentTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetDocumentTypeById method start----");
            try
            {
                var visaType = await _context.DocumentTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysDocumentTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetDocumentTypeById method end----");

                if (visaType is not null)
                    return visaType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDocumentTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateDocumentType

    public class CreateUpdateDocumentType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysDocumentTypeDto Input { get; set; }
    }
    public class CreateUpdateDocumentTypeHandler : IRequestHandler<CreateUpdateDocumentType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDocumentTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateDocumentType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateDocumentType method start----");
                    var obj = request.Input;
                    TblHRMSysDocumentType documentType = new();

                    documentType = await _context.DocumentTypes.FirstOrDefaultAsync(e => e.DocumentTypeCode == request.Input.DocumentTypeCode);

                    if (documentType is not null)
                    {
                        documentType.DocumentTypeNameEn = obj.DocumentTypeNameEn;
                        documentType.DocumentTypeNameAr = obj.DocumentTypeNameAr;
                        documentType.Id = obj.Id;
                        documentType.IsMandatory = obj.IsMandatory;
                        documentType.IsActive = obj.IsActive;
                        documentType.ModifiedBy = request.User.UserId;
                        documentType.Modified = DateTime.Now;

                        _context.DocumentTypes.Update(documentType);
                    }
                    else
                    {
                        documentType = new()
                        {
                            DocumentTypeCode = obj.DocumentTypeCode,
                            DocumentTypeNameEn = obj.DocumentTypeNameEn,
                            DocumentTypeNameAr = obj.DocumentTypeNameAr,
                            IsMandatory = obj.IsMandatory,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.DocumentTypes.AddAsync(documentType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateDocumentType method Exit----");
                    return ApiMessageInfo.Status(1, documentType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateDocumentType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteDocumentType
    public class DeleteDocumentType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDocumentTypeHandler : IRequestHandler<DeleteDocumentType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDocumentTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDocumentType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteDocumentType method start----");
                if (request.Id > 0)
                {
                    var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(documentType);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteDocumentType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteDocumentType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDocumentTypeSelectListItem
    public class GetDocumentTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetDocumentTypeSelectListItemHandler : IRequestHandler<GetDocumentTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDocumentTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetDocumentTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.DocumentTypes
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.DocumentTypeNameAr : e.DocumentTypeNameEn, Value = e.DocumentTypeCode })
                .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
