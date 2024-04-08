import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';

const routes: Routes = [
  { path: '', component: DashboardComponent},
  { path: 'brand/index', component: ListBrandComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
