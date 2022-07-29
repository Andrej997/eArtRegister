using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.Application.Users.Queries.GetUserIds;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace eArtRegister.API.WebApi.Controllers
{
    [ApiKey]
    public class UserWorkerController : ApiControllerBase
    {
        [HttpGet("ids")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<Guid>>> GetUserIds()
        {
            try
            {
                return await Mediator.Send(new GetUserIdsQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
