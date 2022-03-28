import { Inject, Injectable, InjectionToken } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, empty, Observable, timeout, TimeoutError } from 'rxjs';

export const DEFAULT_TIMEOUT = new InjectionToken<number>('defaultTimeout');
@Injectable({
  providedIn: 'root'
})
export class TimeoutInterceptor implements HttpInterceptor {

  constructor( @Inject(DEFAULT_TIMEOUT) protected defaultTimeout: number,) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const modified = req.clone({
      setHeaders: { 'X-Request-Timeout': `${this.defaultTimeout}` }
    });

    return next.handle(modified).pipe(
      timeout(this.defaultTimeout),
      catchError(err => {
        if (err instanceof TimeoutError)
          console.error('Timeout has occurred', req.url);
        return empty();
      })
    );
  }
}
