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