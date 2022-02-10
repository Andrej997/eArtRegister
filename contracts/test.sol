pragma solidity ^0.8.11;

contract test {
    
    int _multiplier;

    constructor(int multiplier) public {
        _multiplier = multiplier;
    }

    function multiply(int val) public returns (int d) {
        return val * _multiplier;    
    }
}