import {Inject, Injectable} from '@angular/core';
import { WEB3 } from '../../core/web3';
import { Subject } from 'rxjs';
import Web3 from 'web3';
import Web3Modal from "web3modal";
import WalletConnectProvider from "@walletconnect/web3-provider";
import { provider } from 'web3-core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { timer } from 'rxjs';
import { take } from 'rxjs/operators';

let tokenAbi = require('./transferAbi.json');
let erc271Abi = require('./erc271Abi.json');
//let depositAbi = require('./depositAbi.json');

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

  private _traderContract: any;
  private _erc271Contract: any;
  private _depositContract: any;

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
    } 
    this.accounts = await this.web3js.eth.getAccounts();

    this.checkWallet((this.accounts as string[])[0]);
    return this.accounts;
  }

  async getTransactionStatus(transactionHash): Promise<boolean> {
    this.provider = await this.web3Modal.connect(); // set provider
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 

    let receipt = null;
    do {
      receipt = await this.web3js.eth.getTransactionReceipt(transactionHash);
      await timer(1000).pipe(take(1)).toPromise();
    } while (receipt == null)

    return ((receipt as any).status as boolean);
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

  // ERC721 CONTRACT ----------------------------------------------------------------

  async setApprovalForAll(abi: string, address: string, operator: string): Promise<string> {

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);

    return new Promise((resolve, reject) => {
      contract.methods.setApprovalForAll(operator, true).send({ from: (this.accounts as string[])[0], gas: 3000000 }, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  public async isApprovedForAll(abi: string, address: string, ownerAddress: string, purchaseAddress: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.isApprovedForAll(ownerAddress, purchaseAddress).call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async ownerOf(abi: string, address: string, tokenId: number): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.ownerOf(tokenId).call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  // PURCHASE CONTRACT ---------------------------------------------------------------

  public async getUserBalance(purchaseContract: string, wallet: string): Promise<number> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    this._traderContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
  
    return new Promise((resolve, reject) => {
      this._traderContract.methods.balance(wallet).call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<number>;
  }

  async setPrice(abi: string, address: string, price: number, minParticipation: number, daysToPay: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(price.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    const etherValueMP = Web3.utils.fromWei(minParticipation.toString(), 'ether');
    const weiValueMP = Web3.utils.toWei(etherValueMP, 'ether');

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);

    return new Promise((resolve, reject) => {
      contract.methods.setPrice(weiValue, daysToPay, weiValueMP).send({ from: (this.accounts as string[])[0], gas: 3000000 }, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async editPrice(abi: string, address: string, price: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(price.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);

    return new Promise((resolve, reject) => {
      contract.methods.editPrice(weiValue).send({ from: (this.accounts as string[])[0], gas: 3000000 }, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async editDeadline(abi: string, address: string, daysToPay: number): Promise<string> {
    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);

    return new Promise((resolve, reject) => {
      contract.methods.editDeadline(daysToPay).send({ from: (this.accounts as string[])[0], gas: 3000000 }, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async purchase(abiDeposit: string, addressDeposit: string, addressPurchase: string, amount: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(amount.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abiDeposit), addressDeposit);
    
    return new Promise((resolve, reject) => {
      contract.methods.purchaseContract(addressPurchase, weiValue).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async participate(abiDeposit: string, addressDeposit: string, addressPurchase: string, amount: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(amount.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abiDeposit), addressDeposit);
    
    return new Promise((resolve, reject) => {
      contract.methods.participate(addressPurchase, weiValue).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async payTheInstallment(abiDeposit: string, addressDeposit: string, addressPurchase: string, amount: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(amount.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abiDeposit), addressDeposit);
    
    return new Promise((resolve, reject) => {
      contract.methods.payTheInstallment(addressPurchase, weiValue).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async sendBid(abiDeposit: string, addressDeposit: string, addressPurchase: string, amount: number): Promise<string> {
    const etherValue = Web3.utils.fromWei(amount.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abiDeposit), addressDeposit);
    
    return new Promise((resolve, reject) => {
      contract.methods.sendBid(addressPurchase, weiValue).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async sellerStop(purchaseContract: string, buyerAddr: string, minParticipation: number): Promise<string> {
    minParticipation = minParticipation / 2;
    const etherValue = Web3.utils.fromWei(minParticipation.toString(), 'ether');
    const weiValue = Web3.utils.toWei(etherValue, 'ether');

    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    this._traderContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
    
    return new Promise((resolve, reject) => {
      this._traderContract.methods.sellerStop(buyerAddr, environment.serverAddress).send({from: (this.accounts as string[])[0], gas: 3000000, value: weiValue}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  async buyerRequestToStopBuy(purchaseContract: string, buyerAddr: string): Promise<string> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    this._traderContract = await new this.web3js.eth.Contract(tokenAbi, purchaseContract);
    
    return new Promise((resolve, reject) => {
      this._traderContract.methods.buyerRequestToStopBuy(buyerAddr).send({from: (this.accounts as string[])[0], gas: 3000000}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }

        resolve(result);
      });
    }) as Promise<string>;
  }

  // views
  public async getPrice(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getPrice().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getIsPriceSet(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getIsPriceSet().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getBuyer(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getBuyer().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getParticipationPayed(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getParticipationPayed().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getIsSold(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getIsSold().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getMinParticipation(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getMinParticipation().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getSeller(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getSeller().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getContractOwner(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getContractOwner().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getListedDate(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getListedDate().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getEndSellDate(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getEndSellDate().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getBiggestBid(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getBiggestBid().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  public async getInstallemntUser(abi: string, address: string): Promise<any> {
    this.provider = await this.web3Modal.connect(); 
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    }

    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(abi), address);
  
    return new Promise((resolve, reject) => {
      contract.methods.getInstallemntUser().call(function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<any>;
  }

  // DEPOSIT CONTRACT ---------------------------------------------------------------

  public async deposit(depositContract: string, depositAbi: string, amount: number): Promise<string> {
    const weiValue = Web3.utils.toWei(amount.toString(), 'ether');

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(depositAbi), depositContract);

    return new Promise((resolve, reject) => {
      
      contract.methods.deposit().send({ from: (this.accounts as string[])[0], gas: 3000000, value: weiValue},function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<string>;
  }

  public async withdraw(depositContract: string, depositAbi: string, amount: number): Promise<string> {
    const weiValue = Web3.utils.toWei(amount.toString(), 'ether');

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    let contract = await new this.web3js.eth.Contract(JSON.parse(depositAbi), depositContract);

    return new Promise((resolve, reject) => {
      
        contract.methods.withdraw((this.accounts as string[])[0], amount).send({ from: (this.accounts as string[])[0], gas: 3000000},function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<string>;
  }

  public async viewDeposit(depositContract: string, depositAbi: string): Promise<string> {
    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    this._depositContract = await new this.web3js.eth.Contract(JSON.parse(depositAbi), depositContract);

    return new Promise((resolve, reject) => {
      
      this._depositContract.methods.viewDeposit().call({from: (this.accounts as string[])[0]}, function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<string>;
  }

  public async withdrawDeposit(depositContract: string, depositAbi: string): Promise<string> {
    const weiValue = Web3.utils.toWei('0.01', 'ether');

    this.provider = await this.web3Modal.connect();
    if (this.provider) {
      this.web3js = new Web3(this.provider);
    } 
    this.accounts = await this.web3js.eth.getAccounts();
    this._depositContract = await new this.web3js.eth.Contract(depositAbi, depositContract);

    return new Promise((resolve, reject) => {
      
      this._depositContract.methods.withdraw(environment.serverAddress).send({ from: (this.accounts as string[])[0], gas: 3000000, value: weiValue},function (err, result) {
        
        if(err != null) {
          reject(err);
        }
  
        resolve(result);
      });
    }) as Promise<string>;
  }
}



