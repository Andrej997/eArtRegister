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

        [HttpGet("{customRoot}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<BundleDto>> GetBundle(string customRoot)
        {
            try
            {
                return await Mediator.Send(new GetBundleQuery(customRoot));
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
        public async Task<ActionResult<RetBundle>> CreateBundle(CreateBundleCommand command)
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
    }
}
