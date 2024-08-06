using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Dtos;
using CIN.DB;
using CIN.Domain.MobileMgt;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.MobileMgt.Queries
{





    #region PushNotificationAction

    public class PushNotificationActionQuery : IRequest<bool>
    {
        public UserMobileIdentityDto User { get; set; }
        public TblErpSysPushNotification Input { get; set; }
    }

    public class PushNotificationActionQueryHandler : IRequestHandler<PushNotificationActionQuery, bool>
    {
        private readonly DMC2Context _dmcContext;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public PushNotificationActionQueryHandler(IMapper mapper, DMC2Context dmcContext, CINDBOneContext context)
        {
            _mapper = mapper;
            _dmcContext = dmcContext;
            _context = context;
        }
        public async Task<bool> Handle(PushNotificationActionQuery request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info PushNotificationActionQuery method start----");
                var log = await _context.ErpSysPushNotifications.OrderByDescending(e => e.Id).FirstOrDefaultAsync(e=>e.UserId==request.Input.UserId&&e.Token==request.Input.Token && e.Status);
                if (request.Input.Status == true)
                {
                    if(log is null)
                    {
                        TblErpSysPushNotification newLog = new TblErpSysPushNotification
                        { 
                         Id=0,
                          Status=true,
                           Token=request.Input.Token,
                            UserId=request.Input.UserId,
                            CreatedOn=DateTime.UtcNow
                        };
                        await _context.ErpSysPushNotifications.AddAsync(newLog);
                        await _context.SaveChangesAsync();
                    }
                }
                else 
                {
                    if (log is not null)
                    {
                       
                       _context.ErpSysPushNotifications.Remove(log);
                        await _context.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error in PushNotificationActionQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return false;
            }
        }
    }




    #endregion




}
