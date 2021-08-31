import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-admin-order-details',
  templateUrl: './admin-order-details.component.html',
  styleUrls: ['./admin-order-details.component.scss']
})
export class AdminOrderDetailsComponent implements OnInit {
  order: IOrder;

  constructor(private route: ActivatedRoute, private breadcrumbService: BreadcrumbService, private adminService: AdminService) {
    this.breadcrumbService.set('@OrderDetails', ' ');
  }

  ngOnInit(): void {
    this.getDetails();
  }

  getDetails(){
    this.adminService.getOrderDetails(+this.route.snapshot.paramMap.get('id'))
    .subscribe((order: IOrder) => {
      this.order = order;
      console.log(this.order);
      this.breadcrumbService.set('@OrderDetails', `Order #${order.id}`);
    }, error => {
      console.log(error);
    })
  }

}
