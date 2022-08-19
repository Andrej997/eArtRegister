using eArtRegister.API.Application.Users.Commands.CheckWallet;
using eArtRegister.API.Application.Users.Commands.CreateDeposit;
using eArtRegister.API.Application.Users.Commands.Deposit;
using eArtRegister.API.Application.Users.Commands.RequestRolePermission;
using eArtRegister.API.Application.Users.Queries.GetUser;
using eArtRegister.API.Application.Users.Queries.GetUserHistoryActions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet("getUser/{wallet}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> GetUser(string wallet)
        {
            try
            {
                return await Mediator.Send(new GetUserQuery(wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checkWallet")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CheckWallet(CheckWalletCommand command)
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

        [HttpGet("actions/{wallet}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<ActionHistoryDto>>> GetUserHistoryActions(string wallet)
        {
            try
            {
                return await Mediator.Send(new GetUserHistoryActionsQuery(wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("createDeposit")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateDeposit(CreateDepositCommand command)
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

        [HttpPost("deposit")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> Deposit(DepositCommand command)
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