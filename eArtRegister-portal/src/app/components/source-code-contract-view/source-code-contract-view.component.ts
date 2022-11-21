import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-source-code-contract-view',
  templateUrl: './source-code-contract-view.component.html',
  styleUrls: ['./source-code-contract-view.component.css']
})
export class SourceCodeContractViewComponent implements OnInit {

  @Input() contract;

  constructor() { }

  ngOnInit(): void {
  }

}
