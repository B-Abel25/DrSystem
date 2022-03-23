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
    this.loadDoctors();
    console.log('komment1');
    this.loadPostCodes();
    console.log('komment3');
    this.loadProfileDatas();
    this.initializeForm();
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
      doctor: ['', Validators.required],

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
      doctorSealNumber: ['', [Validators.required]],
      email: [
        '',
        [
          Validators.required,
          Validators.email,
          Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'),
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
      acceptTerms: [false, Validators.requiredTrue],
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(16),
        ]),
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });

    this.profileModifyForm.controls['postCode'].valueChanges.subscribe((x) => {
      x = x + '';
      if (x.length == 4) {
        this.profileModifyForm.controls['city'].setValue(
          this.places.find((y) => y.postCode == x).city
        );
      } else {
        this.profileModifyForm.controls['city'].setValue('');
      }
    });

    this.profileModifyForm.controls['city'].valueChanges.subscribe((x) => {
      let exist = this.places.find(
        (y) =>
          y.city == x &&
          y.postCode == this.profileModifyForm.controls['postCode'].value
      );

      if (exist != null) {
        this.profileModifyForm.controls['placeId'].setValue(exist.id);
      } else {
        this.profileModifyForm.controls['placeId'].setValue('');
      }
    });

    this.profileModifyForm.controls['doctor'].valueChanges.subscribe((x) => {
      let exist = this.doctors.find(
        (y) => y.name + ' ' + '-' + ' ' + y.place.postCode == x
      );

      if (exist != null)
        this.profileModifyForm.controls['doctorSealNumber'].setValue(
          exist.sealNumber
        );
      else this.profileModifyForm.controls['doctorSealNumber'].setValue('');
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl | any) => {
      return control?.value == control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  register() {
    this.accountService.register(this.profileModifyForm.value).subscribe(
      (response) => {
        this.router.navigateByUrl('/login');
        this.showMsg = true;
        this.toastr.success(
          'Sikeres regisztráció! Kérjük erősítse meg email címét!'
        );
      },
      (error) => {
        this.validationErrors = error;
        console.log(error);
      }
    );
  }

  loadDoctors() {
    this.accountService.getDoctors().subscribe((doctors) => {
      this.doctors = doctors.sort((one, two) => (one.name < two.name ? -1 : 1));
    });
  }
  loadPostCodes() {
    this.accountService.getPlaces().subscribe((postCodes) => {
      this.places = postCodes;
      console.log('komment2');
      this.loadPlaces();
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
    });
  }
}
