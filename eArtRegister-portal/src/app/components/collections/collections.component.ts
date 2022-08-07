import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {

  bundles: any[] = [];

  constructor(private http: HttpClient, private router: Router) { }

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

  openCollection(collectionId: any) {
    //this.router.navigate([`/collections/${this.userId}/collection/${collectionId}`]);
  }

}
