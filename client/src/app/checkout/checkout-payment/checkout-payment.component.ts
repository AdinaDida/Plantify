import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/models/basket';
import { IOrder } from 'src/app/models/order';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup
  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  submitOrder()
  {
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) =>{
     this.toastrService.success("Order created successfully"); 
     this.basketService.deleteLocalBasket(basket.id)
     console.log(order)});
    
  }
  private getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod'),
      shipToAddress: this.checkoutForm.get('addressForm').value
    }
  }

}
