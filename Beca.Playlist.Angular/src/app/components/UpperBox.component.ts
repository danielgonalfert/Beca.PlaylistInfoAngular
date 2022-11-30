import { Component, Input } from '@angular/core';


@Component({
  selector: 'upperBox',
  templateUrl: './templates/upperBox.component.html',
  styleUrls: ['./styles/upperBox.component.css']
})

export default class upperBox {
  title = "SpotiMine";

  @Input()
  section = "";

} 
