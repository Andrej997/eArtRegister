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
  selector: 'app-create-bundle',
  templateUrl: './create-bundle.component.html',
  styleUrls: ['./create-bundle.component.css']
})
export class CreateBundleComponent implements OnInit {

  bundleForm: FormGroup;
  wallet: any;

  constructor(private fb: FormBuilder, private router: Router,
    private authService: AuthService,
    private http: HttpClient,
    private toastr: ToastrService,
    private metaMaskService: MetaMaskService,
    private cdr: ChangeDetectorRef,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.bundleForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onFirstSubmit() {
    let body = {
      Name: this.bundleForm.value.name,
      Description: this.bundleForm.value.description,
    };

    this.http.post(environment.api + `Bundle/create`, body).subscribe(result => {
      this.toastr.success("Bundle created");
      this.router.navigate([`/bundles/${result}`]);
    }, error => {
        console.error(error);
        this.toastr.error("Failed to create bundle");
    });
  }

}
