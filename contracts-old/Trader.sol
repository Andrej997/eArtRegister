// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";

contract Trader {

    address contractAddress;
    uint256 tokenId;
    address server;

    constructor(address _contractAddress, uint256 _tokenId, address _server) {
        contractAddress = _contractAddress;
        tokenId = _tokenId;
        server = _server;
        isStopped = false;
    }

    mapping(address => uint256) public balance;

    Listing public listing;
    Buyer public buyer;
    bool public isStopped; 

    struct Listing {
        uint256 price;
        address seller;
        uint256 minParticipation;
        uint daysToPay;
        uint listed;
    }

    struct Buyer {
        uint256 participation;
        address buyer;
        uint participated;
    }

    function setPrice(uint256 price, uint256 minParticipation, uint256 daysToPay) public {
        require(isStopped == false, "The contract has expired!");
        ERC721 token = ERC721(contractAddress);
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        listing = Listing(price, msg.sender, minParticipation, daysToPay, block.timestamp);
    }
    
    function purchase() public payable {
        require(isStopped == false, "The contract has expired!");
        require(msg.value > 0, "You can't send 0!");
        if (buyer.buyer == address(0)) {
            require(msg.value >= listing.minParticipation, "You need to send at least the minimum defined participation!");
            buyer = Buyer(msg.value, msg.sender, block.timestamp);
            balance[listing.seller] += msg.value;
            if (balance[listing.seller] >= listing.price) {
                ERC721 token = ERC721(contractAddress);
                token.safeTransferFrom(listing.seller, msg.sender, tokenId);
                isStopped = true;
            }
        }
        else if (buyer.buyer != address(0)) {
            require(buyer.buyer == msg.sender, "You are not the buyer!");
            balance[listing.seller] += msg.value;
            if (balance[listing.seller] >= listing.price) {
                ERC721 token = ERC721(contractAddress);
                token.safeTransferFrom(listing.seller, msg.sender, tokenId);
                isStopped = true;
            }
        }
    }

    function withdraw(address payable sellerAddr) public {
        require(isStopped == true, "The contract needs to be expired!");
        require(sellerAddr == msg.sender, "You can't send to other address!");
        require(listing.seller == msg.sender, "You are not the seller!");
        require(listing.seller == sellerAddr, "Address is not the address of the seller!");

        sellerAddr.transfer(balance[listing.seller]);
        balance[listing.seller] = 0;
    }

    function sellerStop(address payable buyerAddr, address payable serverAddr) public payable {
        require(isStopped == false, "The contract has expired!");
        require(listing.seller == msg.sender, "You are not the seller!");
        require(buyer.buyer != address(0), "There is no buyer!");
        require(buyer.buyer == buyerAddr, "That is not the buyer!");
        require(server == serverAddr, "That is not the server!");

        uint diff = (buyer.participated - block.timestamp) / 60 / 60 / 24;
        if (diff >= listing.daysToPay) {
            isStopped = true;
        }
        else {
            uint256 forServer = (listing.minParticipation / 100) * 50;
            require(msg.value >= forServer, "You need to send 50% od minimun defined participation to server");

            serverAddr.transfer(msg.value);

            buyerAddr.transfer(balance[listing.seller]);

            isStopped = true;
            balance[listing.seller] = 0;
        }
    }

    function buyerRequestToStopBuy(address payable buyerAddr) public payable {
        require(isStopped == false, "The contract has expired!");
        require(buyer.buyer == msg.sender, "You are not the buyer!");

        uint diff = (buyer.participated - block.timestamp) / 60 / 60 / 24;
        if (diff >= listing.daysToPay) {
            isStopped = true;
        }
        else {
            uint256 retVal = (balance[listing.seller] / 100) * 10;
            buyerAddr.transfer(retVal);
            isStopped = true;

            balance[listing.seller] -= retVal;
        }
    }
}