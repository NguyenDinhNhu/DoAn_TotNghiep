import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() currentPage: number = 1;
  @Input() totalItem: number = 0;
  @Input() pageSize: number = 10;
  @Output() changePage = new EventEmitter<number>();

  totalPages: number = 0;

  ngOnChanges(): void {
    this.calculateTotalPages();
  }

  calculateTotalPages(): void {
    this.totalPages = Math.ceil(this.totalItem / this.pageSize);
  }

  getPages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.changePage.emit(this.currentPage + 1);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.changePage.emit(this.currentPage - 1);
    }
  }
}