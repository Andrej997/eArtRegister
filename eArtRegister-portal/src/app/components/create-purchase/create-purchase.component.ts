import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-purchase',
  templateUrl: './create-purchase.component.html',
  styleUrls: ['./create-purchase.component.css']
})
export class CreatePurchaseComponent implements OnInit {

  bundleForm: FormGroup;
  wallet: any;
  repaymentInInstallments: boolean = false;
  entireAmount: boolean = false;
  auction: boolean = false;
  bundleId: any;
  tokenId: any;
  private routeSub: Subscription;

  constructor(private fb: FormBuilder, 
    private router: Router,
    private route: ActivatedRoute, 
    private web3: Web3Service,
    private http: HttpClient,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.web3.connectAccount().then(response => {
      this.wallet = (response as string[])[0]
    });
    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
      this.tokenId = params['tokenId'];
    });

    this.bundleForm = this.fb.group({
      entireAmount: [false, Validators.required],
      repaymentInInstallments: [false, Validators.required],
      auction: [false, Validators.required],
    });
  }

  repaymentInInstallmentsChange(event: any){
    this.repaymentInInstallments = (event.currentTarget.checked as boolean);
    (event.currentTarget.checked as boolean) ? this.toastr.info("Repayment in installments activated") : this.toastr.info("Repayment in installments deactivated");
  }

  entireAmountChange(event: any){
    (event.currentTarget.checked as boolean) ? this.toastr.info("Entire amount activated") : this.toastr.info("Entire amount deactivated");
    this.entireAmount = (event.currentTarget.checked as boolean);
  }

  auctionChange(event: any){
    (event.currentTarget.checked as boolean) ? this.toastr.info("Auction activated") : this.toastr.info("Auction deactivated");
    this.auction = (event.currentTarget.checked as boolean);
  }

  onFirstSubmit() {
    this.web3.connectAccount().then(response => {
      let body = {
        CustomRouth: this.bundleId,
        TokenId: this.tokenId,
        EntireAmount: this.bundleForm.value.entireAmount,
        RepaymentInInstallments: this.bundleForm.value.repaymentInInstallments,
        Auction: this.bundleForm.value.auction,
        Wallet: (response as string[])[0]
      };
  
      this.http.post(environment.api + `NFT/createPurchase`, body).subscribe(result => {
        this.toastr.success("Purchase contract created");
        this.router.navigate([`bundle/${this.bundleId}/${this.tokenId}`]);
      }, error => {
          console.error(error);
          this.toastr.error("Failed to create purchase contract");
      });
    });
  }

}
