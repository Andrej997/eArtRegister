import { HttpClient, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Web3Service } from 'src/app/services/contract/web3.service';
// import { ToastrService } from 'ngx-toastr';
// import { AuthGuard } from 'src/app/guards/auth.guard';
// import { AuthService } from 'src/app/services/auth.service';
// import { MetaMaskService } from 'src/app/services/meta-mask.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-bundle',
  templateUrl: './create-bundle.component.html',
  styleUrls: ['./create-bundle.component.css']
})
export class CreateBundleComponent implements OnInit {

  bundleForm: FormGroup;
  wallet: any;

  constructor(private fb: FormBuilder, private router: Router,
    private web3: Web3Service,
    private http: HttpClient,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.bundleForm = this.fb.group({
      customRoute: ['', Validators.required],
      name: ['', Validators.required],
      symbol: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onFirstSubmit() {
    this.web3.connectAccount().then(response => {
      let body = {
        CustomRoute: this.bundleForm.value.customRoute,
        Name: this.bundleForm.value.name,
        Symbol: this.bundleForm.value.symbol,
        Description: this.bundleForm.value.description,
        Wallet: (response as string[])[0]
      };
  
      this.http.post(environment.api + `Bundle/create`, body).subscribe(result => {
        this.toastr.success("Bundle created");
        this.router.navigate([`/bundles/${(result as any).customRoute}`]);
      }, error => {
          console.error(error);
          this.toastr.error("Failed to create bundle");
      });
    });
  }

}
