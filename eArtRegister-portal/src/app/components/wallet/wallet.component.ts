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
  isSellet: boolean = false;
  balance: any = '';
  depositValue: any = '';
  depositContract: any = '';
  depositServerValue: any = '';

  constructor(private web3: Web3Service, private http: HttpClient) { }

  ngOnInit(): void {
    this.signInWithMetaMask();
  }

  signInWithMetaMask(){
    this.web3.connectAccount().then(response => {
      console.log(response);
      this.user = response
      this.getUserRoles();
      this.getGetWalletBallance(this.user[0]);
    });
  }

  private getGetWalletBallance(wallet) {
    this.http.get(environment.chainApi + "?module=account&action=balance&address=" + wallet + "&tag=latest&apikey=" + environment.apiKey).subscribe(result => {
      this.balance = ((result as any).result) / 1000000000000000000;
    }, error => {
        console.error(error);
    });
  }

  getUserRoles(){
    this.http.get(environment.api + `Users/getUser/` + this.user[0]).subscribe(result => {
      console.log(result);
      this.isSellet = ((result as any).roleIds.includes(4));
      this.depositValue = (result as any).depositBalance / 1000000000000000000;
      this.depositContract = (result as any).depositContract;
      this.depositServerValue = (result as any).serverBalance / 1000000000000000000;
    }, error => {
        console.error(error);
    });
  }

  createDeposit() {
    let body = {
      Wallet: this.user[0],
    };

    this.http.post(environment.api + `Users/createDeposit`, body).subscribe(result => {
    }, error => {
        console.error(error);
    });
  }

  addDeposit() {
    this.web3.deposit(this.depositContract).then(response => {
      let body = {
        Wallet: this.user[0],
        DepositValue : 20000000000000000,
        TransactionHash: response
      };
  
      this.http.post(environment.api + `Users/deposit`, body).subscribe(result => {
      }, error => {
          console.error(error);
      });
    });
  }

  depositToServer() {
    this.web3.withdrawDeposit(this.depositContract).then(response => {
      let body = {
        Wallet: this.user[0],
        DepositValue : 10000000000000000,
      };
  
      this.http.post(environment.api + `Users/deposit/server`, body).subscribe(result => {
      }, error => {
          console.error(error);
      });
    });
  }
}
