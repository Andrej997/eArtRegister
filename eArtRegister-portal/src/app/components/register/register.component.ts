import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: UntypedFormGroup;

  constructor(private fb: UntypedFormBuilder,
    private authService: AuthService,
    private http: HttpClient,
    private toastr: ToastrService,
    public authGuard: AuthGuard) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      name: ['', Validators.required],
      surname: ['', Validators.required],
    });
  }

  onFirstSubmit() {
    let body = {
      Username: this.registerForm.value.username,
      Password: this.registerForm.value.password,
      Name: this.registerForm.value.name,
      Surname: this.registerForm.value.surname,
    };

    this.http.post(environment.api + `Users/register`, body).subscribe(result => {
      this.authService.login(this.registerForm.value.username, this.registerForm.value.password);
    }, error => {
        console.error(error);
        this.toastr.error(error.error);
    });
  }

}
