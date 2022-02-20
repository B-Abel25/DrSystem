import { TestBed } from '@angular/core/testing';

import { DoctorJWTInterceptor } from './doctor-jwt.interceptor';

describe('DoctorJWTInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      DoctorJWTInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: DoctorJWTInterceptor = TestBed.inject(DoctorJWTInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
