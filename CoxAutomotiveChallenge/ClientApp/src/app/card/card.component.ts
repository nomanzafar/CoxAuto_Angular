import { Component, OnInit, Input } from '@angular/core';
import { Summary } from 'src/models/importResult';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  @Input() summary: Summary[];

  constructor() { }

  ngOnInit() {
  }

}
