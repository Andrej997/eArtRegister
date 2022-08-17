import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Web3Service } from 'src/app/services/contract/web3.service';
import { environment } from 'src/environments/environment';
import { ScrollDispatcher } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-my-system-history',
  templateUrl: './my-system-history.component.html',
  styleUrls: ['./my-system-history.component.css']
})
export class MySystemHistoryComponent implements OnInit {

  myEvents: any[] = [];
  toWallet: string = "";
  wallet = "";

  constructor(private http: HttpClient, 
    private web3: Web3Service) { }

  ngOnInit(): void {
    this.web3.connectAccount().then(response => {
      this.wallet = (response as string[])[0].toLowerCase();
      this.getUserSystemHistory();
    });
    
  }

  private getUserSystemHistory(){
    this.http.get(environment.api + `Users/actions/` + this.wallet).subscribe(result => {
      this.myEvents = result as any[];
    }, error => {
        console.error(error);
    });
  }

}
