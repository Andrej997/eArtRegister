import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router: Router,) { }

  login(username: string, password: string) {
    let body = {
        username: username,
        password: password
    };
    return this.http.post(environment.api + `Users/login`, body).subscribe(result => {
        this.setData(result);
        this.router.navigate([`/`]);
      }, error => {
          console.error(error);
      });
  }

  logout() {
    this.removeData('user');
  }

  setData(data: any) {
    const jsonData = JSON.stringify(data)
    localStorage.setItem('user', jsonData)
  }
  
  getData() {
      return JSON.parse(localStorage.getItem('user') as string);
  }

  getUserId(): number {
    return JSON.parse(localStorage.getItem('user') as string).id as number;
  }
  
  private removeData(key: any) {
      localStorage.removeItem(key)
  }
}
