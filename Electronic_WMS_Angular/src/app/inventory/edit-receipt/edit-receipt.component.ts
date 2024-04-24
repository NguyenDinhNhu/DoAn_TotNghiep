import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { InventoryAPIService } from '../InventoryAPIService';
import { UserAPIService } from 'src/app/users/UserAPIService';
import { WareHouseAPIService } from 'src/app/warehouse/WareHouseAPIService';
import { ProductAPIService } from 'src/app/product/ProductAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-edit-receipt',
  templateUrl: './edit-receipt.component.html',
  styleUrls: ['./edit-receipt.component.css']
})
export class EditReceiptComponent {
  public submited: boolean = false;
  public InventoryId!: number;
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

  editReceipt = this.fb.group({
    InventoryId: ['', Validators.required],
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
    this.getReceiptDetail();
    this.getListWHCombobox();
    this.getListCateCombobox();
    this.getListUserCombobox();
  }

  getReceiptDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("receiptId"));   // lay id
      let id = query.get("receiptId");
      if (id != null) {
        let receiptId = parseInt(id);
        this.inventoryService.getInventory(receiptId).subscribe(res => {
          // this.rows = res.listProductFeature;
          res.listInventoryLine.forEach(e => {
            const ListSeri: any[] = [];
            e.listSerialNumber.forEach(ele => {
              const r = {
                SerialId: ele.serialId,
                SerialNumber: ele.serialNumber,
                submited: false
              }
              ListSeri.push(r)
            })
            const row = {
                InventoryLineId: e.inventoryLineId,
                ProductId: e.productId,
                Quantity: e.quantity,
                ListSerialNumber: ListSeri,
                submited: false,
                shouldDisableCombo: true,
              }
            this.ListInventoryLine.push(row)
          });
          this.editReceipt = this.fb.group({
            InventoryId: [res.inventoryId, Validators.required],
            SourceLocation: [res.sourceLocation, Validators.required],
            WareHouseId: [res.wareHouseId, Validators.required]
          });
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
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

  get f() {return this.editReceipt.controls;}
  
  addRow() {
    let row = {InventoryLineId: 0, ProductId: 0, Quantity: 0, ListSerialNumber: [], submited: false, shouldDisableCombo: false};
    this.ListInventoryLine.push(row);
  }

  deleteRow(index:any) {
    this.ListInventoryLine.splice(index, 1);
  }


  onSubmit(): void {
    this.submited = true;

    const previousQuantities = this.ListInventoryLine.map(row => row.Quantity);

    this.ListInventoryLine.forEach((row, index) => {
      if (!row.ProductId || !row.Quantity || row.ListSerialNumber.length != row.Quantity) {
        row.submited = true;
      } else {
        row.submited = false; // Reset submited for valid rows
      }

      row.ListSerialNumber.forEach(e => {
        if (!e.SerialNumber) {
          row.submited = true;
        } else {
          e.submited = false; // Reset submited for valid rows
        }
      });

      // Kiểm tra số lượng của các dòng input
      if (row.Quantity < previousQuantities[index]) {
        row.submited = true;
        this.toastr.error('Quantity cannot be decreased', 'Error');
      }

    });

    // Nếu bất kỳ dòng nào không hợp lệ, không thực hiện gửi form và hiển thị thông báo lỗi
    if (this.ListInventoryLine.some(row => row.submited)) {
      this.toastr.error('Please fill out all fields in each row of the list product table & list serial number and quantity must be > 0', 'Error');
      return;
    }

    // if(!this.editReceipt.invalid){
    //   console.log(this.editReceipt.value);
    //   this.InventoryId = parseInt(this.editReceipt.value.InventoryId!);
    //   this.SourceLocation = parseInt(this.editReceipt.value.SourceLocation!);
    //   this.WarehouseId = parseInt(this.editReceipt.value.WareHouseId!);
      
    //   console.log(this.SourceLocation, this.WarehouseId, this.ListInventoryLine);
    //   //edit
    //   this.inventoryService.updateInventory({
    //     InventoryId: this.InventoryId,
    //     SourceLocation: this.SourceLocation,
    //     WareHouseId: this.WarehouseId,
    //     Type: 1,
    //     ListInventoryLine: this.ListInventoryLine
    //   }).subscribe(res => {
    //     if(res.statusCode == 200){
    //       this.submited = false;
    //       this.router.navigate(['/receipts/index']).then(() => {
    //         this.toastr.success(res.statusMessage, "Success");
    //       });
    //     }
    //     else if(res.statusCode == 404){
    //       this.toastr.warning(res.statusMessage, "Warning");
    //     }
    //     else if(res.statusCode == 400){
    //       this.toastr.warning(res.statusMessage, "Warning");
    //     }
    //     else if(res.statusCode == 500){
    //       this.toastr.error(res.statusMessage, "Error");
    //     }
    //   })
    // }
  }

  // Popup Modal
  OpenModal(row: any) {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    this.ListSerial = row.ListSerialNumber;
    for (let i = this.ListSerial.length; i < row.Quantity; i++) {
      let row = {SerialId: 0, SerialNumber: '', submited: false};
      this.ListSerial.push(row);
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
