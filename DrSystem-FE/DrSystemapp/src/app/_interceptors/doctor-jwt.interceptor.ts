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

  constructor(private doctorService: DoctorService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log("Kérés küldve");
    let currentDoctor: DoctorAdmin;
    this.doctorService.currentDoctor$.pipe(take(1)).subscribe(doctor => currentDoctor = doctor);
    if (currentDoctor) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentDoctor.token}`
        }
      })
    }
    console.log(request);
    return next.handle(request);
  }
}
