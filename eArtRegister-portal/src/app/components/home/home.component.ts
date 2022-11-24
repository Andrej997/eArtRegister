import { HttpClient, HttpParams } from '@angular/common/http';
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
  bundles: any[] = [];

  constructor(private http: HttpClient, 
    private router: Router) { }

  ngOnInit(): void {
    this.getBundles();
  }

  private getBundles() {
    this.http.get(environment.api + `Bundle`).subscribe(result => {
      console.log(result);
      this.bundles = result as any[];
    }, error => {
        console.error(error);
    });
  }

  changeSearch(): void {
    let params = new HttpParams()
      .set('search', this.searchInput)
    this.http.get(environment.api + `Bundle/search`, { params: params }).subscribe(result => {
      console.log(result);
      this.bundles = result as any[];
    }, error => {
        console.error(error);
    });
  }

  openCollection(customRoot: any) {
    this.router.navigate([`/bundle/${customRoot}`]);
  }
}
