import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from '../InventoryAPIService';
import { UserAPIService } from 'src/app/users/UserAPIService';
import { ProductAPIService } from 'src/app/product/ProductAPIService';
import { WareHouseAPIService } from 'src/app/warehouse/WareHouseAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-inventory-detail',
  templateUrl: './inventory-detail.component.html',
  styleUrls: ['./inventory-detail.component.css']
})
export class InventoryDetailComponent {
  public detailInventory!: any;
  public InventoryId!: number;
  public SourceLocation!: number;
  public WarehouseId!: number;

  // table edit
  public ListInventoryLine: any[] = []; 
  public Quantity!: number;
  public ProductId!: number;
  public Price!: number;

  public ListSerial: any[] = []; 
  public SerialNumber!: string;

  
  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private inventoryService: InventoryAPIService,
    private userService: UserAPIService,
    private warehouseService: WareHouseAPIService,
    private productService: ProductAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getDetail();
  }
  
  formatVND(productPrice: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(productPrice);
  }
  getDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("inventoryId"));   // lay id
      let id = query.get("inventoryId");
      if (id != null) {
        let receiptId = parseInt(id);
        this.inventoryService.getInventory(receiptId).subscribe(res => {
          res.listInventoryLine.forEach(e => {
            const ListSeri: any[] = [];
            e.listSerialNumber.forEach(ele => {
              const r = {
                SerialId: ele.serialId,
                SerialNumber: ele.serialNumber
              }
              ListSeri.push(r)
            })
            const row = {
                InventoryLineId: e.inventoryLineId,
                ProductId: e.productId,
                ProductName: e.productName,
                Quantity: e.quantity,
                Price: e.price,
                ListSerialNumber: ListSeri
              }
            this.ListInventoryLine.push(row)
          });
          this.detailInventory = res
        });
      }
      else if (id == null) {
        this.toastr.warning("Receipt not found!", "Warning")
      }
    })
  }
  
  // Popup Modal
  OpenModal(row: any) {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    this.ListSerial = row.ListSerialNumber;
  }

  CloseModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
}
