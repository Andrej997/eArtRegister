const express = require('express');
const solc = require('solc');
const Web3 = require('Web3');
const HDWalletProvider = require('@truffle/hdwallet-provider');
const mnemonic = 'season cross bless aspect speed suspect cross attitude income just link bomb'; 
const providerOrUrl = 'https://goerli.infura.io/v3/dce7e8edfaff4084ae2f60b36b8cee6e';
const provider = new HDWalletProvider({ mnemonic, providerOrUrl });
const web3 = new Web3(provider);
const router = express.Router();

router.post('/deposit', async (req, res, next) => {
  const ownerAddress = req.body['ownerAddress'];
  const [abi, bytecode] = await build(depositContract, 'Deposit');
  const address = await deploy(abi, bytecode, [ownerAddress]);
  res.send({ abi: abi, bytecode: bytecode, address: address, contract: depositContract });
});

async function build(contract, contractName) {
  const input = {
    language: 'Solidity',
    sources: {
      'contract' : {
          content: contract
      }
    },
    settings: {
      outputSelection: { '*': { '*': ['*'] } }
    }
  };

  const {contracts} = JSON.parse(solc.compile(JSON.stringify(input)));
  const compiledContract = contracts['contract'][contractName];
  const abi = compiledContract.abi;
  const bytecode = compiledContract.evm.bytecode.object;

  return [abi, bytecode];
}

async function deploy(abi, bytecode, arguments) {
  const [account] = await web3.eth.getAccounts();
  const { _address } = await new web3.eth.Contract(abi)
    .deploy({ data: bytecode, arguments: arguments })
    .send({from: account, gas: 1000000 });

  return _address;
}

module.exports = router;

const depositContract = `
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

interface IPurchase {
    function bid() payable external;
    function participate(uint256) external;
    function purchase(address) payable external;
}

contract Deposit {
    address internal owner;
    address internal server;
    uint256 internal balance;

    constructor(address _owner) {    
        owner = _owner;  
        server = msg.sender;
    }

    function deposit() public payable {
        require(msg.sender == owner, "Only owner can deposit");
        balance += msg.value;
    }

    function viewDeposit() public view returns(uint256) {
        require(msg.sender == owner, "You are not the owner of this deposit");
        return balance;
    }

    function withdraw(address payable _owner, uint256 amount) external {
        require(owner == _owner, "Not owner");
        require(msg.sender == _owner, "Not owner");
        require(msg.sender == owner, "Not owner");
        _owner.transfer(amount);
        balance -= amount;
    }

    function serverWithdraw(uint256 amount, address payable _server) external {
        require(server == _server, "Not server");
        require(msg.sender == _server, "Not server");
        require(msg.sender == server, "Not server");
        _server.transfer(amount);
        balance -= amount;
    }

    function sendBid(address _auction, uint256 _amount) external {
        uint256 transferableAmount = address(this).balance -(address(this).balance - _amount);
        IPurchase(_auction).bid{value: transferableAmount}();
    }

    function receiveBidMoney() payable external {
        balance += msg.value;
    }

    function purchase(address _purchaseContract, uint256 _amount) external {
        uint256 transferableAmount = address(this).balance -(address(this).balance - _amount);
        IPurchase(_purchaseContract).purchase{value: transferableAmount}(owner);
    }
}
`;