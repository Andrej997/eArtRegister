import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit {

  collections: any[] = [1,2,3,4,5,6,7,8,9];

  private userId: string = 'admin';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  openCollection(collectionId: any) {
    this.router.navigate([`/collections/${this.userId}/collection/${collectionId}`]);
  }

}
