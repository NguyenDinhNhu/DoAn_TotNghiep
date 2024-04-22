import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { InventoryAPIService } from '../InventoryAPIService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-list-deliveries',
  templateUrl: './list-deliveries.component.html',
  styleUrls: ['./list-deliveries.component.css']
})
export class ListDeliveriesComponent {
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listDeliveries: any[] = [];
  public totalItem: number = 0;
  public status: number = 0;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private inventoryService: InventoryAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }


  ngOnInit(): void {
    feather.replace();
    this.getAllDeliveries();
  }

  // delete
  deleteDeliveries(id: number): void {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Confirm"
    }).then((result) => {
      if (result.isConfirmed) {
        this.inventoryService.deleteInventory(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllDeliveries();
          }
          else if (res.statusCode = 404){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
        })
      }
    });
  }

  getAllDeliveries(): any {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 2,
      Status: 0}).subscribe(res => {
        this.listDeliveries = res.listInventory;
        this.totalItem = res.total;
        setTimeout(() => {
          feather.replace();
        }, 10)
    }, error => {
      this.toastr.error("Loading data fail!", "Error");
    })
  }

  // pagination
  changePage(page: number): void{
    this.currentPage = page;    
    console.log(this.currentPage);
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage,
      TextSearch: this.textSearch,
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  filterByStatus(event: any) {
    this.status = event.target.value;
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage,
      TextSearch: this.textSearch,
      Type: 1,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, CurrentPage: this.currentPage, 
      TextSearch: this.textSearch.trim(),
      Type: 2,
      Status: this.status}).subscribe(res => {
      this.listDeliveries = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
}
