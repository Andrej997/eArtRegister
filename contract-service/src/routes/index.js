const express = require('express');
const path = require('path');
const fs = require('fs');
const solc = require('solc');
const Web3 = require('Web3');
const HDWalletProvider = require('@truffle/hdwallet-provider');
const publicAddress = "0xdbAB2059F593D6Aa5c6c50A973ceE919c70A4d91";
const mnemonic = 'season cross bless aspect speed suspect cross attitude income just link bomb'; 
const providerOrUrl = 'https://goerli.infura.io/v3/dce7e8edfaff4084ae2f60b36b8cee6e';
const provider = new HDWalletProvider({ mnemonic, providerOrUrl });
const web3 = new Web3(provider);
const router = express.Router();

router.post('/deposit', async (req, res, next) => {
    const owner = req.body['owner'];

    const [abi, bytecode] = await build(depositContract, 'Deposit');
    const address = await deploy(abi, bytecode, [owner]);

    res.send({ abi: abi, bytecode: bytecode, address: address, contract: depositContract });
});

router.post('/erc721', async (req, res, next) => {
    const owner = req.body['owner'];
    const bundleName = req.body['bundleName'];
    const bundleSymbol = req.body['bundleSymbol'];

    const [abi, bytecode] = await build(erc721Contract, 'eArtRegister');
    const address = await deploy(abi, bytecode, [bundleName, bundleSymbol, owner]);

    res.send({ abi: JSON.stringify(abi), bytecode: bytecode, address: address, contract: erc721Contract });
});

router.post('/purchase', async (req, res, next) => {
    const erc721Address = req.body['erc721Address'];
    const tokenId = req.body['tokenId'];
    const entireAmount = req.body['entireAmount'];
    const repaymentInInstallments = req.body['repaymentInInstallments'];
    const auction = req.body['auction'];

    const [abi, bytecode] = await build(purchaseContract, 'Purchase');
    const address = await deploy(abi, bytecode, [erc721Address, tokenId, entireAmount, repaymentInInstallments, auction]);

    res.send({ abi: abi, bytecode: bytecode, address: address, contract: purchaseContract });
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
    address private _owner;

    constructor(string memory name, string memory symbol, address owner) ERC721(name, symbol) {
        _owner = owner;
    }

    function safeMint(address to, string memory uri) public {
        require(_owner == msg.sender, "Caller is not the owner");
        uint256 tokenId = _tokenIdCounter.current();
        _tokenIdCounter.increment();
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, uri);
    }

    function _burn(uint256 tokenId) internal override(ERC721, ERC721URIStorage) {
        super._burn(tokenId);
    }

    function tokenURI(uint256 tokenId) public view override(ERC721, ERC721URIStorage) returns (string memory) {
        return super.tokenURI(tokenId);
    }
}
`;

const depositContract = `
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

interface IPurchase {
    function bid() payable external;

    function participate(address) payable external;
    function payTheInstallment(address customer) payable external;

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

    function participate(address _purchaseContract, uint256 _amount) external {
        uint256 transferableAmount = address(this).balance -(address(this).balance - _amount);
        IPurchase(_purchaseContract).participate{value: transferableAmount}(owner);
    }

