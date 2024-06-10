import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CategoryAPIService } from './CategoryAPIService';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';
import { AuthAPIService } from '../login/AuthAPIService';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent {
  // Search - Get List Category
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listCategory: any[] = [];
  public totalItem: number = 0;
  // combobox
  public cateParentCombobox: any[] = [ 
    { cateName: 'Highest Level', cateId: 0}];

  public cateName!: string;
  public parentLevel!: number;
  public cateId!: number;
  // Form add 
  public submitedAdd: boolean = false;

  addCateForm = this.fb.group({
    CateName: ['', Validators.required],
    ParentLevel: ['', Validators.required],
  });

  // Form edit
  public submitedEdit: boolean = false;

  editCateForm = this.fb.group({
    CateId: ['', Validators.required],
    CateName: ['', Validators.required],
    ParentLevel: ['', Validators.required],
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private categoryService: CategoryAPIService,
    private authAPISerivce: AuthAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addCateForm.controls;}
  get v() {return this.editCateForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllCategory();
    this.getParentCategoryCombobox();
  }

  isAdmin(): boolean {
    const token = this.authAPISerivce.getToken();
    if (token) {
      const parsedToken = this.authAPISerivce.parseJwt(token);
      const role = parsedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role === 'Administrator';
    }
    return false;
  }

  // Add 
  addCategory(): void {
    this.submitedAdd = true;
    
    if(!this.addCateForm.invalid){
      console.log(this.addCateForm.value);
      this.cateName = this.addCateForm.value.CateName!.trim();
      this.parentLevel = parseInt(this.addCateForm.value.ParentLevel!);
      //call api add category
      this.categoryService.insertCategory( { 
        CateName: this.cateName,
        ParentId: this.parentLevel }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllCategory();
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

  // Edit 
  editCategory(): void {
    this.submitedEdit = true;
    
    if(!this.editCateForm.invalid){
      console.log(this.editCateForm.value);
      this.cateId = parseInt(this.editCateForm.value.CateId!);
      this.cateName = this.editCateForm.value.CateName?.trim()!;
      this.parentLevel = parseInt(this.editCateForm.value.ParentLevel!);
      //call api update category
      this.categoryService.updateCategory( { 
        CateId: this.cateId,
        CateName: this.cateName,
        ParentId: this.parentLevel }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllCategory();
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

  // Delete 
  deleteCategory(id: number): void {
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
        this.categoryService.deleteCategory(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllCategory();
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

  getAllCategory(): any {
    this.categoryService.getListCategory({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listCategory = res.listCate;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    }, error => {
      this.toastr.error("Loading data fail!", "Error");
    })
  }

  getParentCategoryCombobox(): any {
    this.categoryService.getParentCategoryCombobox().subscribe(res => {
      res.unshift(this.cateParentCombobox[0]) // unshift: thêm vào đầu danh sách
      this.cateParentCombobox = res;
    })
  }
  // pagination
  changePage(page: number): void{
    this.currentPage = page;    
    console.log(this.currentPage);
    this.categoryService.getListCategory({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listCategory = res.listCate;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.categoryService.getListCategory({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listCategory = res.listCate;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.categoryService.getListCategory({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listCategory = res.listCate;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  // Popup Modal Add   
  OpenModal() {
    const modelDiv = document.getElementById("myModal");
    if(modelDiv != null) {
      modelDiv.style.display = "block";
      modelDiv.style.backgroundColor = "rgba(136,136,136,0.8)";
    }
    this.addCateForm.reset();
    this.getParentCategoryCombobox();
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

    this.categoryService.getCategory(id).subscribe(res => {
      this.editCateForm = this.fb.group({
        CateId: [res.cateId, Validators.required],
        CateName: [res.cateName, Validators.required],
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
