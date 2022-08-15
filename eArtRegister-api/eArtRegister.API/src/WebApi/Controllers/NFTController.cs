using eArtRegister.API.Application.NFTs.Commands.AddNFT;
using eArtRegister.API.Application.NFTs.Commands.Approved;
using eArtRegister.API.Application.NFTs.Commands.Bought;
using eArtRegister.API.Application.NFTs.Commands.GetNFTsByByndleId;
using eArtRegister.API.Application.NFTs.Commands.PrepareForSale;
using eArtRegister.API.Application.NFTs.Commands.SetOnSale;
using eArtRegister.API.Application.NFTs.Commands.TransferNFT;
using eArtRegister.API.Application.NFTs.Queries.GetNFTs;
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

        [HttpGet]
        [Route("mine/{wallet}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<NFTDto>>> NFTs(string wallet)
        {
            try
            {
                return await Mediator.Send(new GetNFTsQuery(wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        [HttpPost("setOnSale")]
        public async Task<IActionResult> SetOnSale(SetOnSaleCommand command)
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
        [HttpPost("bought")]
        public async Task<IActionResult> Bought(BoughtCommand command)
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
        [HttpPost("prepareForSale")]
        public async Task<IActionResult> PrepareForSale(PrepareForSaleCommand command)
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
        [HttpPost("approved")]
        public async Task<IActionResult> Approved(ApprovedCommand command)
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
