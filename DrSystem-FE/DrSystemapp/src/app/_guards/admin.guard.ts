import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { DoctorService } from '../_services/doctor.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private doctorService:DoctorService, private toastr: ToastrService){}
  canActivate(): Observable<boolean>{
    return this.doctorService.currentDoctor$.pipe(
      map(doctor=>{
        if(doctor) return true;
      
        this.toastr.error('You shall not pass');
        return false;
      })
    )
  }
  
}
