import { Component } from '@angular/core';
import * as feather from 'feather-icons';
import { BrandAPIService } from '../BrandAPIService';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { AuthAPIService } from 'src/app/login/AuthAPIService';

@Component({
  selector: 'app-list-brand',
  templateUrl: './list-brand.component.html',
  styleUrls: ['./list-brand.component.css']
})
export class ListBrandComponent {
  // Search - Get List Brand
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listBrand: any[] = [];
  public totalItem: number = 0;
  // combobox
  public brandParentCombobox: any[] = [ 
    { brandName: 'Highest Level', brandId: 0}];
  
  // Form add 
  public brandName!: string;
  public parentLevel!: number;
  public submitedAdd: boolean = false;

  addBrandForm = this.fb.group({
    BrandName: ['', Validators.required],
    ParentLevel: ['', Validators.required],
  });

  // Form edit
  public brandId!: number;
  public submitedEdit: boolean = false;

  editBrandForm = this.fb.group({
    BrandId: ['', Validators.required],
    BrandName: ['', Validators.required],
    ParentLevel: ['', Validators.required],
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private brandService: BrandAPIService,
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addBrandForm.controls;}
  get v() {return this.editBrandForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllBrand();
    this.getParentBrandCombobox();
  }

  isAdmin(): boolean {
    const token = this.authAPIService.getToken();
    if (token) {
      const parsedToken = this.authAPIService.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator';
    }
    return false;
  }

  // Add Brand
  addBrand(): void {
    this.submitedAdd = true;
    
    if(!this.addBrandForm.invalid){
      console.log(this.addBrandForm.value);
      this.brandName = this.addBrandForm.value.BrandName!.trim();
      this.parentLevel = parseInt(this.addBrandForm.value.ParentLevel!);
      //call api add brand
      this.brandService.insertBrand( { 
        BrandName: this.brandName,
        ParentId: this.parentLevel }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllBrand();
            this.CloseModal();
          }
          else if (res.statusCode = 400){
            this.toastr.warning(res.statusMessage, "Warning");
          }
          else if (res.statusCode = 500) {
            this.toastr.error(res.statusMessage, "Error");
          }
      });
    }
  }

  // Edit Brand
  editBrand(): void {
    this.submitedEdit = true;
    
    if(!this.editBrandForm.invalid){
      console.log(this.editBrandForm.value);
      this.brandId = parseInt(this.editBrandForm.value.BrandId!);
      this.brandName = this.editBrandForm.value.BrandName?.trim()!;
      this.parentLevel = parseInt(this.editBrandForm.value.ParentLevel!);
      //call api update brand
      this.brandService.updateBrand( { 
        BrandId: this.brandId,
        BrandName: this.brandName,
        ParentId: this.parentLevel }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllBrand();
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

  // Delete Brand
  deleteBrand(id: number): void {
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
        this.brandService.deleteBrand(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllBrand();
            this.CloseModalEdit();
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

  getAllBrand(): any {
    this.brandService.getListBrand({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listBrand = res.listBrand;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    }, error => {
      this.toastr.error("Loading data fail!", "Error");
    })
  }

  getParentBrandCombobox(): any {
    this.brandService.getParentBrandCombobox().subscribe(res => {
      res.unshift(this.brandParentCombobox[0]) // unshift: thêm vào đầu danh sách
      this.brandParentCombobox = res;
    })
  }
  // pagination
  changePage(page: number): void{
    this.currentPage = page;    
    console.log(this.currentPage);
    this.brandService.getListBrand({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listBrand = res.listBrand;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.brandService.getListBrand({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listBrand = res.listBrand;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.brandService.getListBrand({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listBrand = res.listBrand;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  // Popup Modal Add Brand
  OpenModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.addBrandForm.reset();
    this.getParentBrandCombobox();
  }

  CloseModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "none";
    }
  }
  
  OpenModalEdit(id: number) {
    const modelDiv = document.getElementById("editModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }

    this.getParentBrandCombobox();
    
    this.brandService.getBrand(id).subscribe(res => {
      this.editBrandForm = this.fb.group({
        BrandId: [res.brandId, Validators.required],
        BrandName: [res.brandName, Validators.required],
        ParentLevel: [res.parentId, Validators.required]
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
