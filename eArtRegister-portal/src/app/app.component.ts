import {Component, ViewEncapsulation} from '@angular/core';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [
    trigger("enterAnimation", [
      transition(":enter", [
        style({ transform: "translateX(100%)", opacity: 0 }),
        animate("200ms", style({ transform: "translateX(0)", opacity: 1 })),
      ]),
      transition(":leave", [
        style({ transform: "translateX(0)", opacity: 1 }),
        animate("200ms", style({ transform: "translateX(100%)", opacity: 0 })),
      ]),
    ])
  ],
})
export class AppComponent {
  title = 'eART register';
  authenticated: boolean = false;
  data: string[] | undefined;

  showWallet: boolean = false;
  
  toggleWallet(){
    this.showWallet = !this.showWallet;
  }

  constructor() {
  }

}
