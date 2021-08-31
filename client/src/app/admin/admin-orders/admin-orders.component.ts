import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/models/order';
import { AdminService } from '../admin.service';
import Modal from 'sweetalert2';

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

  confirmChangeStatus(id, status){
    Modal.fire({
      title: 'Are you sure you want to change this status?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#4bbf73',
      cancelButtonColor: '#343a40',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.changeStatus(id, status);
        Modal.fire({
          position: 'center',
          icon: 'success',
          title: 'Status changed!',
          showConfirmButton: false,
          timer: 800
        });
      } 
    })
  }
}
