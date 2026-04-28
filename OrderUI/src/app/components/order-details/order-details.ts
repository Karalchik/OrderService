import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderService } from '../../services/order';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './order-details.html',
  styleUrls: ['./order-details.scss']
})
export class OrderDetails implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly orderService = inject(OrderService);

  order = signal<Order | null>(null);
  isLoading = signal(true);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.fetchOrderDetails(id);
    }
  }

  fetchOrderDetails(id: string): void {
    this.orderService.getOrderById(id).subscribe({
      next: (data) => {
        this.order.set(data);
        this.isLoading.set(false);
        console.log(`[OrderUI] Details loaded for ${id}. Sources: Redis/Mongo.`);
      },
      error: (err) => {
        console.error('[OrderUI] DETAIL_ERROR:', err);
        this.isLoading.set(false);
      }
    });
  }
}