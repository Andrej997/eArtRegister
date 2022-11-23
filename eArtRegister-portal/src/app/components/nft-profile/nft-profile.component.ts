import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-nft-profile',
  templateUrl: './nft-profile.component.html',
  styleUrls: ['./nft-profile.component.css']
})
export class NftProfileComponent implements OnInit {

  ipdsPublicGateway = environment.ipfs;
  private bundleId = "";
  private tokenId = -1;
  private routeSub: Subscription;
  wallet = "";
  bundle = null;
  nft: any;
  private nfts: any[] = [];
  purchaseContract: any = null;

  constructor(private http: HttpClient, 
    private router: Router, 
    private toastr: ToastrService,
    private route: ActivatedRoute, 
    private web3: Web3Service) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
      this.tokenId = params['tokenId'];
      
      this.web3.connectAccount().then(response => {
        this.wallet = (response as string[])[0].toLowerCase();
        this.getNFTs(this.bundleId);
        this.getBundle(this.bundleId);
      });
    });
  }

  setOnSale(bundleCustomRoot, tokenId) {
    this.router.navigate([`/${bundleCustomRoot}/${tokenId}/set-on-sale`]);
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
    this.http.get(environment.api + `NFT/${bundleId}/${this.tokenId}`).subscribe(result => {
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
      
      if (this.nfts.length > 0) {
        this.nft = this.nfts[0];
        this.purchaseContract = this.nft.purchaseContracts[0];
      }
        console.log(this.nft);
        
    }, error => {
        console.error(error);
    });
  }
}
