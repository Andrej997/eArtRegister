using eArtRegister.API.Application.NFTs.Commands.AddNFT;
using eArtRegister.API.Application.NFTs.Commands.ChangeStatus;
using eArtRegister.API.Application.NFTs.Commands.GetNFTsByByndleId;
using eArtRegister.API.Application.NFTs.Commands.TransferNFT;
using eArtRegister.API.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class NFTController : ApiControllerBase
    {
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpGet("bundle/{bundleId}")]
        public async Task<ActionResult<List<NFTDto>>> GetNFTsByByndleId(Guid bundleId)
        {
            try
            {
                return await Mediator.Send(new GetNFTsByByndleIdCommand
                {
                    BundleId = bundleId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddNFT(IFormFile file, string name, string description, Guid bundleId, double price, double royality, string wallet)
        {
            if (file == null)
            {
                return BadRequest("Missing NFT file");
            }

            try
            {
                return await Mediator.Send(new AddNFTCommand
                {
                    Name = name,
                    Description = description,
                    BundleId = bundleId,
                    Price = price,
                    Royality = royality,
                    Wallet = wallet,
                    File = file.GetUploadFileModel()
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("sendAsGift")]
        public async Task<IActionResult> TransferNFT(TransferNFTCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPut("changeStatus")]
        public async Task<IActionResult> ChangeStatus(ChangeStatusCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
