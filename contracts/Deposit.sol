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