import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit {

  nfts: any[] = [];
  bundleId = "473a2cb4-c062-4e4a-a71f-c5321ee3ee0a";

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getNFTs(this.bundleId);
  }

  openNFT(nftId: any) {

  }

  mintNFT() {
    this.router.navigate([`/bundles/${this.bundleId}/mint`]);
  }

  private getNFTs(bundleId: string) {
    this.http.get(environment.api + `NFT/bundle/` + bundleId).subscribe(result => {
      console.log(result);
      this.nfts = result as any[];
    }, error => {
        console.error(error);
    });
  }

}
