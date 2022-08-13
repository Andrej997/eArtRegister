import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  user: any;

  constructor(private web3: Web3Service, private http: HttpClient) { }

  ngOnInit(): void {
  }

  signInWithMetaMask(){
    this.web3.connectAccount().then(response => {
      console.log(response);
      this.user = response

      this.checkWallet(this.user[0]);
    });
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
}
