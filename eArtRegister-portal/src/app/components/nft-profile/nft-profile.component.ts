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
  bundle;
  nft: any;
  private nfts: any[] = [];
  purchaseContract: any = null;
  amountInETH;
  minParticipation;
  saleEnds;
  seller;
  biggestBid;
  listedDate;
  isPurchaseApproved: boolean = false;

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
    this.router.navigate([`bundle/${bundleCustomRoot}/${tokenId}/set-on-sale`]);
  }

  editPrice(bundleCustomRoot, tokenId) {
    this.router.navigate([`bundle/${bundleCustomRoot}/${tokenId}/edit-price`]);
  }

  editDate(bundleCustomRoot, tokenId) {
    this.router.navigate([`bundle/${bundleCustomRoot}/${tokenId}/edit-date`]);
  }

  createPurchase(bundleCustomRoot, tokenId) {
    this.router.navigate([`bundle/${bundleCustomRoot}/${tokenId}/create-purchase`]);
  }

  appropve() {
    this.web3.setApprovalForAll(this.bundle.abi, this.bundle.address, this.purchaseContract.address).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.toastr.success("Purchase is approved successfully");
        }
        else {
          this.toastr.error("Failed to approved");
        }
      });
    });
  }

  buy() {
    
  }

  private callPurchaseContractViews() {
    this.isApprovedForAll();
    this.getPrice();
    this.getSeller();
    this.getListedDate();
    this.getEndSellDate();
    this.getBiggestBid();
  }

  private isApprovedForAll() {
    if (this.purchaseContract) {
      this.web3.isApprovedForAll(this.bundle.abi, this.bundle.address, this.bundle.ownerWallet, this.purchaseContract.address).then(response => {
        this.isPurchaseApproved = response;
      });
    }
  }

  private getPrice() {
    if (this.purchaseContract) {
      this.web3.getPrice(this.purchaseContract.abi, this.purchaseContract.address).then(response => {
        this.amountInETH = response / 1000000000000000000;
      });
    }
  }

  private getSeller() {
    if (this.purchaseContract) {
      this.web3.getSeller(this.purchaseContract.abi, this.purchaseContract.address).then(response => {
        this.seller = response;
      });
    }
  }

  private getListedDate() {
    if (this.purchaseContract) {
      this.web3.getListedDate(this.purchaseContract.abi, this.purchaseContract.address).then(response => {
        var date = new Date(response * 1000);
        this.listedDate = date;
      });
    }
  }

  private getEndSellDate() {
    if (this.purchaseContract) {
      this.web3.getEndSellDate(this.purchaseContract.abi, this.purchaseContract.address).then(response => {
        var date = new Date(response * 1000);
        this.saleEnds = date;
      });
    }
  }

  private getBiggestBid() {
    if (this.purchaseContract) {
      this.web3.getBiggestBid(this.purchaseContract.abi, this.purchaseContract.address).then(response => {
        this.biggestBid = response;
      });
    }
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
        if (this.nft.purchaseContracts?.length > 0) {
          this.purchaseContract = this.nft.purchaseContracts[0];
          console.log(this.nft);
          
          this.callPurchaseContractViews();
        }
      }
    }, error => {
        console.error(error);
    });
  }
}
