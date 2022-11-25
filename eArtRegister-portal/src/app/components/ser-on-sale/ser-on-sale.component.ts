import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-ser-on-sale',
  templateUrl: './ser-on-sale.component.html',
  styleUrls: ['./ser-on-sale.component.css']
})
export class SerOnSaleComponent implements OnInit {

  bundleForm: FormGroup;
  wallet: any;
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
    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
      this.tokenId = params['tokenId'];
    });

    this.bundleForm = this.fb.group({
      amountInETH: [0, Validators.required],
      daysOnSale: [0, Validators.required],
      minParticipation: [0, Validators.required],
    });
  }

  onFirstSubmit() {
    this.web3.connectAccount().then(response => {
      let body = {
        CustomRouth: this.bundleId,
        TokenId: this.tokenId,
        AmountInETH: this.bundleForm.value.amountInETH,
        DaysOnSale: this.bundleForm.value.daysOnSale,
        MinParticipation: this.bundleForm.value.minParticipation,
        Wallet: (response as string[])[0]
      };
  
      // this.http.post(environment.api + `NFT/setOnSale`, body).subscribe(result => {
      //   this.toastr.success("Purchase contract created");
      //   this.router.navigate([`bundle/${(result as any).customRoute}`]);
      // }, error => {
      //     console.error(error);
      //     this.toastr.error("Failed to create bundle");
      // });
    });
  }

}
