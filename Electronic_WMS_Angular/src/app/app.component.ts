import { Component } from '@angular/core';
import { CustomFunc } from '../assets/js/script.js';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Electronic_WMS_Angular';
  ngOnInit(): void {
    CustomFunc();
  }

  ngAfterViewInit() {
    feather.replace();
  }
}
