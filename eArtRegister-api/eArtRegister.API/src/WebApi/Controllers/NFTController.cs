using eArtRegister.API.Application.NFTs.Commands.AddNFT;
using eArtRegister.API.Application.NFTs.Commands.Approved;
using eArtRegister.API.Application.NFTs.Commands.Bought;
using eArtRegister.API.Application.NFTs.Commands.Cancel;
using eArtRegister.API.Application.NFTs.Commands.CreatePurchase;
using eArtRegister.API.Application.NFTs.Commands.PrepareForSale;
using eArtRegister.API.Application.NFTs.Commands.SetOnSale;
using eArtRegister.API.Application.NFTs.Commands.WithdrawFunds;
using eArtRegister.API.Application.NFTs.Queries.GetNFTs;
using eArtRegister.API.Application.NFTs.Queries.GetNFTsByByndleId;
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
        [HttpGet("{customRoute}")]
        public async Task<ActionResult<List<NFTDto>>> GetNFTsByByndleId(string customRoute)
        {
            try
            {
                return await Mediator.Send(new GetNFTsByByndleIdCommand(customRoute));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpGet("{customRoute}/{tokenId}")]
        public async Task<ActionResult<List<NFTDto>>> GetNFTsByName(string customRoute, long tokenId)
        {
            try
            {
                return await Mediator.Send(new GetNFTsByByndleIdCommand(customRoute, tokenId));
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
        public async Task<ActionResult<Guid>> AddNFT(IFormFile file, [FromQuery] AddNFTData data)
        {
            if (file == null)
            {
                return BadRequest("Missing NFT file");
            }

            try
            {
                return await Mediator.Send(new AddNFTCommand
                {
                    Name = data.Name,
                    Description = data.Description,
                    CustomRouth = data.CustomRouth,
                    Wallet = data.Wallet,
                    File = file.GetUploadFileModel(),
                    AttributeKeys = data.AttributeKeys,
                    AttributeValues = data.AttributeValues,
                    ExternalUrl = data.ExternalUrl,
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("createPurchase")]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseCommand command)
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
        [HttpPost("withdrawFunds")]
        public async Task<IActionResult> WithdrawFunds(WithdrawFundsCommad command)
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
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelNFT(CancelNFTCommand command)
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
