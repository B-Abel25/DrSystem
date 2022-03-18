import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { DoctorService } from '../_services/doctor.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private doctorService: DoctorService, private toastr: ToastrService, private router:Router) { }
  canActivate(): Observable<boolean> {
    return this.doctorService.currentDoctor$.pipe(
      map(doctor => {
        if (doctor) return true;
        this.router.navigateByUrl("/admin/login")
        this.toastr.error('Sikertelen belépés!');
        return false;
      
      })
    )
  }

}