    function payTheInstallment(address _purchaseContract, uint256 _amount) external {
        uint256 transferableAmount = address(this).balance -(address(this).balance - _amount);
        IPurchase(_purchaseContract).payTheInstallment{value: transferableAmount}(owner);
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
    function setPrice(uint256 amount, uint daysOnSale, uint256 participation) virtual public returns(bool);
    function editPrice(uint256 amount) virtual public returns(bool);
    function editDeadline(uint moveDays) virtual public returns(bool);
    function bid() virtual payable external;
    function participate(address) virtual payable public;
    function payTheInstallment(address) virtual payable public;
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
    // internal
    bool internal entireAmount;
    bool internal repaymentInInstallments;
    bool internal auction;

    // private
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

    Installemnt private installemntCustomer;

    // structs
    struct Listing {
        uint256 price;
        address seller;
        uint listedTimestamp;
        uint deadlineTimestamp;
        uint256 participation;
    }

    struct Installemnt {
        uint256 amountPayed;
        address buyer;
        uint lastInstallemnt;
    }

    struct Bid {
        address bidder;
        uint256 amount;
        uint bidTimestamp;
    }

    constructor(address _erc721, uint256 _tokenId, bool _entireAmount, bool _repaymentInInstallments, bool _auction) {
        erc721 = _erc721;
        tokenId = _tokenId;
        isPriceSet = false;
        isSold = false;
        token = ERC721(erc721);
        entireAmount = _entireAmount;
        repaymentInInstallments = _repaymentInInstallments;
        auction = _auction;
    }

    function setPrice(uint256 amount, uint daysOnSale, uint256 participation) 
            override
            public 
            returns(bool) {
        require(!isPriceSet, "Price is alredy been set!");
        require(!isSold, "NFT is sold!");

        if (participation > 0)
            require(repaymentInInstallments, "Action not available");
        
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        require(amount > 0, "Amount for sale must be greated that 0 (zero)");
        require(daysOnSale > 0, "NFT must be at least 1 (one) day on sale");

        listing = Listing(
                    amount, 
                    msg.sender, 
                    block.timestamp,
                    addDays(daysOnSale),
                    participation
                );

        isPriceSet = true;

        return true;
    }

    function editPrice(uint256 amount) 
            override
            public 
            returns(bool) {
        require(!isSold, "NFT is sold!");
        require(isPriceSet, "Price is not been set!");

        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        require(amount > 0, "Amount for sale must be greated that 0 (zero)");

        require(checkDeadline(), "Deadline is passed");

        require(installemntCustomer.amountPayed == 0, "Installment payments have already started!");

        listing.price = amount;
        emit PriceChanged(amount, block.timestamp);

        return true;
    }

    function editDeadline(uint moveDays) 
            override
            public 
            returns(bool) {
        require(!isSold, "NFT is sold!");
        require(isPriceSet, "Price is not been set!");

        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");

        require(checkDeadline(), "Deadline is passed");

        require(installemntCustomer.amountPayed == 0, "Installment payments have already started!");

        uint prevDeadline = listing.deadlineTimestamp;
        listing.deadlineTimestamp = addDays(moveDays);
        emit DeadlineChanged(prevDeadline, listing.deadlineTimestamp, block.timestamp);

        return true;
    }

    // auction methods
    function bid() 
            override 
            payable 
            external {
        require(!isSold, "NFT is sold!");

        require(auction, "Action not available");

        require(installemntCustomer.amountPayed == 0, "Participation is payed");

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
            private {
        require(!isSold, "NFT is sold!");

        for (uint i = 0; i < bidsCount; i++) {
            address bidderAddress = bids[i].bidder;
            uint256 bidValue = bids[i].amount; 
            IDeposit(bidderAddress).receiveBidMoney{value: bidValue}();
        }
        bidsCount = 0;
    }

    // installment methods
    function participate(address customer) 
            override 
            payable 
            public {
        require(!isSold, "NFT is sold!");

        require(repaymentInInstallments, "Action not available");

        require(msg.value >= listing.participation, "Insufficient funds");
        require(installemntCustomer.amountPayed == 0, "Participation is payed");
        require(installemntCustomer.buyer == address(0), "Installemnt is payed");

        installemntCustomer = Installemnt(
                    msg.value, 
                    customer, 
                    block.timestamp
                );

        if (installemntCustomer.amountPayed >= listing.price) {
            token.safeTransferFrom(listing.seller, installemntCustomer.buyer, tokenId);
            payable(listing.seller).transfer(installemntCustomer.amountPayed);
        }

        closeBid();
    }

    function payTheInstallment(address customer) 
            override 
            payable 
            public {
        require(!isSold, "NFT is sold!");

        require(repaymentInInstallments, "Action not available");

        require(customer == installemntCustomer.buyer, "You are not the participator");
        require(installemntCustomer.amountPayed > 0, "Installemnt is not payed");
        installemntCustomer.lastInstallemnt = block.timestamp;
        installemntCustomer.amountPayed += msg.value;

        if (installemntCustomer.amountPayed >= listing.price) {
            token.safeTransferFrom(listing.seller, installemntCustomer.buyer, tokenId);
            payable(listing.seller).transfer(installemntCustomer.amountPayed);
        }
    }

    // full price methods
    function purchase(address customer) 
            override 
            payable 
            public {
        require(!isSold, "NFT is sold!");
        require(entireAmount, "Action not available");

        require(installemntCustomer.amountPayed == 0, "Participation is payed");
        require(msg.value >= listing.price, "Insufficient funds");

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

router.post('/safeMint', async (req, res, next) => {
    const abi = req.body['abi'];
    const address = req.body['address'];
    const to = req.body['to'];
    const uri = req.body['uri'];

    const daiToken = new web3.eth.Contract(abi, address);
    daiToken.methods.safeMint(to, uri)
        .send({ from: publicAddress }, function (err, transactionHash) {
            if (err) {
                console.log("An error occured", err);
                res.send({ error: "Failed to mint" });
            }
            console.log("Hash of the transaction:" + transactionHash);
            res.send({ transactionHash: transactionHash });
    });
});

router.post('/ownerOf', async (req, res, next) => {
    const abi = req.body['abi'];
    const address = req.body['address'];
    const tokenId = req.body['tokenId'];

    const daiToken = new web3.eth.Contract(abi, address);

    daiToken.methods.ownerOf(tokenId)
        .call(function (err, owner) {
            if (err) {
                console.log("An error occured", err);
                return
            }
            console.log("Owner of is:", owner);
            res.send({ owner: owner });
    });
});

router.post('/tokenURI', async (req, res, next) => {
    const abi = req.body['abi'];
    const address = req.body['address'];
    const tokenId = req.body['tokenId'];

    const daiToken = new web3.eth.Contract(abi, address);

    daiToken.methods.tokenURI(tokenId)
        .call(function (err, uri) {
            if (err) {
                console.log("An error occured", err);
                return
            }
            console.log("Token URI is:", uri);
            res.send({ uri: uri });
    });
});

router.post('/setApprovalForAll', async (req, res, next) => {
    const abi = req.body['abi'];
    const address = req.body['address'];
    const operator = req.body['operator'];

    const daiToken = new web3.eth.Contract(abi, address);

    daiToken.methods.setApprovalForAll(operator, true)
        .send({ from: publicAddress }, function (err, transactionHash) {
            if (err) {
                console.log("An error occured", err);
                res.send({ error: "Failed to mint" });
            }
            console.log("Hash of the transaction:" + transactionHash);
            res.send({ transactionHash: transactionHash });
    });
});