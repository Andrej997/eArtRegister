using eArtRegister.API.Application.Bundles.Commands.AddNFTToBundle;
using eArtRegister.API.Application.Bundles.Commands.CreateBundle;
using eArtRegister.API.Application.Bundles.Queries.GetBundles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    public class BundleController : ApiControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<BundleDto>>> GetBundles()
        {
            try
            {
                return await Mediator.Send(new GetBundlesQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<BundleDto>> GetBundle(Guid id)
        {
            try
            {
                return await Mediator.Send(new GetBundleQuery(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("search")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<BundleDto>>> SearchBundles(string search)
        {
            try
            {
                return await Mediator.Send(new GetBundlesQuery(search));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("mine/{wallet}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<BundleDto>>> MineBundles(string wallet)
        {
            try
            {
                return await Mediator.Send(new GetBundlesQuery(true, wallet: wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<Guid>> CreateBundle(CreateBundleCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("addNFT")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<Guid>> AddNFT(AddNFTToBundleCommand command)
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
