import { Component } from '@angular/core';
import { InventoryAPIService } from '../inventory/InventoryAPIService';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  public CountReceiptReady: number = 0;
  public CountDeliveryReady: number = 0;
  public CountProductOutOfStock: number = 0;

  constructor(
    private toastr: ToastrService,
    private inventoryService: InventoryAPIService,
    private router: Router
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getDashBoardVM();
  }

  getDashBoardVM(): any {
    this.inventoryService.getDashBoardVM().subscribe(res => {
        this.CountDeliveryReady = res.countDeliveryReady;
        this.CountReceiptReady = res.countReciptReady;
        this.CountProductOutOfStock = res.countProductOutOfStock;
        setTimeout(() => {
          feather.replace();
        }, 10)
    }, error => {
      this.toastr.error("Loading data fail!", "Error");
    })
  }
}
