const express = require('express');
const path = require('path');
const fs = require('fs');
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

router.post('/erc721', async (req, res, next) => {
  const bundleName = req.body['bundleName'];
  const [abi, bytecode] = await build(erc721Contract, 'eArtRegister');
  const address = await deploy(abi, bytecode, [bundleName]);
  res.send({ abi: abi, bytecode: bytecode, address: address, contract: depositContract });
});

router.post('/purchase', async (req, res, next) => {
  const erc721Address = req.body['erc721Address'];
  const tokenId = req.body['tokenId'];
  const minParticipation = req.body['minParticipation'];
  const [abi, bytecode] = await build(purchaseContract, 'Purchase');
  const address = await deploy(abi, bytecode, [erc721Address, tokenId, minParticipation]);
  res.send({ abi: abi, bytecode: bytecode, address: address, contract: depositContract });
});

function findImports(relativePath) {
  const absolutePath = path.resolve(__dirname, './../../node_modules', relativePath);
  const source = fs.readFileSync(absolutePath, 'utf8');
  return { contents: source };
}

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

  const {contracts} = JSON.parse(solc.compile(JSON.stringify(input), { import: findImports }));
  const compiledContract = contracts['contract'][contractName];
  const abi = compiledContract.abi;
  const bytecode = compiledContract.evm.bytecode.object;
  return [abi, bytecode];
}

async function deploy(abi, bytecode, argumentsArray) {
  const [account] = await web3.eth.getAccounts();
  const { _address } = await new web3.eth.Contract(abi)
    .deploy({ data: bytecode, arguments: argumentsArray })
    .send({from: account, gas: 3000000 });

  return _address;
}

module.exports = router;

const erc721Contract = `
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

contract eArtRegister is ERC721, ERC721URIStorage, Ownable {
    using Counters for Counters.Counter;

    Counters.Counter private _tokenIdCounter;

    constructor(string memory name) ERC721(name, "EART") {}

    function safeMint(address to, string memory uri) public onlyOwner {
        uint256 tokenId = _tokenIdCounter.current();
        _tokenIdCounter.increment();
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, uri);
    }

    function _burn(uint256 tokenId) internal override(ERC721, ERC721URIStorage) {
        super._burn(tokenId);
    }

    function tokenURI(uint256 tokenId)
        public
        view
        override(ERC721, ERC721URIStorage)
        returns (string memory)
    {
        return super.tokenURI(tokenId);
    }
}
`;

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

