using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SndQuery
{




    #region GetSelectItemList

    public class GetSelectItemList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectItemListHandler : IRequestHandler<GetSelectItemList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectItemList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectItemList method start----");
            var item = await _context.InvItemMaster.AsNoTracking()
                // .Where(e => e.CompanyId == request.User.CompanyId)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.ShortName ,TextTwo= e.ShortNameAr, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectItemList method Ends----");
            return item;
        }
    }

    #endregion

    #region ItemUnitPriceItem

    public class ItemUnitPriceItem : IRequest<ItemUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ItemUnitPriceItemHandler : IRequestHandler<ItemUnitPriceItem, ItemUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ItemUnitPriceItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ItemUnitPriceDTO> Handle(ItemUnitPriceItem request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info ItemUnitPriceItem method start----");

                var item = await _context.InvItemMaster.AsNoTracking()
                    .Where(e => e.Id == request.Id)

                   .Select(Item => new ItemUnitPriceDTO
                   {
                       ItemId = Item.Id,
                       ItemCode = Item.ItemCode,
                       Description = Item.ItemDescription,
                       UnitType=Item.ItemBaseUnit,
                       UnitTypeEN = Item.ItemBaseUnit,//Item.ItemType,
                   UnitTypeAR = Item.ItemBaseUnit,// Item.ItemType,
                   UnitPrice = Item.ItemStandardCost.ToString(),
                       NameEN = Item.ShortName,
                       NameAR = Item.ShortNameAr,
                       Vat=_context.SystemTaxes.FirstOrDefault(e=>e.TaxCode==Item.ItemTaxCode).Taxper01,
                        
                   })
                      .FirstOrDefaultAsync(cancellationToken);

                if(item is not null)
                {
                    var uom = await _context.InvItemsUOM.FirstOrDefaultAsync(e=>e.ItemCode==item.ItemCode && e.ItemUOM==item.UnitType);
                    if(uom is not null)
                    {
                        item.UnitPrice = uom.ItemUOMPrice1.ToString();
                    }
                }
                Log.Info("----Info ItemUnitPriceItem method Ends----");
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ItemUnitPriceItem Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion




    #region ItemUnitPriceItemUnit

    public class ItemUnitPriceItemUnit : IRequest<ItemUnitPriceDTO>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode{ get; set; }
        public string  UnitType { get; set; }
    }

    public class ItemUnitPriceItemUnitHandler : IRequestHandler<ItemUnitPriceItemUnit, ItemUnitPriceDTO>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ItemUnitPriceItemUnitHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ItemUnitPriceDTO> Handle(ItemUnitPriceItemUnit request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info ItemUnitPriceItemUnit method start----");
                var item = await _context.InvItemsUOM.AsNoTracking()
                    .Where(e => e.ItemCode == request.ItemCode &&e.ItemUOM==request.UnitType)

                   .Select(Item => new ItemUnitPriceDTO
                   {
                       //ItemId = Item.Id,
                       //ItemCode = Item.ItemCode,
                       //Description =
                       //UnitTypeEN = _context.InvItemsUOM.FirstOrDefault(e => e.ItemCode == Item.ItemCode).ItemUOM,//Item.ItemType,
                       //UnitTypeAR = _context.InvItemsUOM.FirstOrDefault(e => e.ItemCode == Item.ItemCode).ItemUOM,// Item.ItemType,
                       UnitPrice = Item.ItemUOMPrice1.ToString(),
                       //NameEN =,
                       //NameAR = 
                   })
                      .FirstOrDefaultAsync(cancellationToken);

                Log.Info("----Info ItemUnitPriceItemUnit method Ends----");
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ItemUnitPriceItemUnit Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

#region ItemUomMap

    public class GetItemUomMapByItemUnit : IRequest<TblErpInvItemsUOMDto>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode{ get; set; }
        public string  UnitType { get; set; }
    }

    public class GetItemUomMapByItemUnitHandler : IRequestHandler<GetItemUomMapByItemUnit, TblErpInvItemsUOMDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemUomMapByItemUnitHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpInvItemsUOMDto> Handle(GetItemUomMapByItemUnit request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info ItemUomMap method start----");
                var item = await _context.InvItemsUOM.AsNoTracking().ProjectTo<TblErpInvItemsUOMDto>(_mapper.ConfigurationProvider)
                    .Where(e => e.ItemCode == request.ItemCode &&e.ItemUOM==request.UnitType)

                   
                      .FirstOrDefaultAsync(cancellationToken);

                Log.Info("----Info ItemUomMap method Ends----");
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ItemUomMap Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region GetSelectItemMOUList

    public class GetSelectItemMOUList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
         }

    public class GetSelectItemMOUListHandler : IRequestHandler<GetSelectItemMOUList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectItemMOUListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectItemMOUList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectItemMOUList method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                var items = await _context.InvItemsUOM.Include(e => e.InvItemMaster)
                     .Select(e => new CustomSelectListItem { Text = isArab? e.InvItemMaster.ShortNameAr:e.InvItemMaster.ShortName , Value = e.InvItemMaster.Id.ToString() ,TextTwo=e.ItemCode})
                       .ToListAsync();
                var resList = items.GroupBy(e => e.Value).Select(x => new CustomSelectListItem { Text = x.First().Text, Value = x.First().Value,TextTwo=x.First().TextTwo }).ToList();

                return resList;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSelectItemMOUList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion
    #region GetSelectItemMOUUnitTypeListByItem

    public class GetSelectItemMOUUnitTypeListByItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectItemMOUUnitTypeListByItemHandler : IRequestHandler<GetSelectItemMOUUnitTypeListByItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectItemMOUUnitTypeListByItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectItemMOUUnitTypeListByItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectItemMOUUnitTypeListByItem method start----");
            try
            {
                var items = await _context.InvItemsUOM.Where(e=>e.ItemCode==request.Input)
                     .Select(e => new CustomSelectListItem { Text = e.ItemUOM , Value = e.ItemUOM.ToString() })
                       .ToListAsync();
               

                return items;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSelectItemMOUUnitTypeListByItem Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region ItemStockAvailability

    public class ItemStockAvailability : IRequest<List<TblErpInvItemInventory>>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }
    }

    public class ItemStockAvailabilityHandler : IRequestHandler<ItemStockAvailability, List<TblErpInvItemInventory>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ItemStockAvailabilityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblErpInvItemInventory>> Handle(ItemStockAvailability request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info ItemStockAvailability method start----");
                var items = await _context.InvItemInventory.Where(e=>e.ItemCode==request.ItemCode).Include(e=>e.InvWarehouses).Include(e=>e.InvItemMaster).ToListAsync();

                Log.Info("----Info ItemStockAvailability method Ends----");
                return items;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ItemStockAvailability Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion





    #region GetItemByItemCode

    public class GetItemByItemCode : IRequest<TblErpInvItemMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string ItemCode { get; set; }
    }

    public class GetItemByItemCodeHandler : IRequestHandler<GetItemByItemCode, TblErpInvItemMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetItemByItemCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpInvItemMasterDto> Handle(GetItemByItemCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetItemByItemCode method start----");
                var item = await _context.InvItemMaster.AsNoTracking().ProjectTo<TblErpInvItemMasterDto>(_mapper.ConfigurationProvider).FirstAsync(e => e.ItemCode == request.ItemCode);

                Log.Info("----Info GetItemByItemCode method Ends----");
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetItemByItemCode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion



    

 #region GetSelectedItemByItemBarcode

    public class GetSelectedItemByItemBarcode : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string Barcode { get; set; }
    }

    public class GetSelectedItemByItemBarcodeHandler : IRequestHandler<GetSelectedItemByItemBarcode, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectedItemByItemBarcodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetSelectedItemByItemBarcode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetSelectedItemByItemBarcode method start----");
                var barcode = await _context.InvItemsBarcode.AsNoTracking().FirstAsync(e => e.ItemBarcode == request.Barcode);
                if (barcode is null) return null;
                var item = await _context.InvItemMaster.Select(e => new CustomSelectListItem() { Text = e.ItemCode, Value = e.Id.ToString(),TextTwo=barcode.ItemUOM }).FirstOrDefaultAsync(e => e.Text == barcode.ItemCode);

                Log.Info("----Info GetSelectedItemByItemBarcode method Ends----");
                return item;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSelectedItemByItemBarcode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion
}
