import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from '../InventoryAPIService';
import { UserAPIService } from 'src/app/users/UserAPIService';
import { WareHouseAPIService } from 'src/app/warehouse/WareHouseAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductAPIService } from 'src/app/product/ProductAPIService';

@Component({
  selector: 'app-add-receipt',
  templateUrl: './add-receipt.component.html',
  styleUrls: ['./add-receipt.component.css']
})
export class AddReceiptComponent {
  public submited: boolean = false;
  public SourceLocation!: Number;
  public WarehouseId!: number;

  // table edit
  public ListInventoryLine: any[] = []; 
  public Quantity!: number;
  public ProductId!: number;

  // Combobox
  public ListSupplierCombobox: any[] = [];
  public ListWHCombobox: any[] = [];
  public ListProductCombobox: any[] = [];

  addReceipt = this.fb.group({
    UserId: ['', Validators.required],
    WareHouseId: ['', Validators.required]
  });
  
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
    this.getListWHCombobox();
    this.getListCateCombobox();
    this.getListUserCombobox();
  }

  // Get List combobox Supplier
  getListUserCombobox(): void {
    this.userService.getListSupplierOrShop(3).subscribe(res => {
      this.ListSupplierCombobox = res;
    })
  }

  getListWHCombobox(): void {
    this.warehouseService.getWareHouseCombobox().subscribe(res => {
      this.ListWHCombobox = res;
    })
  }

  getListCateCombobox(): void {
    this.productService.getListProductCombobox().subscribe(res => {
      this.ListProductCombobox = res;
    })
  }

  get f() {return this.addReceipt.controls;}
  
  addRow() {
    let row = {ProductId: 0, Quantity: 0, submited: false};
    this.ListInventoryLine.push(row);
  }

  deleteRow(index:any) {
    this.ListInventoryLine.splice(index, 1);
  }


  onSubmit(): void {
    this.submited = true;

    this.ListInventoryLine.forEach(row => {
      if (!row.ProductId || !row.Quantity) {
        row.submited = true;
      } else {
        row.submited = false; // Reset submited for valid rows
      }
    });

    // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
    if (this.ListInventoryLine.some(row => row.submited)) {
      this.toastr.error('Please fill out all fields in each row of the list product table and quantity must be > 0', 'Error');
      return;
    }

    if(!this.addReceipt.invalid){
      console.log(this.addReceipt.value);
      this.SourceLocation = parseInt(this.addReceipt.value.UserId!);
      this.WarehouseId = parseInt(this.addReceipt.value.WareHouseId!);
      
      console.log(this.SourceLocation, this.WarehouseId, this.ListInventoryLine);
      //add
      // this.inventoryService.insertInventory({
      //   SourceLocation: this.SourceLocation,
      //   WareHouseId: this.WarehouseId,
      //   Type: 1,
      //   ListInventoryLine: this.ListInventoryLine
      // }).subscribe(res => {
      //   if(res.statusCode == 200){
      //     this.submited = false;
      //     this.router.navigate(['/product/index']).then(() => {
      //       this.toastr.success(res.statusMessage, "Success");
      //     });
      //   }
      //   else if(res.statusCode == 400){
      //     this.toastr.warning(res.statusMessage, "Warning");
      //   }
      //   else if(res.statusCode == 500){
      //     this.toastr.error(res.statusMessage, "Error");
      //   }
      // })
    }
  }
}
