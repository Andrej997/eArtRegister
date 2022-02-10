import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  userId: string = 'admin';

  constructor(public authGuard: AuthGuard, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  logout() {
    this.router.navigate([`/login`]);
    this.authService.logout();
  }

}
