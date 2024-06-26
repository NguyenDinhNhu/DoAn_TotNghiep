import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';
import { ProductAPIService } from '../ProductAPIService';
import { AuthAPIService } from 'src/app/login/AuthAPIService';
import { BrandAPIService } from 'src/app/brand/BrandAPIService';
import { CategoryAPIService } from 'src/app/category/CategoryAPIService';

@Component({
  selector: 'app-list-product',
  templateUrl: './list-product.component.html',
  styleUrls: ['./list-product.component.css']
})
export class ListProductComponent {
  // Search - Get List 
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listProduct: any[] = [];
  public totalItem: number = 0;
  public productPrice: number = 0;

  public BrandId: number = 0;
  public CateId: number = 0;
  public CheckStock: number = 0;
  // combobox
  public brandCombobox: any[] = [ 
    { brandName: 'All', brandId: 0}];
  public categoryCombobox: any[] = [ 
    { cateName: 'All', cateId: 0}];
  
  public statusProductCombobox: any[] = [ 
    { statusName: 'All', checkStock: 0},
    { statusName: 'Out of stock', checkStock: 1},
    { statusName: 'Stocking', checkStock: 2},
  ];
  
  formatVND(productPrice: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(productPrice);
  }

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private productService: ProductAPIService,
    private brandService: BrandAPIService,
    private categoryService: CategoryAPIService,
    private authAPIService: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }


  ngOnInit(): void {
    feather.replace();
    this.getAllProduct();
    this.getBrandCombobox();
    this.getCategoryCombobox();
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

  getBrandCombobox(): any {
    this.brandService.getBrandCombobox().subscribe(res => {
      res.unshift(this.brandCombobox[0]) // unshift: thêm vào đầu danh sách
      this.brandCombobox = res;
    })
  }

  getCategoryCombobox(): any {
    this.categoryService.getCategoryCombobox().subscribe(res => {
      res.unshift(this.categoryCombobox[0]) // unshift: thêm vào đầu danh sách
      this.categoryCombobox = res;
    })
  }

  // delete
  deleteProduct(id: number): void {
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
        this.productService.deleteProduct(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllProduct();
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

  getAllProduct(): any {
    this.productService.getListProduct({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listProduct = res.listProduct;
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
    this.productService.getListProduct({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listProduct = res.listProduct;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.productService.getListProduct({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listProduct = res.listProduct;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.productService.getListProduct({
      PageSize: this.pageSize, 
      CurrentPage: this.currentPage, 
      TextSearch: this.textSearch ? this.textSearch.trim() : '',
      BrandId: this.BrandId,
      CateId: this.CateId,
      CheckStock: this.CheckStock
    }).subscribe(res => {
      this.listProduct = res.listProduct;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }
}
