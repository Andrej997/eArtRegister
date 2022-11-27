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

  numOfAttributes = 0;
  numOfAttributesArr: number[] = [];

  attributeKeys: string[] = [];
  attributeValues: string[] = [];

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
      external_url: [''],
    });

    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
    });
  }

  addAttribute() {
    this.numOfAttributesArr.push(this.numOfAttributes);
    ++this.numOfAttributes;
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
    for (let index = 0; index < this.numOfAttributes; index++) {
      let key = (document.querySelector("#key_" +  index) as any).value;
      this.attributeKeys.push(key);
      
      let value = (document.querySelector("#value_" + index) as any).value;
      this.attributeValues.push(value);
    }
    
    this.web3.connectAccount().then(response => {
      let params: any = {};
      params.name = this.mintForm.value.name;
      params.description = this.mintForm.value.description;
      params.customRouth = this.bundleId;
      params.externalUrl = this.mintForm.value.external_url;
      params.wallet = (response as string[])[0];
      if (this.attributeKeys.length > 0) {
        params.attributeKeys = this.attributeKeys;
        params.attributeValues = this.attributeValues;
      }

      this.http.post(environment.api + `NFT/add`, this.formData, { params: params }).subscribe(result => {
        this.toastr.success("Image minted");
        this.router.navigate([`/bundle/${this.bundleId}`]);
      }, error => {
          console.error(error);
          this.toastr.error("Failed to mint");
          this.attributeKeys = [];
          this.attributeValues = [];
      });
    });
    
    
  }

}
