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
  nft: any;
  private nfts: any[] = [];
  purchaseContract: any = null;
  setOnSale: boolean = false;
  editPrice: boolean = false;
  editDate: boolean = false;

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
      if (this.router.url.includes('set-on-sale')) this.setOnSale = true;
      else if (this.router.url.includes('edit-price')) this.editPrice = true;
      else if (this.router.url.includes('edit-date')) this.editDate = true;
    });

    this.web3.connectAccount().then(response => {
      this.wallet = (response as string[])[0].toLowerCase();
      this.getNFTs(this.bundleId);
    });

    this.bundleForm = this.fb.group({
      amountInETH: [0, Validators.required],
      daysOnSale: [0, Validators.required],
      minParticipation: [0, Validators.required],
    });
  }

  private getNFTs(bundleId: string) {
    this.http.get(environment.api + `NFT/${bundleId}/${this.tokenId}`).subscribe(result => {
      this.nfts = result as any[];
      if (this.nfts.length > 0) {
        this.nft = this.nfts[0];
        this.purchaseContract = this.nft.purchaseContracts[0];
      }
    }, error => {
        console.error(error);
    });
  }

  onFirstSubmit() {
    let priceWei = this.bundleForm.value.amountInETH * 1000000000000000000;
    let minParticipationWei = this.bundleForm.value.minParticipation * 1000000000000000000;
    
    if (this.setOnSale) this.setPrice(priceWei, minParticipationWei);
    else if (this.editPrice) this.editPriceF(priceWei);
    else if (this.editDate) this.editDeadline();
  }

  private setPrice(priceWei, minParticipationWei) {
    this.web3.setPrice(this.purchaseContract.abi, this.purchaseContract.address, priceWei, minParticipationWei, this.bundleForm.value.daysOnSale).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.toastr.success("NFT is successfully set on sale");
          this.router.navigate([`bundle/${this.bundleId}/${this.tokenId}`]);
        }
        else {
          this.toastr.error("Failed to set NFT on sale");
        }
      });
    });
  }

  private editPriceF(priceWei) {
    this.web3.editPrice(this.purchaseContract.abi, this.purchaseContract.address, priceWei).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.toastr.success("NFT is successfully set on sale");
          this.router.navigate([`bundle/${this.bundleId}/${this.tokenId}`]);
        }
        else {
          this.toastr.error("Failed to set NFT on sale");
        }
      });
    });
  }

  private editDeadline() {
    this.web3.editDeadline(this.purchaseContract.abi, this.purchaseContract.address, this.bundleForm.value.daysOnSale).then(response => {
      this.web3.getTransactionStatus(response).then(response2 => {
        if ((response2 as boolean) == true) {
          this.toastr.success("NFT is successfully set on sale");
          this.router.navigate([`bundle/${this.bundleId}/${this.tokenId}`]);
        }
        else {
          this.toastr.error("Failed to set NFT on sale");
        }
      });
    });
  }
}
