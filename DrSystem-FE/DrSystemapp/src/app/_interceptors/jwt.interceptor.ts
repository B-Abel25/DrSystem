import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { Registration } from '../_models/registration';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: Registration;
    this.accountService.currentClient$.pipe(take(1)).subscribe(client=>currentUser=client);
    if (currentUser) {
      request=request.clone({
        setHeaders:{
          Authorization:'Bearer ${currentUser.token}'
        }
      })
    }
    return next.handle(request);
  }
}
