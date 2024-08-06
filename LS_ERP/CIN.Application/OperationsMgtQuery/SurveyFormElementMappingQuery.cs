//using AutoMapper;
//using CIN.Application.Common;
//using CIN.Application.OperationsMgtDtos;
//using CIN.DB;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper.QueryableExtensions;
//using System.Linq.Dynamic.Core;
//using CIN.Domain.OpeartionsMgt;


//namespace CIN.Application.OperationsMgtQuery
//{



//    #region CreateUpdateSurveyFormElementMapping
//    public class CreateSurveyFormElementMapping : IRequest<int>
//    {
//        public UserIdentityDto User { get; set; }
//        public TblSndDefSurveyFormElementsMappDto Dto { get; set; }
//    }

//    public class CreateSurveyFormElementMappingHandler : IRequestHandler<CreateSurveyFormElementMapping, int>
//    {
//        private readonly CINDBOneContext _context;
//        private readonly IMapper _mapper;

//        public CreateSurveyFormElementMappingHandler(CINDBOneContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<int> Handle(CreateSurveyFormElementMapping request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                Log.Info("----Info CreateUpdateSurveyFormElementMapping method start----");



//                var obj = request.Dto;


//                TblSndDefSurveyFormElementsMapp SurveyFormElementMapp = new();
//                if (obj.Id > 0)
//                    SurveyFormElementMapp = await _context.OprSurveyFormElementsMapp.FirstOrDefaultAsync(e => e.Id == obj.Id);
//                SurveyFormElementMapp.Id = obj.Id;
//                //SurveyFormElementMapp.ModifiedOn = obj.ModifiedOn;
//                SurveyFormElementMapp.IsActive = obj.IsActive;
//                SurveyFormElementMapp.SurveyFormCode = obj.SurveyFormCode;
//                SurveyFormElementMapp.FormElementCode = obj.FormElementCode;



//                if (obj.Id > 0)
//                {
//                    obj.ModifiedOn = DateTime.Now;

//                    _context.OprSurveyFormElementsMapp.Update(SurveyFormElementMapp);
//                }
//                else
//                {
//                    obj.CreatedOn = DateTime.Now;
//                    await _context.OprSurveyFormElementsMapp.AddAsync(SurveyFormElementMapp);
//                }
//                await _context.SaveChangesAsync();
//                Log.Info("----Info CreateUpdateSurveyFormElementMapping method Exit----");
//                return SurveyFormElementMapp.Id;
//            }
//            catch (Exception ex)
//            {
//                Log.Error("Error in CreateUpdateSurveyFormElementMapping Method");
//                Log.Error("Error occured time : " + DateTime.UtcNow);
//                Log.Error("Error message : " + ex.Message);
//                Log.Error("Error StackTrace : " + ex.StackTrace);
//                return 0;
//            }
//        }
//    }

//    #endregion



//    #region DeleteSurveyFormElementMapp
//    public class DeleteSurveyFormElementMapping : IRequest<int>
//    {
//        public UserIdentityDto User { get; set; }
//        public int Id { get; set; }
//    }

//    public class DeleteSurveyFormElementMappingHandler : IRequestHandler<DeleteSurveyFormElementMapping, int>
//    {
//        //private readonly ICurrentUserService _currentUserService;
//        //protected string UserId => _currentUserService.UserId;
//        private readonly CINDBOneContext _context;
//        private readonly IMapper _mapper;

//        public DeleteSurveyFormElementMappingHandler(CINDBOneContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<int> Handle(DeleteSurveyFormElementMapping request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                Log.Info("----Info DeleteSurveyFormElementMapping start----");

//                if (request.Id > 0)
//                {
//                    var formElement = await _context.OprSurveyFormElements.FirstOrDefaultAsync(e => e.Id == request.Id);
//                    _context.Remove(formElement);

//                    await _context.SaveChangesAsync();

//                    return request.Id;
//                }
//                return 0;
//            }
//            catch (Exception ex)
//            {

//                Log.Error("Error in DeleteSurveyFormElementMapping");
//                Log.Error("Error occured time : " + DateTime.UtcNow);
//                Log.Error("Error message : " + ex.Message);
//                Log.Error("Error StackTrace : " + ex.StackTrace);
//                return 0;
//            }

//        }
//    }

//    #endregion

//}
