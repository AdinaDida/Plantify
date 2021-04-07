import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/models/order';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

  orders:IOrder[]
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.adminService.getOrders().subscribe(orders => {
      this.orders = orders;
    }, error => {
      console.log(error);
    });
  }

  changeStatus(id, status)
  {
    this.adminService.changeOrderStatus(id, status).toPromise().then(x => this.getOrders());
  }
}
