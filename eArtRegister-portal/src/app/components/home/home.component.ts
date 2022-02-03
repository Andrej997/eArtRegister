import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  searchInput: string = '';
  users: any[] = [];

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUsers();
  }

  changeSearch(): void {
    this.getUsers();
  }

  private getUsers() {
    let body = {
      InputSearch: this.searchInput,
    };

    this.http.post(environment.api + `Users/search`, body).subscribe(result => {
      console.log(result);
      this.users = result as any[];
    }, error => {
        console.error(error);
    });
  }

  openUser(username: string) {
    console.log(username);
    this.router.navigate([`/user/${username}`]);
  }

}
