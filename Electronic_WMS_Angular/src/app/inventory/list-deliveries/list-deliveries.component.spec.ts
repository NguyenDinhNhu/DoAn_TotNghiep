import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListDeliveriesComponent } from './list-deliveries.component';

describe('ListDeliveriesComponent', () => {
  let component: ListDeliveriesComponent;
  let fixture: ComponentFixture<ListDeliveriesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ListDeliveriesComponent]
    });
    fixture = TestBed.createComponent(ListDeliveriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
