import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common'; 
import { OrderService } from '../../services/order';
import { Router, RouterLink } from '@angular/router';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './order-list.html',
  styleUrls: ['./order-list.scss']
})
export class OrderList implements OnInit {
  private readonly orderService = inject(OrderService);

  orders = signal<Order[]>([]);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.isLoading.set(true);
    this.orderService.getOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.isLoading.set(false);
        console.log('[OrderUI] Orders loaded successfully via Signals.');
      },
      error: (err) => {
        this.errorMessage.set('Failed to load orders. Please check your connection.');
        this.isLoading.set(false);
        console.error('[OrderUI] FETCH_ERROR:', err);
      }
    });
  }

  onCancel(id: string | undefined): void {
    if (!id) return;

    this.orderService.cancelOrder(id).subscribe({
      next: (updatedOrder) => {
        this.orders.update(prev => 
          prev.map(o => o.id === id ? updatedOrder : o)
        );
        console.log(`[OrderUI] Order ${id} cancelled.`);
      },
      error: (err) => alert('Action failed: ' + (err.error?.message || 'Unknown error'))
    });
  }
}