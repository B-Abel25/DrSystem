import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Doctor } from 'src/app/_models/doctor';
import { Place } from 'src/app/_models/place';
import { Registration } from 'src/app/_models/registration';
import { AccountService } from 'src/app/_services/account.service';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-profile-modify',
  templateUrl: './profile-modify.component.html',
  styleUrls: ['./profile-modify.component.css'],
})
export class ProfileModifyComponent implements OnInit {
  profileDatas: Registration;
  doctors: Doctor[];
  submitted: boolean = false;
  profileModifyForm: FormGroup;
  validationErrors: string[];
  places: Place[];
  placesString: string[];
  public showPasswordOnPress: boolean;
  showMsg: boolean = false;
  fieldTextType: boolean;
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private doctorService: DoctorService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.loadPostCodes();
  }

  onSubmit() {
    this.submitted = true;
    if (this.profileModifyForm.valid) {
      alert(
        'Form Submitted succesfully!!!\n Check the values in browser console.'
      );
    }
  }

  initializeForm() {
    this.profileModifyForm = this.fb.group({
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern('[0-9]*'),
          Validators.maxLength(11),
          Validators.minLength(9),
        ],
      ],

      medNumber: [
        '',
        [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')],
      ],
      doctorSealNumber: ['not-modified', [Validators.required]],

      houseNumber: [
        '',
        [
          Validators.required,
          Validators.pattern('[0-9 a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ -/.]*'),
        ],
      ],
      birthDate: ['', [Validators.required]],
      street: [
        '',
        [
          Validators.required,
          Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ -/]*'),
        ],
      ],
      city: ['', Validators.required],
      postCode: ['', Validators.required],

      email: [
        '',
        [
          Validators.required,
          Validators.pattern('[a-z 0-9 A-Z .-]+@[a-z .-]+.[a-z]*'),
          Validators.email,
        ],
      ],
      name: [
        '',
        [
          Validators.required,
          Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ -]*'),
        ],
      ],
      motherName: [
        '',
        [
          Validators.required,
          Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ -]*'),
        ],
      ],
      birthPlace: [
        '',
        [
          Validators.required,
          Validators.pattern('[a-z A-Z áéűúőóüöíÁÉŰÚŐÓÜÖÍ -]*'),
        ],
      ],

      password: [
        '',
        Validators.compose([Validators.minLength(8), Validators.maxLength(16)]),
      ],
      confirmPassword: ['', [this.matchValues('password')]],
    });

    this.profileModifyForm.controls['postCode'].valueChanges.subscribe((x) => {
      x = x + '';

      let placeHelper = this.places.find((y) => y.postCode == x);

      if (x.length == 4 && placeHelper != undefined) {
        this.profileModifyForm.controls['city'].setValue(placeHelper.city);
      } else {
        this.profileModifyForm.controls['city'].setValue('');
      }
    });

    this.profileModifyForm.controls['password'].valueChanges.subscribe(
      (passwordValue) => {
        
        if (passwordValue !== '') {
          this.profileModifyForm
            .get('password')
            .addValidators(Validators.required);
          this.profileModifyForm
            .get('confirmPassword')
            .addValidators(Validators.required);
        } else if (passwordValue === '') {
          this.profileModifyForm
            .get('password')
            .removeValidators(Validators.required);
          this.profileModifyForm
            .get('confirmPassword')
            .removeValidators(Validators.required);
          console.log('removed');
        }
        // console.log('confirmPass');
        // console.log(
        //   this.profileModifyForm.controls['confirmPassword'].hasValidator(
        //     Validators.required
        //   )
        // );
        // console.log('Pass');
        // console.log(
        //   this.profileModifyForm.controls['confirmPassword'].hasValidator(
        //     Validators.required
        //   )
        // );
      }
    );

    console.log(this.profileModifyForm.controls);
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl | any) => {
      return control?.value == control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  profileModify() {
    console.log(this.profileModifyForm.controls['password'].value);
    if (this.profileModifyForm.controls['password'].value == '') {
      this.profileModifyForm.controls['password'].setValue('not-modified');
    }

    this.accountService
      .profileModifyPut(this.profileModifyForm.value)
      .subscribe(
        (response) => {
          this.toastr.success('Sikeresen módosította az adatait!');
        },
        (error) => {
          this.validationErrors = error;
          console.log(error);
        }
      );
    this.profileModifyForm.controls['password'].setValue('');
    this.profileModifyForm.controls['confirmPassword'].setValue('');
  }

  loadPostCodes() {
    this.accountService.getPlaces().subscribe((postCodes) => {
      this.places = postCodes;
      console.log('komment2');
      this.loadPlaces();
      this.loadProfileDatas();
    });
  }
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  loadPlaces() {
    this.placesString = [...new Set(this.places.map((x) => x.city))];
  }
  loadProfileDatas() {
    this.accountService.getProfileDatas().subscribe((profile) => {
      this.profileDatas = profile;
      // TODO:dupla replace helyett 1

      this.profileDatas.birthDate = this.profileDatas.birthDate
        .replace('.', '-')
        .replace('.', '-');
      this.profileDatas.phoneNumber = this.profileDatas.phoneNumber.replace(
        '+36',
        ''
      );
      this.setControlValues();
    });
  }

  setControlValues() {
    this.profileModifyForm.controls['name'].setValue(this.profileDatas.name);
    this.profileModifyForm.controls['medNumber'].setValue(
      this.profileDatas.medNumber
    );
    this.profileModifyForm.controls['phoneNumber'].setValue(
      this.profileDatas.phoneNumber
    );
    this.profileModifyForm.controls['street'].setValue(
      this.profileDatas.street
    );
    this.profileModifyForm.controls['birthDate'].setValue(
      this.profileDatas.birthDate
    );
    this.profileModifyForm.controls['city'].setValue(
      this.profileDatas.place.city
    );
    this.profileModifyForm.controls['postCode'].setValue(
      this.profileDatas.place.postCode
    );
    this.profileModifyForm.controls['birthPlace'].setValue(
      this.profileDatas.birthPlace
    );
    this.profileModifyForm.controls['motherName'].setValue(
      this.profileDatas.motherName
    );
    this.profileModifyForm.controls['houseNumber'].setValue(
      this.profileDatas.houseNumber
    );
    this.profileModifyForm.controls['email'].setValue(this.profileDatas.email);
  }
}
