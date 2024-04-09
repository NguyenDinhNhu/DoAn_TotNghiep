import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';
import { CategoryComponent } from './category/category.component';
import { FeatureComponent } from './feature/feature.component';
import { RolesComponent } from './roles/roles.component';

const routes: Routes = [
  { path: '', component: DashboardComponent},
  { path: 'brand/index', component: ListBrandComponent},
  { path: 'category/index', component: CategoryComponent},
  { path: 'feature/index', component: FeatureComponent},
  { path: 'roles/index', component: RolesComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
