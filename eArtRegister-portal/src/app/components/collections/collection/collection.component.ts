import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit, OnDestroy {

  nfts: any[] = [];
  private bundleId = "";
  private routeSub: Subscription;


  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe(params => {
      this.bundleId = params['bundleId'];
      this.getNFTs(this.bundleId);
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
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
