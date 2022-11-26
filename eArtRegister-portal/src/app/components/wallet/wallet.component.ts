import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  wallet: any;
  user: any;
  isSellet: boolean = false;
  balance: any = '';
  depositValue: any = '';
  depositContract: any = '';
  depositServerValue: any = '';

  constructor(private web3: Web3Service, private http: HttpClient,
    private router: Router, 
    private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.signInWithMetaMask();
  }

  signInWithMetaMask(){
    this.web3.connectAccount().then(response => {
      console.log(response);
      this.wallet = response
      this.getUser();
    });
  }

  getUser(){
    this.http.get(environment.api + `Users/getUser/` + this.wallet[0]).subscribe(result => {
      this.user = result;
      this.web3.viewDeposit(this.user.depositAddress, this.user.depositAbi).then(response =>{
        this.depositValue = (response as any) / 1000000000000000000;
      });
      this.depositContract = (result as any).depositAddress;
    }, error => {
        console.error(error);
    });
  }

  createDeposit() {
    let body = {
      Wallet: this.wallet[0],
    };

    this.http.post(environment.api + `Users/createDeposit`, body).subscribe(result => {
      this.toastr.success("Deposit created");
      this.signInWithMetaMask();
    }, error => {
        console.error(error);
    });
  }

  openDeposit() {
    this.router.navigate([`/deposit`]);
  }
}
