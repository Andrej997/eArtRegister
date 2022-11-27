import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.css']
})
export class DepositComponent implements OnInit {

  depositForm: FormGroup;
  withdrawForm: FormGroup;
  wallet: any;
  user: any;
  showCode: boolean = false;
  balance;

  constructor(private web3: Web3Service, 
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.signInWithMetaMask();
    
    this.depositForm = this.fb.group({
      value: [0, Validators.required],
    });

    this.withdrawForm = this.fb.group({
      value: [0, Validators.required],
    });
  }

  signInWithMetaMask(){
    this.web3.connectAccount().then(response => {
      console.log(response);
      this.wallet = response;
      this.getUser();
    });
  }

  getUser(){
    this.http.get(environment.api + `Users/getUser/` + this.wallet[0]).subscribe(result => {
      this.user = result
      this.viewDeposit();
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

  viewDeposit() {
    this.web3.viewDeposit(this.user.depositAddress, this.user.depositAbi).then(response =>{
      this.balance = (response as any) / 1000000000000000000;
    });
  }

  addDeposit() {
    this.web3.deposit(this.user.depositAddress, this.user.depositAbi, this.depositForm.value.value).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.viewDeposit();
          this.toastr.success("Add to your private deposit");
        }
        else {
          this.toastr.error("Failed to add to deposit");
        }
      });
    });
  }

  withdraw() {
    this.web3.withdraw(this.user.depositAddress, this.user.depositAbi, this.withdrawForm.value.value * 1000000000000000000).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.viewDeposit();
          this.toastr.success("Withdraw successfully");
        }
        else {
          this.toastr.error("Failed to withdraw");
        }
      });
    });
  }
}
