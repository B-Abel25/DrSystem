import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientHomepageComponent } from './client-homepage.component';

describe('ClientHomepageComponent', () => {
  let component: ClientHomepageComponent;
  let fixture: ComponentFixture<ClientHomepageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientHomepageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientHomepageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
