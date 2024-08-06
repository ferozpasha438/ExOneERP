using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class PvAddResourceController : BaseController
    {

        public PvAddResourceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPvAddResourceReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvAddResourceReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InputPvAddResourceReq dTO)
        {
            var id = await Mediator.Send(new CreatePvAddResourceReq() { PvAddResourceReqDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
           
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpGet("getPvAddResourceReqById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPvAddResourceReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("mergePvAddResourceReqById/{id}")]
        public async Task<IActionResult> MergePvAddResourceReqById([FromRoute] long id)
        {

            var Req = await Mediator.Send(new GetPvAddResourceReqById() { Id = id, User = UserInfo() });
            if (Req.Id<=0) {
                return NotFound(new ApiMessageDto { Message = "No_Request_Found" });
            }
            var ResMap = await Mediator.Send(new GetPvAddResourceEmpToResMapsByReqIdById() { Id=Req.Id});
            if (ResMap.Count == 0 )
            {
                return NotFound(new ApiMessageDto { Message = "No_Resource_MappingsFound" });
            }
            InputPvAddResourceEmployeeToResourceMap List = new() { MappingsList=ResMap};
            

            var isValid = await Mediator.Send(new IsValidPvAddResourceEmployeeToResourceMap(){ InputDto = List, User = UserInfo() });


            if (!isValid.IsValidReq)
            {
                return BadRequest(new ApiMessageDto { Message = isValid.ErrorMsg });

            }






            var obj = await Mediator.Send(new MergePvAddResourceReqById() { Id = id, User = UserInfo() });

            if (obj is not null)
            {
                if (obj.Id > 0)
                    return Ok(obj);
                else if (obj.Id == -1)
                {
                    return NotFound(new ApiMessageDto { Message = "Request_not_exist" });
                }
                else if (obj.Id == -2)
                {
                    return NotFound(new ApiMessageDto { Message = "Request_Not_Approved" });
                }
                else if (obj.Id == -3)
                {
                    return NotFound(new ApiMessageDto { Message = "invalid_projec_code" });
                }
                else if (obj.Id == -4)
                {
                    return NotFound(new ApiMessageDto { Message = "No_Reource_Mapping_Exist" });
                }

                else if (obj.Id == -5)
                {
                    return NotFound(new ApiMessageDto { Message = "Invalid_Project_Code" });
                }
                else if (obj.Id == -6)
                {
                    return NotFound(new ApiMessageDto { Message = "Shift_Plan_Not_Exist_for_Project" });
                }
                else if (obj.Id == -7)
                {
                    return NotFound(new ApiMessageDto { Message = "No_Off_Shifts_Found_in_Site" });
                }
                else if (obj.Id == -8)
                {
                    return NotFound(new ApiMessageDto { Message = "Incompatible_Dates_with_project_Dates" });
                }

                else if (obj.Id == -9)
                {
                    return NotFound(new ApiMessageDto { Message = "Employee_Already_Exists" });
                }
            }

            return NotFound(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }






            [HttpGet("approveReqPvAddResourceReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvAddResourceReqById([FromRoute] long id)
        {

            var obj = await Mediator.Send(new ApproveReqPvAddResourceReqById() { Id = id, User = UserInfo() });

            if (obj is not null)
            {
                if (obj.Id > 0)
                    return Ok(obj);
                else if (obj.Id==-1)
                {
                    return NotFound(new ApiMessageDto { Message = "Request_not_exist" }); 
                }
                 else if (obj.Id==-2)
                {
                    return NotFound(new ApiMessageDto { Message = "Request_Already_Approved" });
                }
else if (obj.Id==-3)
                {
                    return NotFound(new ApiMessageDto { Message = "invalid_project_code" });
                }
              
                  
                 
            }

            return NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });

        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var obj = await Mediator.Send(new DeletePvAddResourceReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        [HttpPost("CreateUpdatePvAddResourceEmployeeToResourceMap")]
        public async Task<ActionResult> CreateUpdatePvAddResourceEmployeeToResourceMap([FromBody] InputPvAddResourceEmployeeToResourceMap dTO)
        {

            var MapReq= await Mediator.Send(new IsValidPvAddResourceEmployeeToResourceMap(){  InputDto=dTO,User=UserInfo()});


            if (MapReq.IsValidReq)
            {
                var id = await Mediator.Send(new CreateUpdatePvAddResourceEmployeeToResourceMap() { PvAddResourceEmpToResMaps = dTO, User = UserInfo() });
                if (id > 0)
                {
                    return Created($"get/{id}", dTO);
                }
                else if (id == -1)
                {
                    return BadRequest(new ApiMessageDto { Message = "Empty Mappings" });
                }
                else if (id == -2)
                {
                    return BadRequest(new ApiMessageDto { Message = "invalid request Id" });
                } else if (id == -3)
                {
                    return BadRequest(new ApiMessageDto { Message = "invalid ProjectSite" });
                }
            }

            else if(MapReq.ErrorId<0){
                return BadRequest(new ApiMessageDto { Message =MapReq.ErrorMsg});

            }


            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });





        }


        [HttpGet("getPvAddResourceEmpToResMapsByReqIdById/{id}")]
        public async Task<IActionResult> GetPvAddResourceEmpToResMapsByReqIdById([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetPvAddResourceEmpToResMapsByReqIdById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



    }
}
