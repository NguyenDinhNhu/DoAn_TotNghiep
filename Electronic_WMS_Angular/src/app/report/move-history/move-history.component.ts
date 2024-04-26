import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from 'src/app/inventory/InventoryAPIService';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-move-history',
  templateUrl: './move-history.component.html',
  styleUrls: ['./move-history.component.css']
})
export class MoveHistoryComponent {
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listItem: any[] = [];
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
    this.getAll();
  }

  getAll(): any {
    this.inventoryService.getListInventoryByType({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch,
      Type: 0,
      Status: 2}).subscribe(res => {
        this.listItem = res.listInventory;
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
      Type: 0,
      Status: 2}).subscribe(res => {
      this.listItem = res.listInventory;
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
      Type: 0,
      Status: 2}).subscribe(res => {
      this.listItem = res.listInventory;
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
      Type: 0,
      Status: 2}).subscribe(res => {
      this.listItem = res.listInventory;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
}
