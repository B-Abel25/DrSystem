import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginComponent } from '../client/login/login.component';
import { LostPasswordRequestComponent } from '../client/lost-newPassword/lost-password-request/lost-password-request.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: LostPasswordRequestComponent): boolean {
    if (component.editForm.dirty) {
      return confirm('Folytatja a műveletet?');
    }
    return true;
  }
}
