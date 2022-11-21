import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

class Buyer {
  buyer: string;
  participated: string;
  participation: string;
  get getDate() {
    return this.participated;
  }
}

@Component({
  selector: 'app-bundle',
  templateUrl: './bundle.component.html',
  styleUrls: ['./bundle.component.css']
})
export class BundleComponent implements OnInit, OnDestroy {

  nfts: any[] = [];
  private bundleId = "";
  private routeSub: Subscription;
  toWallet: string = "";
  wallet = "";
  bundle = null;
  minParticipation = 0;
  showCode: boolean = false;

  constructor(private http: HttpClient, 
    private router: Router, 
    private toastr: ToastrService,
    private route: ActivatedRoute, 
    private web3: Web3Service) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
      this.web3.connectAccount().then(response => {
        this.wallet = (response as string[])[0].toLowerCase();
        this.getNFTs(this.bundleId);
        this.getBundle(this.bundleId);
      });
    });
    
  }

  private getBundle(bundleId: string) {
    this.http.get(environment.api + `Bundle/` + bundleId).subscribe(result => {
      console.log(result);
      this.bundle = (result as any);
    }, error => {
        console.error(error);
    });
  }

  private getNFTs(bundleId: string) {
    this.nfts = [];
    this.http.get(environment.api + `NFT/bundle/` + bundleId).subscribe(result => {
      console.log(result);
      this.nfts = result as any[];

      this.nfts.forEach(element => {
        if (element.purchaseContract != null) {
          if (element.statusId == "ON_SALE" || element.statusId == "CANCELED") {
            this.web3.getUserBalance(element.purchaseContract, element.currentWallet).then(response =>{
              element.balance = (response as number);
            });

            this.web3.getBuyer(element.purchaseContract).then(buyerres =>{
              if ((buyerres as any).participation != '0')
                element.buyer = (buyerres as any).buyer.toLowerCase();;
            });
          }
          else if (element.statusId == "SOLD") {
            this.web3.getUserBalance(element.purchaseContract, this.wallet).then(response =>{
              element.balance = (response as number);
            });
          }
        }
      });
    }, error => {
        console.error(error);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  setInStatusForSale(nftId: any) {
    let body = {
      Id: nftId,
      Wallet: this.wallet,
    };

    this.http.post(environment.api + `NFT/prepareForSale`, body).subscribe(result => {
      this.toastr.success("Purchase contract created");
      this.getNFTs(this.bundleId);
    }, error => {
        console.error(error);
    });
  }


  setNFTOnSale(purchaseContract: string, price: number, minParticipation: number, daysToPay: number, nftId: any) {
    this.web3.setNftOnSale(purchaseContract, price, minParticipation, daysToPay).then(response =>{
      this.web3.getTransactionStatus(response).then(response2 => {

        if ((response2 as boolean) == true) {
          this.toastr.success("NFT set on sale");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: true
          };
      
          this.http.post(environment.api + `NFT/setOnSale`, body).subscribe(result => {
            this.getNFTs(this.bundleId);
          }, error => {
              console.error(error);
          });
        }
        else {
          this.toastr.error("Failed to set NFT on sale");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: false
          };
      
          this.http.post(environment.api + `NFT/setOnSale`, body).subscribe(result => {
          }, error => {
              console.error(error);
          });
        }
      });
    });
  }

  buyNFT(purchaseContract: string, nftId: any) {
    let valueToBuy = this.minParticipation * 1000000000000000000;
    this.web3.purchaseNft(purchaseContract, valueToBuy).then(response =>{
      this.web3.getTransactionStatus(response).then(response2 => {

        if ((response2 as boolean) == true) {
          this.toastr.success("Funds transacted for NFT");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: true
          };
      
          this.http.post(environment.api + `NFT/bought`, body).subscribe(result => {
            this.getNFTs(this.bundleId);
          }, error => {
              console.error(error);
          });
        }
        else {
          this.toastr.error("Failed to transact funds for NFT");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: false
          };
      
          this.http.post(environment.api + `NFT/bought`, body).subscribe(result => {
          }, error => {
              console.error(error);
          });
        }
      });
    })
  }

  sendNFTasGift(nftId: any, erc721: string, tokenId: number) {
    this.web3.sendNftAsGift(erc721, this.wallet, this.toWallet, tokenId).then(response =>{
      // let body = {
      //   NFTId: nftId,
      //   From: this.wallet,
      //   To: this.toWallet,
      //   TransactionHash: response
      // };
  
      // this.http.post(environment.api + `NFT/sendAsGift`, body).subscribe(result => {
      // }, error => {
      //     console.error(error);
      // });
    })
  }

  mintNFT() {
    this.router.navigate([`/bundles/${this.bundleId}/mint`]);
  }

  withdraw(purchaseContract: string, nftId: string) {
    this.web3.withdraw(purchaseContract).then(response =>{
      this.web3.getTransactionStatus(response).then(response2 => {

        if ((response2 as boolean) == true) {
          this.toastr.success("Funds withdrawed");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: true
          };
      
          this.http.post(environment.api + `NFT/withdrawFunds`, body).subscribe(result => {
            this.getNFTs(this.bundleId);
          }, error => {
              console.error(error);
          });
        }
        else {
          this.toastr.error("Failed to withdrawed funds");
          let body = {
            NFTId: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: false
          };
      
          this.http.post(environment.api + `NFT/withdrawFunds`, body).subscribe(result => {
          }, error => {
              console.error(error);
          });
        }
      });
    })
  }

  approve(nftId: string, purchaseContract: string) {
    this.web3.setApprovalForAll((this.bundle as any).contractAddress, purchaseContract).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {

        if ((response2 as boolean) == true) {
          this.toastr.success("Contract approved");
          let body = {
            Id: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: true
          };
      
          this.http.post(environment.api + `NFT/approved`, body).subscribe(result => {
            this.getNFTs(this.bundleId);
          }, error => {
              console.error(error);
          });
        }
        else {
          this.toastr.error("Failed to approve contract");
          let body = {
            Id: nftId,
            Wallet: this.wallet,
            TransactionHash: response,
            IsCompleted: false
          };
      
          this.http.post(environment.api + `NFT/approved`, body).subscribe(result => {
          }, error => {
              console.error(error);
          });
        }
      });
    });
  }

  cancel(purchaseContract: string, buyerAddress: string, currentWallet: string, minParticipation: number, nftId: string) {
    if (this.wallet.toLowerCase() === buyerAddress.toLowerCase()) {
        this.web3.buyerRequestToStopBuy(purchaseContract, buyerAddress).then(response => {
          this.web3.getTransactionStatus(response).then(response2 => {

            if ((response2 as boolean) == true) {
              this.toastr.success("You sucessfully canceled");
              let body = {
                NFTId: nftId,
                Wallet: this.wallet,
                TransactionHash: response,
                IsCompleted: true
              };
          
              this.http.post(environment.api + `NFT/cancel`, body).subscribe(result => {
                this.getNFTs(this.bundleId);
              }, error => {
                  console.error(error);
              });
            }
            else {
              this.toastr.error("Failed to cancel");
              let body = {
                NFTId: nftId,
                Wallet: this.wallet,
                TransactionHash: response,
                IsCompleted: false
              };
          
              this.http.post(environment.api + `NFT/cancel`, body).subscribe(result => {
              }, error => {
                  console.error(error);
              });
            }
          });
      });
    }
    else if (this.wallet.toLowerCase() === currentWallet.toLowerCase()){
      this.web3.sellerStop(purchaseContract, buyerAddress, minParticipation).then(response => {
        this.web3.getTransactionStatus(response).then(response2 => {

          if ((response2 as boolean) == true) {
            this.toastr.success("You sucessfully canceled");
            let body = {
              NFTId: nftId,
              Wallet: this.wallet,
              TransactionHash: response,
              IsCompleted: true
            };
        
            this.http.post(environment.api + `NFT/cancel`, body).subscribe(result => {
              this.getNFTs(this.bundleId);
            }, error => {
                console.error(error);
            });
          }
          else {
            this.toastr.error("Failed to cancel");
            let body = {
              NFTId: nftId,
              Wallet: this.wallet,
              TransactionHash: response,
              IsCompleted: false
            };
        
            this.http.post(environment.api + `NFT/cancel`, body).subscribe(result => {
            }, error => {
                console.error(error);
            });
          }
        });
    });
    }
    else {
      this.toastr.error('Unknown action');
    }
  }
}
