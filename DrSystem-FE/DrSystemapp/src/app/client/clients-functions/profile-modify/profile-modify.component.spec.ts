import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileModifyComponent } from './profile-modify.component';

describe('ProfileModifyComponent', () => {
  let component: ProfileModifyComponent;
  let fixture: ComponentFixture<ProfileModifyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileModifyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileModifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
