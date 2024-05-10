import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserAPIService } from '../UserAPIService';
import { ActivatedRoute, Router } from '@angular/router';
import * as feather from 'feather-icons';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  public UserDetail!: any;

  constructor(
    private toastr: ToastrService,
    private fb: FormBuilder, 
    private userService: UserAPIService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    feather.replace();
    this.getDetail();
  }

  getDetail(): void {
    this.activatedRoute.paramMap.subscribe(query => {
      console.log(query.get("userId"));   // lay id
      let id = query.get("userId");
      if (id != null) {
        let userId = parseInt(id);
        this.userService.getUser(userId).subscribe(res => {
          this.UserDetail = res;
        });
      }
      else if (id == null) {
        this.toastr.warning("Receipt not found!", "Warning")
      }
    })
  }
}
