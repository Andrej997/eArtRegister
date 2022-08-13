import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  @Output() toggleWallet: EventEmitter<any> = new EventEmitter<any>();

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  logout() {
    this.router.navigate([`/login`]);
    // this.authService.logout();
  }

  toggleWalletDiv(){
    this.toggleWallet.emit();
  }

}
