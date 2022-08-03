import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import detectEthereumProvider from '@metamask/detect-provider';
import { switchMap } from 'rxjs/operators';
import { from } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MetaMaskService } from 'src/app/services/meta-mask.service';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  user: any;

  constructor(private metaMaskService: MetaMaskService, private cdr: ChangeDetectorRef) { }

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
      console.log(res)
      this.user = res[0].caveats[0].value[0];
      this.cdr.detectChanges();
    })
  }

}
