using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using CIN.Domain.SchoolMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SchoolMgtQuery
{
    public class SchoolTeacherLanguagesQuery
    {

    }
    #region GetSchoolTeacherLanguagesByTeacherCode 
    public class GetSchoolTeacherLanguagesByTeacherCode : IRequest<List<TblDefSchoolTeacherLanguagesDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
    }

    public class GetSchoolTeacherLanguagesByTeacherCodeHandler : IRequestHandler<GetSchoolTeacherLanguagesByTeacherCode, List<TblDefSchoolTeacherLanguagesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolTeacherLanguagesByTeacherCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefSchoolTeacherLanguagesDto>> Handle(GetSchoolTeacherLanguagesByTeacherCode request, CancellationToken cancellationToken)
        {
            List<TblDefSchoolTeacherLanguagesDto> result = new();
            result = await _context.DefSchoolTeacherLanguages.AsNoTracking().ProjectTo<TblDefSchoolTeacherLanguagesDto>(_mapper.ConfigurationProvider).Where(e => e.TeacherCode == request.TeacherCode).ToListAsync();
            return result;
        }
    }

    #endregion

    #region SchoolTeacherLanguagesMapping

    public class SchoolTeacherLanguagesMapping : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblDefSchoolTeacherLanguagesDto> SchoolTeacherLanguagesList { get; set; }
    }

    public class SchoolTeacherLanguagesMappingHandler : IRequestHandler<SchoolTeacherLanguagesMapping, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public SchoolTeacherLanguagesMappingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(SchoolTeacherLanguagesMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info SchoolTeacherLanguagesMapping method start----");
                    List<TblDefSchoolTeacherLanguages> languageList = new();
                    if (request.SchoolTeacherLanguagesList.Count() > 0)
                    {
                        var oldList = await _context.DefSchoolTeacherLanguages.Where(e => e.TeacherCode == request.SchoolTeacherLanguagesList.FirstOrDefault().TeacherCode).ToListAsync();
                        _context.DefSchoolTeacherLanguages.RemoveRange(oldList);

                        
                        foreach (var teacherLanguage in request.SchoolTeacherLanguagesList)
                        {
                            TblDefSchoolTeacherLanguages detail = new()
                            {
                                Id= teacherLanguage.Id,
                                LanguageCode= teacherLanguage.LanguageCode,
                                TeacherCode= teacherLanguage.TeacherCode,
                                Read= teacherLanguage.Read,
                                Speak= teacherLanguage.Speak,
                                Write= teacherLanguage.Write
                            };
                            languageList.Add(detail);
                        }
                        await _context.DefSchoolTeacherLanguages.AddRangeAsync(languageList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info SchoolTeacherLanguagesMapping  method Exit----");
                    await transaction.CommitAsync();
                    return languageList.FirstOrDefault().Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in SchoolTeacherLanguagesMapping  Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }








    }
    #endregion
}
