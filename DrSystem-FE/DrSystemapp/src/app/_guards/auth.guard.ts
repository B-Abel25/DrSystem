import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService, private router:Router) { }
  canActivate(): Observable<boolean> {
    return this.accountService.currentClient$.pipe(
      map(client => {
        if (client) return true;
        this.router.navigateByUrl("/login")
        this.toastr.error('Sikertelen belépés!');
        return false;
      })
    )
  }

}
