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
  public SourceLocation!: number;
  public WarehouseId!: number;

  // table edit
  public ListInventoryLine: any[] = []; 
  public Quantity!: number;
  public ProductId!: number;

  public ListSerial: any[] = []; 
  public SerialNumber!: string;

  // Combobox
  public ListSupplierCombobox: any[] = [];
  public ListWHCombobox: any[] = [];
  public ListProductCombobox: any[] = [];

  addReceipt = this.fb.group({
    SourceLocation: ['', Validators.required],
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
    let row = {ProductId: 0, Quantity: 0, ListSerialNumber: [], submited: false};
    this.ListInventoryLine.push(row);
  }

  deleteRow(index:any) {
    this.ListInventoryLine.splice(index, 1);
  }

  hasDuplicateProductIds(): boolean {
    const productIds: number[] = [];
    for (const row of this.ListInventoryLine) {
      if (productIds.includes(row.ProductId)) {
        return true; // Nếu đã tồn tại ProductId trong mảng, trả về true
      }
      productIds.push(row.ProductId); // Nếu chưa, thêm ProductId vào mảng
    }
    return false; // Không có ProductId nào trùng nhau
  }

  onSubmit(): void {
    this.submited = true;
    let hasDuplicateSerial = false;

    this.ListInventoryLine.forEach(row => {
      if (!row.ProductId || !row.Quantity || row.ListSerialNumber.length != row.Quantity) {
        row.submited = true;
      } else {
        row.submited = false; // Reset submited for valid rows
      }

      let serialNumber = new Set(); // Set để lưu trữ các serial number đã xuất hiện trong dòng
      row.ListSerialNumber.forEach(e => {
        if (!e.SerialNumber) {
          row.submited = true;
        } else {
          e.submited = false; // Reset submited for valid rows
        }

        if (serialNumber.has(e.SerialNumber)) {
          hasDuplicateSerial = true;
        } else {
          serialNumber.add(e.SerialNumber); // Thêm serial number vào set
        }
      });
    });

    // Kiểm tra xem có ProductId nào trùng nhau không
    const hasDuplicateProductId = this.hasDuplicateProductIds();
    if (hasDuplicateProductId) {
      this.toastr.error('Product Id must be unique for each row', 'Error');
      return;
    }

    // Nếu có serial number trùng nhau, hiển thị thông báo và không thực hiện hành động tiếp theo
    if (hasDuplicateSerial) {
      this.toastr.error('Duplicate serial numbers are not allowed in the same row.', 'Error');
      return;
    }
    
    // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
    if (this.ListInventoryLine.some(row => row.submited)) {
      this.toastr.error('Please fill out all fields in each row of the list product table & list serial number and quantity must be > 0', 'Error');
      return;
    }

    if(!this.addReceipt.invalid){
      console.log(this.addReceipt.value);
      this.SourceLocation = parseInt(this.addReceipt.value.SourceLocation!);
      this.WarehouseId = parseInt(this.addReceipt.value.WareHouseId!);
      
      console.log(this.SourceLocation, this.WarehouseId, this.ListInventoryLine);
      //add
      this.inventoryService.insertInventory({
        SourceLocation: this.SourceLocation,
        WareHouseId: this.WarehouseId,
        Type: 1,
        ListInventoryLine: this.ListInventoryLine
      }).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/receipts/index']).then(() => {
            this.toastr.success(res.statusMessage, "Success");
          });
        }
        else if(res.statusCode == 400){
          this.toastr.warning(res.statusMessage, "Warning");
        }
        else if(res.statusCode == 500){
          this.toastr.error(res.statusMessage, "Error");
        }
      })
    }
  }

  // Popup Modal
  OpenModal(row: any) {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    this.ListSerial = row.ListSerialNumber;
    if (this.ListSerial.length < row.Quantity) {
      for (let i = this.ListSerial.length; i < row.Quantity; i++) {
        let row = {SerialNumber: '', submited: false};
        this.ListSerial.push(row);
      }
    } else if (this.ListSerial.length > row.Quantity) {
      this.ListSerial = this.ListSerial.slice(0, row.Quantity);
    }
    console.log(this.ListSerial)
  }

  CloseModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
}