const purchaseContract = `
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";

interface IDeposit {
    function receiveBidMoney() payable external;
}

abstract contract APurchase {
    // methods
    function bid() virtual payable external;
    function closeBid() virtual public;
    function participate(address) virtual payable public;
    function payTheInstallment() virtual payable public;
    function purchase(address) virtual payable public;

    // views
    function getERC721ContractAddress() virtual public view returns(address);
    function getERC721TokenId() virtual public view returns(uint256);
    function getPrice() virtual public view returns(uint256);
    function getSeller() virtual public view returns(address);
    function getListedDate() virtual public view returns(uint);
    function getEndSellDate() virtual public view returns(uint);
    function getBiggestBid() virtual public view returns(address, uint256, uint);
}

contract Purchase is APurchase {
    // privates
    uint256 private balance;
    Listing private listing;
    bool private isPriceSet;
    bool private isSold;
    
    ERC721 token;
    address private erc721;
    uint256 private tokenId;

    uint private bidsCount;
    mapping (uint => Bid) private bids;
    Bid private newBid;
    address private maxBidder;
    uint256 private maxBid;
    uint private maxTimestamp;

    bool private isInstallmentPayed;
    uint256 private minParticipation;
    address private participator;
    uint private participated;
    uint private lastInstallmentPayed;

    // structs
    struct Listing {
        uint256 price;
        address seller;
        uint listedTimestamp;
        uint deadlineTimestamp;
    }

    struct Bid {
        address bidder;
        uint256 amount;
        uint bidTimestamp;
    }

    constructor(address _erc721, uint256 _tokenId, uint256 _minParticipation) {
        erc721 = _erc721;
        tokenId = _tokenId;
        isPriceSet = false;
        isInstallmentPayed = false;
        minParticipation = _minParticipation;
        token = ERC721(erc721);
    }

    function setPrice(uint256 amount, uint daysOnSale) 
            public 
            returns(bool) {
        require(isPriceSet == false, "Price is alredy been set!");
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        require(amount > 0, "Amount for sale must be greated that 0 (zero)");
        require(daysOnSale > 0, "NFT must be at least 1 (one) day on sale");

        listing = Listing(
                    amount, 
                    msg.sender, 
                    block.timestamp,
                    addDays(daysOnSale)
                );

        isPriceSet = true;

        return true;
    }

    function editPrice(uint256 amount) 
            public 
            returns(bool) {
        require(isPriceSet, "Price is not been set!");
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        require(amount > 0, "Amount for sale must be greated that 0 (zero)");

        require(checkDeadline(), "Deadline is passed");

        // TODO: check if there is participation

        listing.price = amount;
        emit PriceChanged(amount, block.timestamp);

        return true;
    }

    function editDeadline(uint moveDays) 
            public 
            returns(bool) {
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");

        require(checkDeadline(), "Deadline is passed");

        // TODO: check if there is participation

        uint prevDeadline = listing.deadlineTimestamp;
        listing.deadlineTimestamp = addDays(moveDays);
        emit DeadlineChanged(prevDeadline, listing.deadlineTimestamp, block.timestamp);

        return true;
    }

    // override methods
    function bid() 
            override 
            payable 
            external {
        newBid = Bid(
            msg.sender, 
            msg.value, 
            block.timestamp
        );
        
        bids[bidsCount] = newBid;
        bidsCount += 1;

        for (uint i = 0; i < bidsCount; i++) {
            if (maxBid < bids[i].amount) {
                maxBidder = bids[i].bidder;
                maxBid = bids[i].amount;
                maxTimestamp = bids[i].bidTimestamp;
            }
        }

        emit BidAdded(msg.sender, msg.value, block.timestamp);
    }

    function closeBid() 
            override 
            public {
        for (uint i = 0; i < bidsCount; i++) {
            address bidderAddress = bids[i].bidder;
            uint256 bidValue = bids[i].amount; 
            IDeposit(bidderAddress).receiveBidMoney{value: bidValue}();
        }
        bidsCount = 0;
    }

    function participate(address customer) 
            override 
            payable 
            public {
        require(msg.value >= minParticipation, "Insufficient funds");
        require(!isInstallmentPayed, "Installemnt is payed");
        isInstallmentPayed = true;
        require(participator == address(0), "Installemnt is payed");
        participator = customer;
        participated = block.timestamp;
        balance += msg.value;

        if (balance >= listing.price) {
            token.safeTransferFrom(listing.seller, participator, tokenId);
            payable(listing.seller).transfer(balance);
        }
    }

    function payTheInstallment() 
            override 
            payable 
            public {
        require(msg.sender == participator, "You are not the participator");
        require(isInstallmentPayed, "Installemnt is not payed");
        lastInstallmentPayed = block.timestamp;
        balance += msg.value;

        if (balance >= listing.price) {
            token.safeTransferFrom(listing.seller, participator, tokenId);
            payable(listing.seller).transfer(balance);
        }
    }

    function purchase(address customer) 
            override 
            payable 
            public {
        require(msg.value >= listing.price, "Insufficient funds");
        require(!isSold, "NFT is sold");

        token.safeTransferFrom(listing.seller, customer, tokenId);
        payable(listing.seller).transfer(msg.value);
        isSold = true;

        emit TokenPurhchased(customer, block.timestamp);

        closeBid();
    }

    // override views
    function getERC721ContractAddress() 
            override 
            public 
            view 
            returns(address) {
        return erc721;
    }

    function getERC721TokenId() 
            override 
            public 
            view 
            returns(uint256) {
        return tokenId;
    }

    function getPrice() 
            override 
            public 
            view 
            returns(uint256) {
        return listing.price;
    }

    function getSeller() 
            override 
            public
            view 
            returns(address) {
        return listing.seller;
    }

    function getListedDate() 
            override 
            public 
            view 
            returns(uint) {
        return listing.listedTimestamp;
    }

    function getEndSellDate() 
            override 
            public 
            view 
            returns(uint) {
        return listing.deadlineTimestamp;
    }

    function getBiggestBid() 
            override 
            public 
            view 
            returns(address, uint256, uint) {
        return(maxBidder, maxBid, maxTimestamp);
    }

    // private methods
    function addDays(uint numDays) 
            private 
            view 
            returns(uint256) {
        return (block.timestamp + (numDays * 24 * 60 * 60));
    }

    function checkDeadline() 
            private 
            view 
            returns(bool) {
        if (block.timestamp > listing.deadlineTimestamp) {
            return false;
        }
        else {
            return true;
        }
    }

    // events
    event PriceChanged(uint256 amount, uint timestamp);
    event DeadlineChanged(uint timestampFrom, uint timestampTo, uint timestamp);
    event TokenPurhchased(address customer, uint timestamp);
    event BidAdded(address bidder, uint256 amount, uint timestamp);
}
`;