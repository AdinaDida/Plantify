import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem, IBasketTotals } from '../models/basket';
import { BasketService } from './basket.service';
import Modal from 'sweetalert2';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;
  basketTotals$: Observable<IBasketTotals>;
  
  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItemFromBasket(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItemQuantity(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  removeBasketItemConfirm(item: IBasketItem){
    Modal.fire({
      title: 'Are you sure you want to remove this item?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#4bbf73',
      cancelButtonColor: '#343a40',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.removeBasketItem(item);
        Modal.fire({
          position: 'center',
          icon: 'success',
          title: 'Removed!',
          showConfirmButton: false,
          timer: 700
        });
      } 
    })
  }
  
}
