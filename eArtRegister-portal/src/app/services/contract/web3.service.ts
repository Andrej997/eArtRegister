import {Inject, Injectable} from '@angular/core';
import { WEB3 } from '../../core/web3';
import { Subject } from 'rxjs';
import Web3 from 'web3';
import Web3Modal from "web3modal";
import WalletConnectProvider from "@walletconnect/web3-provider";
import { provider } from 'web3-core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

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

  constructor(@Inject(WEB3) private web3: Web3, private http: HttpClient) {
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

    this.checkWallet((this.accounts as string[])[0]);
    return this.accounts;
  }

  private checkWallet(wallet: string) {
    let body = {
      Wallet: wallet,
    };

    this.http.post(environment.api + `Users/checkWallet`, body).subscribe(result => {
    }, error => {
        console.error(error);
    });
  }

  async accountInfo(account: any[]){
    const initialvalue = await this.web3js.eth.getBalance(account);
    this.balance = this.web3js.utils.fromWei(initialvalue , 'ether');
    return this.balance;
  }

  private _tokenContract: any;
  private _tokenContractAddress: string = "0x46F0e4574c5A1cB78bAD2144704c247f2d38A365";

  async setNftOnSale(purchaseContract: string, valueOfNft: number, erc721: string, tokenId: number): Promise<number> {
    const etherValue = Web3.utils.fromWei(valueOfNft.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); // set provider
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    console.log(this.accounts);
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);

    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._tokenContract.methods.addListing(weiValue, erc721, tokenId).send({from: (this.accounts as string[])[0], gas: 3000000},function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
  
        // resolve(_web3.fromWei(result));
      });
    }) as Promise<number>;
  }

  

  async purchaseNft(purchaseContract: string, valueOfNft: number, erc721: string, tokenId: number): Promise<number> {
    const etherValue = Web3.utils.fromWei(valueOfNft.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }
    this.accounts = await this.web3js.eth.getAccounts();
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
    
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      console.log(this._tokenContract);
      
      this._tokenContract.methods.purchase(erc721, tokenId).send({from: (this.accounts as string[])[0], gas: 3000000, value: weiValue}, function (err, result) {
        console.log(err);
        console.log(result);
        
        if(err != null) {
          reject(err);
        }
      });
    }) as Promise<number>;
  }

  private _erc271Contract: any;

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

  public async getUserBalance(purchaseContract: string): Promise<number> {

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }
    this.accounts = await this.web3js.eth.getAccounts();
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
  
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      
      this._tokenContract.methods.balances((this.accounts as string[])[0]).call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<number>;
  }

  public async withdraw(purchaseContract: string, valueOfNft: number): Promise<number> {
    const etherValue = Web3.utils.fromWei(valueOfNft.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }
    this.accounts = await this.web3js.eth.getAccounts();
    this._tokenContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
  
    return new Promise((resolve, reject) => {
      let _web3 = this.web3js;
      
      this._tokenContract.methods.withdraw(weiValue, (this.accounts as string[])[0]).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<number>;
  }

}



