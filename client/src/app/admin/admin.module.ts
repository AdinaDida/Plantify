import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';
import { BsDropdownMenuDirective } from 'ngx-bootstrap/dropdown';
import { SharedModule } from '../shared/shared.module';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminProductsComponent } from './admin-products/admin-products.component';
import { AdminProductFormComponent } from './admin-product-form/admin-product-form.component';
import { AdminHistoryComponent } from './admin-history/admin-history.component';
import { AdminOrderDetailsComponent } from './admin-order-details/admin-order-details.component';



@NgModule({
  declarations: [AdminComponent, AdminOrdersComponent, AdminProductsComponent, AdminProductFormComponent, AdminHistoryComponent, AdminOrderDetailsComponent],
  imports: [
    CommonModule, AdminRoutingModule, SharedModule
  ]
})
export class AdminModule { }
