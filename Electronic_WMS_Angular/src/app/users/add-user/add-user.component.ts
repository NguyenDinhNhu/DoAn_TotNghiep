import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';
import { UserAPIService } from '../UserAPIService';
import { ToastrService } from 'ngx-toastr';
import { RolesAPIService } from 'src/app/roles/RolesAPIService';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent {

  public submited: boolean = false;
  public FullName!: string;
  public UserName!: string;
  public PassWord!: string;
  public Address!: string;
  public Email!: string;
  public Phone!: string;
  public RoleId!: number;
  public FileImage!: File;

  // Combobox
  public ListRoleCombobox: any[] = [];

  addUser = this.fb.group({
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

  ngOnInit(): void {
    feather.replace();
    this.getListRole();
  }

  get f() {return this.addUser.controls;}
  
  onChange(event: any) {
    this.FileImage = event.target.files[0];
  }

  onSubmit(): void {
    this.submited = true;
    
    console.log(this.addUser.invalid)
    if(!this.addUser.invalid){
      console.log(this.addUser.value);
      this.FullName= this.addUser.value.FullName!.trim();
      this.Address = this.addUser.value.Address!.trim();
      this.PassWord = this.addUser.value.PassWord!.trim();
      this.UserName = this.addUser.value.UserName!.trim();
      this.Email = this.addUser.value.Email!.trim();
      this.Phone = this.addUser.value.Phone!.trim();
      this.RoleId = parseInt(this.addUser.value.RoleId!);
      
      var formAdd = new FormData();
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
      this.userService.insertUser(formAdd).subscribe(res => {
        if(res.statusCode == 200){
          this.submited = false;
          this.router.navigate(['/user/index']).then(() => {
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

  getListRole(): void {
    this.roleService.getRoleCombobox().subscribe(res => {
      this.ListRoleCombobox = res;
    })
  }
}
