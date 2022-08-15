import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

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
    this.http.get(environment.api + `NFT/bundle/` + bundleId).subscribe(result => {
      console.log(result);
      this.nfts = result as any[];

      this.nfts.forEach(element => {
        if (element.purchaseContract != null) {
          this.web3.getUserBalance(element.purchaseContract).then(response =>{
            element.balance = (response as number);
          });
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
      this.nfts = [];
      this.getNFTs(this.bundleId);
    }, error => {
        console.error(error);
    });
  }

  setNFTOnSale(purchaseContract: string, valueOfNft: number, erc721: string, tokenId: number, nftId: any) {
    this.web3.setNftOnSale(purchaseContract, valueOfNft, erc721, tokenId).then(response =>{
      if (response) {
        let body = {
          NFTId: nftId,
          Wallet: this.wallet,
          TransactionHash: response
        };
    
        this.http.post(environment.api + `NFT/setOnSale`, body).subscribe(result => {
          this.toastr.success("NFT approved");
          this.nfts = [];
          this.getNFTs(this.bundleId);
        }, error => {
            console.error(error);
        });
      }
    });
  }

  buyNFT(purchaseContract: string, valueOfNft: number, erc721: string, tokenId: number, nftId: any) {
    this.web3.purchaseNft(purchaseContract, valueOfNft, erc721, tokenId).then(response =>{
      if (response) {
        let body = {
          NFTId: nftId,
          Wallet: this.wallet,
          TransactionHash: response
        };
    
        this.http.post(environment.api + `NFT/bought`, body).subscribe(result => {
          this.toastr.success("NFT bought");
          this.nfts = [];
          this.getNFTs(this.bundleId);
        }, error => {
            console.error(error);
        });
      }
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

  withdraw(purchaseContract: string, valueOfNft: number) {
    this.web3.withdraw(purchaseContract, valueOfNft).then(response =>{
      this.toastr.success("Funds withdrawed");
        this.nfts = [];
        this.getNFTs(this.bundleId);
    })
  }

  approve(nftId: string, purchaseContract: string) {
    this.web3.setApprovalForAll((this.bundle as any).contractAddress, purchaseContract).then(response => {
      let body = {
        Id: nftId,
        Wallet: this.wallet,
      };
  
      this.http.post(environment.api + `NFT/approved`, body).subscribe(result => {
        this.toastr.success("NFT approved");
        this.nfts = [];
        this.getNFTs(this.bundleId);
      }, error => {
          console.error(error);
      });
    });
  }
}
