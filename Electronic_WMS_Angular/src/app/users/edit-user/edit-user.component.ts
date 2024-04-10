import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserAPIService } from '../UserAPIService';
import { RolesAPIService } from 'src/app/roles/RolesAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent {
  public submited: boolean = false;
  public FullName!: string;
  public UserName!: string;
  public PassWord!: string;
  public Address!: string;
  public Email!: string;
  public Phone!: string;
  public RoleId!: number;
  public FileImage!: File;
  public UserId!: number;
  // Combobox
  public ListRoleCombobox: any[] = [];

  editUser = this.fb.group({
    UserId: ['', Validators.required],
    FullName: ['', Validators.required],
    UserName: ['', Validators.required],
    PassWord: ['', Validators.required],
    Address: ['', Validators.required],
    Email: ['', Validators.required],
    Phone: ['', Validators.required],
    RoleId: ['', Validators.required]
  });

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private userService: UserAPIService,
    private roleService: RolesAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  get f() {return this.editUser.controls;}

  ngOnInit(): void {
    feather.replace();
    this.getUserDetail();
    this.getListRole();
  }

  getUserDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("userId"));   // lay id
      let id = query.get("userId");
      if (id != null) {
        let userId = parseInt(id);
        this.userService.getUser(userId).subscribe(res => {
          this.editUser = this.fb.group({
              UserId: [res.userId, Validators.required],
              FullName: [res.fullName, Validators.required],
              UserName: [res.userName, Validators.required],
              PassWord: [res.password, Validators.required],
              Address: [res.address, Validators.required],
              Email: [res.email, Validators.required],
              Phone: [res.phone, Validators.required],
              RoleId: [res.roleId, Validators.required]
          });
        });
      }
      else if (id == null) {
        this.toastr.warning("User not found!", "Warning")
      }
    })
  }

  onChange(event: any) {
    this.FileImage = event.target.files[0];
  }

  onSubmit(): void {
    this.submited = true;
    
    console.log(this.editUser.invalid)
    if(!this.editUser.invalid){
      console.log(this.editUser.value);
      this.UserId = parseInt(this.editUser.value.UserId!);
      this.FullName = this.editUser.value.FullName!.trim();
      this.Address = this.editUser.value.Address!.trim();
      this.PassWord = this.editUser.value.PassWord!.trim();
      this.UserName = this.editUser.value.UserName!.trim();
      this.Email = this.editUser.value.Email!.trim();
      this.Phone = this.editUser.value.Phone!.trim();
      this.RoleId = parseInt(this.editUser.value.RoleId!);
      
      var formAdd = new FormData();
      formAdd.append("UserId", this.UserId.toString());
      formAdd.append("FullName", this.FullName);
      formAdd.append("Address", this.Address);
      formAdd.append("PassWord", this.PassWord);
      formAdd.append("UserName", this.UserName);
      formAdd.append("Email", this.Email);
      formAdd.append("Phone", this.Phone);
      formAdd.append("RoleId", this.RoleId.toString());
      formAdd.append("FileImage", this.FileImage);

      console.log(formAdd)
      //thêm mới
      this.userService.updateUser(formAdd).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/user/index']).then(() => {
            this.toastr.success(res.statusMessage, "Success");
          });
        }
        else if(res.statusCode == 404){
          this.toastr.warning(res.statusMessage, "Warning");
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

  getListRole(): void {
    this.roleService.getRoleCombobox().subscribe(res => {
      this.ListRoleCombobox = res;
    })
  }
}
