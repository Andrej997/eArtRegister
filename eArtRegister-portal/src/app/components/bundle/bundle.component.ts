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

  ipdsPublicGateway = environment.ipfs;

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
    this.http.get(environment.api + `NFT/` + bundleId).subscribe(result => {
      console.log(result);
      this.nfts = result as any[];

      this.nfts.forEach(element => {
        this.http.get(this.ipdsPublicGateway + element.ipfsnftHash).subscribe(resultData => {
          element.nftData = resultData;
        });
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

  openNFT(bundleCustomRoot, tokenId) {
    this.router.navigate([`bundle/${bundleCustomRoot}/${tokenId}`]);
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

  mintNFT() {
    this.router.navigate([`/bundle/${this.bundleId}/mint`]);
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
