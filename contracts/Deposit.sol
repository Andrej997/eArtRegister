// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

contract Deposit {

    mapping(address => uint256) public balances;

    function deposit() public payable {
        balances[msg.sender] += msg.value;
    }

    function withdraw(address payable destAddr) public payable {
        destAddr.transfer(msg.value);
        balances[msg.sender] -= msg.value;
    }
}