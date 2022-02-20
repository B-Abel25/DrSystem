import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, NavigationStart, Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-new-password-doctor',
  templateUrl: './new-password-doctor.component.html',
  styleUrls: ['./new-password-doctor.component.css'],
})
export class NewPasswordDoctorComponent implements OnInit {
  newPasswordFormDoctor: FormGroup;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toatsr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initializationForm();
    /*TODO megnézni esetleg van e szebb megoldás*/
  }
  initializationForm() {
    this.newPasswordFormDoctor = this.fb.group({
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(16),
        ]),
      ],
      newconfirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
      emailToken: [this.router.url.split('/')[3]],
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl | any) => {
      return control?.value == control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }
  newPassword() {
    console.log(this.newPasswordFormDoctor.value);
    this.accountService.newPassword(this.newPasswordFormDoctor.value).subscribe(
      (response) => {
        this.router.navigateByUrl('/admin/login');
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
