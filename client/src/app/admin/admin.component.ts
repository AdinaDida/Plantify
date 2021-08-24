import { Component, OnInit } from '@angular/core';
import { IOrder } from '../models/order';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
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
