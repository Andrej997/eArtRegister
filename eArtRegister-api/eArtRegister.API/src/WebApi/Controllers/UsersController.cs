using eArtRegister.API.Application.Users.Commands.Login;
using eArtRegister.API.Application.Users.Commands.Register;
using eArtRegister.API.Application.Users.Queries.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task Test()
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var abi = @"[{""inputs"":[],""name"":""get"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""}]";
            var byteCode = "0x6080604052348015600f57600080fd5b50607780601d6000396000f3fe6080604052348015600f57600080fd5b506004361060285760003560e01c80636d4ce63c14602d575b600080fd5b600860405190815260200160405180910390f3fea2646970667358221220b82df5e4c342f8f5343cf0594fde4d13b0f0bfdc39969c4cdf65e402b21ff24164736f6c634300080b0033";
            var web3 = new Web3();
            try
            {
                var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 120);
                Assert.IsTrue(unlockAccountResult);
                var trasactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, gas: new HexBigInteger(100000));
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasactionHash);
                while (receipt == null)
                {
                    Thread.Sleep(5000);
                    receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasactionHash);
                }
                var contractAddress = receipt.ContractAddress;

                var contract = web3.Eth.GetContract(abi, contractAddress);

                var callFunction = contract.GetFunction("get");

                var result = await callFunction.CallAsync<int>();

                Assert.IsEquals(8, result);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        [Route("search")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<UserDto>>> Search(SearchQuerry querry)
        {
            try
            {
                return await Mediator.Send(querry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> Register(RegisterCommand command)
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
        [Route("login")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> Login(LoginCommand command)
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
