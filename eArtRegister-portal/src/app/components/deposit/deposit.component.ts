import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.css']
})
export class DepositComponent implements OnInit {

  wallet: any;
  user: any;
  showCode: boolean = false;
  balance;

  constructor(private web3: Web3Service, private http: HttpClient,
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
      this.user = result
      this.web3.viewDeposit(this.user.depositAddress, this.user.depositAbi).then(response =>{
        this.balance = (response as any) / 1000000000000000000;
      });
    }, error => {
        console.error(error);
    });
  }

  addDeposit() {
    this.web3.deposit(this.user.depositAddress, this.user.depositAbi).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.signInWithMetaMask();
          this.toastr.success("Add to your private deposit");
        }
        else {
          this.toastr.error("Failed to add to deposit");
        }
      });
    });
  }
}
