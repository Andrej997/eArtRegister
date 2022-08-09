// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";

contract Trader {
    mapping(address => mapping(uint256 => Listing)) public listings;
    mapping(address => uint256) public balances;

    struct Listing {
        uint256 price;
        address seller;
    }

    function addListing(uint256 price, address contractAddress, uint256 tokenId) public {
        ERC721 token = ERC721(contractAddress);
        require(msg.sender == token.ownerOf(tokenId), "Caller must own given token");
        require(token.isApprovedForAll(msg.sender, address(this)), "Contract must be approved!");
        listings[contractAddress][tokenId] = Listing(price, msg.sender);
    }

    function purchase(address contractAddress, uint256 tokenId) public payable {
        Listing memory item = listings[contractAddress][tokenId];
        require(msg.value >= item.price, "Insufficient funds sent");
        balances[item.seller] += msg.value;

        ERC721 token = ERC721(contractAddress);
        token.safeTransferFrom(item.seller, msg.sender, tokenId);
    }

    function withdraw(uint256 amount, address payable destAddr) public {
        require(amount <= balances[msg.sender], "insufficient funds");

        destAddr.transfer(amount);
        balances[msg.sender] -= amount;
    }
}