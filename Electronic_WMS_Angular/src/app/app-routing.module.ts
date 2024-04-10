import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';
import { CategoryComponent } from './category/category.component';
import { FeatureComponent } from './feature/feature.component';
import { RolesComponent } from './roles/roles.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { ListUserComponent } from './users/list-user/list-user.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';

const routes: Routes = [
  { path: '', component: DashboardComponent},
  { path: 'brand/index', component: ListBrandComponent},
  { path: 'category/index', component: CategoryComponent},
  { path: 'feature/index', component: FeatureComponent},
  { path: 'roles/index', component: RolesComponent},
  { path: 'warehouse/index', component: WarehouseComponent},
  { path: 'user/index', component: ListUserComponent},
  { path: 'user/add', component: AddUserComponent},
  { path: 'user/edit/:userId', component: EditUserComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
