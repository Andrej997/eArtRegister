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
    function closeBid() virtual public;
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
    // public
    bool entireAmount;
    bool repaymentInInstallments;
    bool auction;

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
            override 
            public {
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