import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-my-nfts',
  templateUrl: './my-nfts.component.html',
  styleUrls: ['./my-nfts.component.css']
})
export class MyNftsComponent implements OnInit {

  nfts: any[] = [];
  wallet = "";
  toWallet = "";

  constructor(private http: HttpClient, 
    private web3: Web3Service) { }

  ngOnInit(): void {
    this.web3.connectAccount().then(response => {
      this.wallet = (response as string[])[0].toLowerCase();
      this.getNFTs();
    });
  }

  private getNFTs() {
    this.http.get(environment.api + `NFT/mine/` + this.wallet).subscribe(result => {
      console.log(result);
      this.nfts = result as any[];
    }, error => {
        console.error(error);
    });
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

  sendNFTasGift(nftId: any, erc721: string, tokenId: number) {
    this.web3.sendNftAsGift(erc721, this.wallet, this.toWallet, tokenId).then(response =>{
      console.log(response);
      
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
}
