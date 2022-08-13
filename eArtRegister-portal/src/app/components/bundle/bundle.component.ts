import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
    }, error => {
        console.error(error);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  setNFTOnSale(nftId: any) {
    // this.web3.setNftOnSale().then(response =>{

    // })
    let body = {
      NFTId: nftId,
    };

    this.http.post(environment.api + `NFT/setOnSale`, body).subscribe(result => {
    }, error => {
        console.error(error);
    });
  }

  buyNFT(nftId: any) {
    console.log(nftId);
    // this.web3.purchaseNft().then(response =>{

    // })
  }

  sendNFTasGift(nftId: any, erc721: string, tokenId: number) {
    this.web3.sendNftAsGift(erc721, this.wallet, this.toWallet, tokenId).then(response =>{
      let body = {
        NFTId: nftId,
        From: this.wallet,
        To: this.toWallet,
        TransactionHash: response
      };
  
      this.http.post(environment.api + `NFT/sendAsGift`, body).subscribe(result => {
      }, error => {
          console.error(error);
      });
    })
  }

  mintNFT() {
    this.router.navigate([`/bundles/${this.bundleId}/mint`]);
  }

  openNFT(nftId: any) {
    this.router.navigate([`/bundles/${this.bundleId}/nft/${nftId}`]);
  }
}
