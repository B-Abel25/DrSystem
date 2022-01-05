import { Component, OnInit } from '@angular/core';
import { UserService } from './../../shared/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }
  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('Sikeres regisztráció!', '' );
        } else {
          res.errors.forEach((element: { code: any; description: string | undefined; }) => {
            switch (element.code) {
              case 'DuplicatTAJ':
                this.toastr.error('Nem megfelelő bemeneti adat!');
                break;

              default:
              this.toastr.error(element.description,'Registration failed.');
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }

}
