import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { DoctorAdmin } from '../_models/doctorsadmin';
import { DoctorService } from '../_services/doctor.service';

@Injectable()
export class DoctorJWTInterceptor implements HttpInterceptor {

  constructor(private doctorService:DoctorService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentDoctor: DoctorAdmin;
    this.doctorService.currentDoctor$.pipe(take(1)).subscribe(doctor=>currentDoctor=doctor);
    if (currentDoctor) {
      request=request.clone({
        setHeaders:{
          Authorization:'Bearer ${currentDoctor.token}'
        }
      })
    }
    return next.handle(request);
  }
}
