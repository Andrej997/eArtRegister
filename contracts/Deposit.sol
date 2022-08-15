// SPDX-License-Identifier: MIT
pragma solidity ^0.8.4;

contract Deposit {

    address owner;
    address server;
    mapping(address => uint256) public balances;

    constructor(address _owner) {    
        owner = _owner;  
        server = msg.sender;
    }

    function deposit() public payable {
        balances[owner] += msg.value;
    }

    function withdraw(address payable _server) external payable {
        require(server == _server, "Not server");
        require(msg.sender == _server, "Not server");
        require(msg.sender == server, "Not server");
        _server.transfer(msg.value);
        balances[owner] -= msg.value;
    }
}