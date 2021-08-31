import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/models/order';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-history',
  templateUrl: './admin-history.component.html',
  styleUrls: ['./admin-history.component.scss']
})
export class AdminHistoryComponent implements OnInit {

  orders:IOrder[]
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getHistory();
  }

  getHistory(){
    this.adminService.getFinishedOrders().subscribe(orders => {
      this.orders = orders;
    }, error => {
      console.log(error);
    });
  }

}
