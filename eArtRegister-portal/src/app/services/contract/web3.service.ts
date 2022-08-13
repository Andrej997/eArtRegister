import {Inject, Injectable} from '@angular/core';
import { WEB3 } from '../../core/web3';
import { Subject } from 'rxjs';
import Web3 from 'web3';
import Web3Modal from "web3modal";
import WalletConnectProvider from "@walletconnect/web3-provider";
import { provider } from 'web3-core';

let tokenAbi = require('./transferAbi.json');
let erc271Abi = require('./erc271Abi.json');

@Injectable({
  providedIn: 'root'
})

@Injectable({
  providedIn: 'root'
})
export class Web3Service {
  public accountsObservable = new Subject<string[]>();
  web3Modal;
  web3js:  any;
  provider: provider | undefined;
  accounts: string[] | undefined;
  balance: string | undefined;

  constructor(@Inject(WEB3) private web3: Web3) {
    const providerOptions = {
      walletconnect: {
        package: WalletConnectProvider, // required
        options: {
          infuraId: 'env', // required change this with your own infura id
          description: 'Scan the qr code and sign in',
          qrcodeModalOptions: {
            mobileLinks: [
              'rainbow',
              'metamask',
              'argent',
              'trust',
              'imtoken',
              'pillar'
            ]
          }
        }
      },
      injected: {
        display: {
          logo: 'https://upload.wikimedia.org/wikipedia/commons/3/36/MetaMask_Fox.svg',
          name: 'metamask',
          description: "Connect with the provider in your Browser"
        },
        package: null
      },
    };

    this.web3Modal = new Web3Modal({
      network: "mainnet", // optional change this with the net you want to use like rinkeby etc
      cacheProvider: true, // optional
      providerOptions, // required
      theme: {
        background: "rgb(39, 49, 56)",
        main: "rgb(199, 199, 199)",
        secondary: "rgb(136, 136, 136)",
        border: "rgba(195, 195, 195, 0.14)",
        hover: "rgb(16, 26, 32)"
      }
    });
  }


  async connectAccount() {
    this.provider = await this.web3Modal.connect(); // set provider
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } // create web3 instance
    this.accounts = await this.web3js.eth.getAccounts();
    return this.accounts;
  }

  async accountInfo(account: any[]){
    const initialvalue = await this.web3js.eth.getBalance(account);
    this.balance = this.web3js.utils.fromWei(initialvalue , 'ether');
    return this.balance;
  }

  private _tokenContract: any;
  private _tokenContractAddress: string = "0x46F0e4574c5A1cB78bAD2144704c247f2d38A365";

  async setNftOnSale(): Promise<number> {
    this.provider = await this.web3Modal.connect(); // set provider
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } // create web3 ins
    this.accounts = await this.web3js.eth.getAccounts();
    console.log(this.accounts);
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, this._tokenContractAddress);

    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._tokenContract.methods.addListing(1000000000, '0xBF656629698C7aD1b7d060650811556d1Fb80055', 1).send({from: (this.accounts as string[])[0], gas: 3000000},function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
  
        // resolve(_web3.fromWei(result));
      });
    }) as Promise<number>;
  }

  

  async purchaseNft(): Promise<number> {
    this.provider = await this.web3Modal.connect(); // set provider
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } // create web3 ins
    this.accounts = await this.web3js.eth.getAccounts();
    console.log(this.accounts);
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, this._tokenContractAddress);
    
    
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._tokenContract.methods.purchase('0xBF656629698C7aD1b7d060650811556d1Fb80055', 1).send({from: (this.accounts as string[])[0], gas: 3000000, value: 1000000000}, function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
      });
    }) as Promise<number>;
  }

  private _erc271Contract: any;
  private _erc271ContractAddress: string = "0xBF656629698C7aD1b7d060650811556d1Fb80055";

  async sendNftAsGift(contractAddress: string, from: string, to: string, tokenId: number): Promise<number> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    console.log(this.accounts);
    this._erc271Contract = await new this.web3js.eth.Contract(erc271Abi, contractAddress);
    
    
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._erc271Contract.methods.safeTransferFrom(from, to, tokenId).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
  
        resolve((result));
      });
    }) as Promise<number>;
  }

  public async getUserBalance(): Promise<number> {

    // console.log(this.accounts);
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, this._tokenContractAddress);
  
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._tokenContract.methods.balanceOf((this.accounts as string[])[0]).call(function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
  
        // resolve(_web3.fromWei(result));
      });
    }) as Promise<number>;
  }
}

