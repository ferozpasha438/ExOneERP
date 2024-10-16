﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinPurchaseMgtDto;
using CIN.Application.InventoryDtos;
using CIN.Application.PurchasemgtQuery;
using CIN.Application.PurchaseSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LS.API.Purchase.Controllers.PurchaseMgt
{
    public class InventoryExpirySerialController : BaseController
    {
        public InventoryExpirySerialController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getInvItemSpecifications")]
        public async Task<IActionResult> GetInvItemSpecifications()
        {
            var list = await Mediator.Send(new GetInvItemSpecifications() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getInvItemSelectListByPoNumber")]
        public async Task<IActionResult> GetInvItemSelectListByPoNumber([FromQuery] string purchaseOrderNO)
        {
            var list = await Mediator.Send(new GetInvItemSelectListByPoNumber() { PurchaseOrderNO = purchaseOrderNO, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost("createInvItemExpiryBatch")]
        public async Task<ActionResult> CreateInvItemExpiryBatch([FromBody] TblErpInvGrnItemExpiryBatchListDto input)
        {
            var expBatch = await Mediator.Send(new CreateInvItemExpiryBatch() { Input = input, User = UserInfo() });
            if (expBatch.Id > 0)
            {
                return Created($"get/{expBatch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = expBatch.Message });
        }

        [HttpPost("updateInvPRItemExpiryBatch")]
        public async Task<ActionResult> UpdateInvPRItemExpiryBatch([FromBody] TblErpInvItemExpiryBatchListDto input)
        {
            var expBatch = await Mediator.Send(new UpdateInvPRItemExpiryBatch() { Input = input, User = UserInfo() });
            if (expBatch.Id > 0)
            {
                return Created($"get/{expBatch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = expBatch.Message });
        }

        [HttpPost("createInvItemSerialBatch")]
        public async Task<ActionResult> CreateInvItemSerialBatch([FromBody] TblErpInvItemSerialBatchListDto input)
        {
            var serialBatch = await Mediator.Send(new CreateInvItemSerialBatch() { Input = input, User = UserInfo() });
            if (serialBatch.Id > 0)
            {
                return Created($"get/{serialBatch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = serialBatch.Message });
        }

        [HttpPost("createInvItemSpecification")]
        public async Task<ActionResult> CreateInvItemSpecification([FromBody] TblErpInvItemSpecificationDto input)
        {
            var specBatch = await Mediator.Send(new CreateInvItemSpecification() { Input = input, User = UserInfo() });
            if (specBatch.Id > 0)
            {
                return Created($"get/{specBatch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = specBatch.Message });
        }

        [HttpGet("GetExpairyDetails/{ItemCode}/{PoNumber}/{GrnNumber}")]
        public async Task<IActionResult> GetExpairyDetails([FromRoute] string ItemCode,string PoNumber,string GrnNumber)
        {
            var obj = await Mediator.Send(new GetExpairyDetails() { ItemCode = ItemCode, PoNumber = PoNumber,GrnNumber= GrnNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("GetPRExpairyDetails/{ItemCode}")]
        public async Task<IActionResult> GetPRExpairyDetails([FromRoute] string ItemCode)
        {
            var obj = await Mediator.Send(new GetPRExpairyDetails() { ItemCode = ItemCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
