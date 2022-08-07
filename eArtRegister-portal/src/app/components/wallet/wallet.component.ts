import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import detectEthereumProvider from '@metamask/detect-provider';
import { switchMap } from 'rxjs/operators';
import { from } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MetaMaskService } from 'src/app/services/meta-mask.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  user: any;

  constructor(private metaMaskService: MetaMaskService, private cdr: ChangeDetectorRef, private http: HttpClient,) { }

  ngOnInit(): void {
    this.checkConnected();
  }

  checkConnected(){
    this.metaMaskService.isConnected().subscribe( (res: any) => {
      this.user = res.accounts[0]
      res.provider.on('accountsChanged', (accounts: any) => {
        if (accounts.length == 0) {
          this.user = undefined;
        }else {
          this.user = accounts[0];
        }
        this.cdr.detectChanges();
      });
    })
  }

  signInWithMetaMask(){
    this.metaMaskService.signInWithMetaMask().subscribe( res => {
      this.checkWallet(res[0].caveats[0].value[0]);
      this.user = res[0].caveats[0].value[0];
      this.cdr.detectChanges();
    })
  }

  private checkWallet(wallet: string) {
    let body = {
      Wallet: wallet,
    };

    this.http.post(environment.api + `Users/checkWallet`, body).subscribe(result => {
    }, error => {
        console.error(error);
    });
  }
}
