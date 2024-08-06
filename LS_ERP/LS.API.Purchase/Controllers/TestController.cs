

//#region GetInvItemSelectListByPoNumber

//using AutoMapper;
//using CIN.Application.Common;
//using CIN.Application.InventoryDtos;
//using CIN.DB;
//using MediatR;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Threading;
//using CIN.Application;

//public class GetInvItemSelectListByPoNumber : IRequest<List<CustomSelectListItem>>
//{
//    public string PurchaseOrderNO { get; set; }
//}

//public class GetInvItemSelectListByPoNumberHandler : IRequestHandler<GetInvItemSelectListByPoNumber, List<CustomSelectListItem>>
//{
//    private readonly CINDBOneContext _context;
//    private readonly IMapper _mapper;

//    public GetInvItemSelectListByPoNumberHandler(CINDBOneContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<List<CustomSelectListItem>> Handle(GetInvItemSelectListByPoNumber request, CancellationToken cancellationToken)
//    {
//        return await _context.purchaseOrderDetails.AsNoTracking()
//            .Where()
//            .OrderByDescending(e => e.Id)
//            .ProjectTo<CustomSelectListItem>(_mapper.ConfigurationProvider)
//            .ToListAsync(cancellationToken);
//    }
//}

//#endregion