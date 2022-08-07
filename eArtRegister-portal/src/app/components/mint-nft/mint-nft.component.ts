import { HttpClient, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { AuthService } from 'src/app/services/auth.service';
import { MetaMaskService } from 'src/app/services/meta-mask.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-mint-nft',
  templateUrl: './mint-nft.component.html',
  styleUrls: ['./mint-nft.component.css']
})
export class MintNftComponent implements OnInit {

  mintForm: FormGroup;
  wallet: any;
  bundleId = "473a2cb4-c062-4e4a-a71f-c5321ee3ee0a";

  constructor(private fb: FormBuilder, private router: Router,
    private authService: AuthService,
    private http: HttpClient,
    private toastr: ToastrService,
    private metaMaskService: MetaMaskService,
    private cdr: ChangeDetectorRef,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.mintForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      royality: [0, Validators.required],
    });

    this.checkConnected();
  }

  checkConnected(){
    this.metaMaskService.isConnected().subscribe( (res: any) => {
      this.wallet = res.accounts[0]
      console.log(this.wallet);
      this.mintForm.value.wallet = this.wallet;
      res.provider.on('accountsChanged', (accounts: any) => {
        if (accounts.length == 0) {
          this.wallet = undefined;
        }else {
          this.wallet = accounts[0];
        }
        this.cdr.detectChanges();
      });
    })
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
    

    let params = new HttpParams()
      .set('name', this.mintForm.value.name)
      .set('description', this.mintForm.value.description)
      .set('bundleId', this.bundleId)
      .set('price', this.mintForm.value.price)
      .set('royality', this.mintForm.value.royality)
      .set('wallet', this.wallet)
      ;

    this.http.post(environment.api + `NFT/add`, this.formData, { params: params }).subscribe(result => {
      this.toastr.success("Image minted");
      this.router.navigate([`/bundles/${this.bundleId}`]);
    }, error => {
        console.error(error);
        this.toastr.error("Failed to mint");
    });
  }

}
