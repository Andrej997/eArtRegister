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
}
