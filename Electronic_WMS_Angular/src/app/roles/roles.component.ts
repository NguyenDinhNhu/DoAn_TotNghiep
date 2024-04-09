import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RolesAPIService } from './RolesAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent {
  // Search - Get List 
  public pageSize: number = 10;
  public currentPage: number = 1;
  public textSearch!: string;
  public listRoles: any[] = [];
  public totalItem: number = 0;

  public roleName!: string;
  public roleId!: number;
  // Form add 
  public submitedAdd: boolean = false;

  addRoleForm = this.fb.group({
    RoleName: ['', Validators.required]
  });

  // Form edit
  public submitedEdit: boolean = false;

  editRoleForm = this.fb.group({
    RoleId: ['', Validators.required],
    RoleName: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private rolesService: RolesAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.addRoleForm.controls;}
  get v() {return this.editRoleForm.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getAllRoles();
  }

  // Add 
  addRole(): void {
    this.submitedAdd = true;
    
    if(!this.addRoleForm.invalid){
      console.log(this.addRoleForm.value);
      this.roleName = this.addRoleForm.value.RoleName!.trim();
      //call api add 
      this.rolesService.insertRole( {RoleName: this.roleName}).subscribe(res => {
        if(res.statusCode == 200){
          this.toastr.success(res.statusMessage, "Success");
          this.submitedAdd = false;
          this.getAllRoles();
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
  editRole(): void {
    this.submitedEdit = true;
    
    if(!this.editRoleForm.invalid){
      console.log(this.editRoleForm.value);
      this.roleId = parseInt(this.editRoleForm.value.RoleId!);
      this.roleName = this.editRoleForm.value.RoleName?.trim()!;
      //call api update 
      this.rolesService.updateRole( { 
        RoleId: this.roleId,
        RoleName: this.roleName }).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.submitedAdd = false;
            this.getAllRoles();
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
  deleteRole(id: number): void {
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
        this.rolesService.deleteRole(id).subscribe(res => {
          if(res.statusCode == 200){
            this.toastr.success(res.statusMessage, "Success");
            this.getAllRoles();
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

  getAllRoles(): any {
    this.rolesService.getListRole({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listRoles = res.listRole;
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
    this.rolesService.getListRole({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listRoles = res.listRole;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  onChangePageSize(event: any) {
    this.pageSize = event.target.value;
    this.rolesService.getListRole({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch}).subscribe(res => {
      this.listRoles = res.listRole;
      this.totalItem = res.total;
      setTimeout(() => {
        feather.replace();
      }, 10)
    })
  }

  search(): void {
    this.rolesService.getListRole({PageSize: this.pageSize, CurrentPage: this.currentPage, TextSearch: this.textSearch.trim()}).subscribe(res => {
      this.listRoles = res.listRole;
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
    this.addRoleForm.reset();
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

    this.rolesService.getRole(id).subscribe(res => {
      this.editRoleForm = this.fb.group({
        RoleId: [res.roleId, Validators.required],
        RoleName: [res.roleName, Validators.required]
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
