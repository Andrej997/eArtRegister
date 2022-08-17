import { HttpClient, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-mint-nft',
  templateUrl: './mint-nft.component.html',
  styleUrls: ['./mint-nft.component.css']
})
export class MintNftComponent implements OnInit, OnDestroy {

  mintForm: FormGroup;
  bundleId = "";
  private routeSub: Subscription;

  constructor(private fb: FormBuilder, private router: Router,
    private http: HttpClient,
    private web3: Web3Service,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef, 
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.mintForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      minimumParticipation: [0, Validators.required],
      daysToPay: [0, Validators.required],
    });

    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  private formData:FormData = new FormData();
  fileChange(event: any) {
    let fileList: FileList = event.target.files;
    if(fileList.length > 0) {
        let file: File = fileList[0];
        this.formData.append('file', file, file.name);
    }
}

  onFirstSubmit() {
    this.web3.connectAccount().then(response => {
      let params = new HttpParams()
        .set('name', this.mintForm.value.name)
        .set('description', this.mintForm.value.description)
        .set('bundleId', this.bundleId)
        .set('price', this.mintForm.value.price * 1000000000000000000)
        .set('minimumParticipation', this.mintForm.value.minimumParticipation * 1000000000000000000)
        .set('daysToPay', this.mintForm.value.daysToPay)
        .set('wallet', (response as string[])[0])
        ;

      this.http.post(environment.api + `NFT/add`, this.formData, { params: params }).subscribe(result => {
        this.toastr.success("Image minted");
        this.router.navigate([`/bundles/${this.bundleId}`]);
      }, error => {
          console.error(error);
          this.toastr.error("Failed to mint");
      });
    });
  }

}
