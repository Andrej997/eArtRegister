import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-my-bundles',
  templateUrl: './my-bundles.component.html',
  styleUrls: ['./my-bundles.component.css']
})
export class MyBundlesComponent implements OnInit {

  bundles: any[] = [];
  wallet = "";

  constructor(private http: HttpClient, 
    private web3: Web3Service,
    private router: Router) { }

  ngOnInit(): void {
    this.web3.connectAccount().then(response => {
      this.wallet = (response as string[])[0].toLowerCase();
      this.getBundles();
    });
    
  }

  private getBundles() {
    this.http.get(environment.api + `Bundle/mine/` + this.wallet).subscribe(result => {
      this.bundles = result as any[];
    }, error => {
        console.error(error);
    });
  }

  openCollection(bundleId: any) {
    this.router.navigate([`/bundles/${bundleId}`]);
  }
  
}
