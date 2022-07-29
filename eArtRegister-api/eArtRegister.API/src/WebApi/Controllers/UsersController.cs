﻿using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.Application.Users.Queries.GetUserIds;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    [ApiKey]
    public class UsersController : ApiControllerBase
    {
        [HttpPost("requestRolePermission")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> RequestRolePermission(RequestRolePermissionCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}