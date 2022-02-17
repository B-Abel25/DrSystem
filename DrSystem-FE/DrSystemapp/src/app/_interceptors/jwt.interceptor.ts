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
    let currentClient: Registration;
    this.accountService.currentClient$.pipe(take(1)).subscribe(client=>currentClient=client);
    if (currentClient) {
      request=request.clone({
        setHeaders:{
          Authorization:'Bearer ${currentClient.token}'
        }
      })
    }
    return next.handle(request);
  }
}
