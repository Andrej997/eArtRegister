import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit {

  nfts: any[] = [1,2,3,4,5,6,7,8,9];

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  openNFT(nftId: any) {

  }

}
