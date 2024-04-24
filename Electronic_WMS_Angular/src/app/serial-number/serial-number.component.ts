import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import * as feather from 'feather-icons';
import { ToastrService } from 'ngx-toastr';
import { SerialNumberAPIService } from './SerialNumberAPIService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-serial-number',
  templateUrl: './serial-number.component.html',
  styleUrls: ['./serial-number.component.css']
})
export class SerialNumberComponent {
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listSerial: any[] = [];
  public totalItem: number = 0;
  public submitedEdit: boolean = false;
  public SerialId!: number;
  public WareHouseId!: number;
  public Location!: string;
  public SerialNumber!: string;

  editForm = this.fb.group({
    SerialId: ['', Validators.required],
    SerialNumber: ['', Validators.required],
    WareHouseId: ['', Validators.required],
    Location: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private seriService: SerialNumberAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get v() {return this.editForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllSerial();
  }
  
  getAllSerial(): any {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.seriService.getListSerialByProductId({
          PageSize: this.pageSize,
          CurrentPage: this.currentPage,
          TextSearch: this.textSearch,
          ProductId: productId
        }).subscribe(res => {
          this.listSerial = res.listSerial;
          this.totalItem = res.total;
          setTimeout(() => {
            feather.replace();
          }, 10)
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  // pagination
  changePage(page: number): void{
    this.currentPage = page;    
    console.log(this.currentPage);
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.seriService.getListSerialByProductId({
          PageSize: this.pageSize,
          CurrentPage: this.currentPage,
          TextSearch: this.textSearch,
          ProductId: productId
        }).subscribe(res => {
          this.listSerial = res.listSerial;
          this.totalItem = res.total;
          setTimeout(() => {
            feather.replace();
          }, 10)
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.seriService.getListSerialByProductId({
          PageSize: this.pageSize,
          CurrentPage: this.currentPage,
          TextSearch: this.textSearch,
          ProductId: productId
        }).subscribe(res => {
          this.listSerial = res.listSerial;
          this.totalItem = res.total;
          setTimeout(() => {
            feather.replace();
          }, 10)
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  search(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("productId"));   // lay id
      let id = query.get("productId");
      if (id != null) {
        let productId = parseInt(id);
        this.seriService.getListSerialByProductId({
          PageSize: this.pageSize,
          CurrentPage: this.currentPage,
          TextSearch: this.textSearch.trim(),
          ProductId: productId
        }).subscribe(res => {
          this.listSerial = res.listSerial;
          this.totalItem = res.total;
          setTimeout(() => {
            feather.replace();
          }, 10)
        });
      }
      else if (id == null) {
        this.toastr.warning("Product not found!", "Warning")
      }
    })
  }

  // Edit 
  editSerial(): void {
    this.submitedEdit = true;
    
    if(!this.editForm.invalid){
      console.log(this.editForm.value);
      this.SerialId = parseInt(this.editForm.value.SerialId!);
      this.WareHouseId = parseInt(this.editForm.value.WareHouseId!);
      this.SerialNumber = this.editForm.value.SerialNumber?.trim()!;
      this.Location = this.editForm.value.Location?.trim()!;
      //call api update 
      this.seriService.updateLocation( [{ 
        SerialId: this.SerialId,
        WareHouseId: this.WareHouseId,
        Location: this.Location }]).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedEdit = false;
            this.getAllSerial();
            this.CloseModalEdit();
          }
          else if (res.statusCode = 400){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 404){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
      });
    }
  }


  OpenModalEdit(id: number) {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.seriService.getSerial(id).subscribe(res => {
      this.editForm = this.fb.group({
        SerialId: [res.serialId, Validators.required],
        SerialNumber: [res.serialNumber, Validators.required],
        WareHouseId: [res.wareHouseId, Validators.required],
        Location: [res.location, Validators.required]
      });
    })
  }

  CloseModalEdit() {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
}
