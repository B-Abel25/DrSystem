import {
  Component,
  EventEmitter,
  HostListener,
  OnInit,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-lost-password-doctor',
  templateUrl: './lost-password-doctor.component.html',
  styleUrls: ['./lost-password-doctor.component.css'],
})
export class LostPasswordDoctorComponent implements OnInit {
  @ViewChild('requestForm')
  requestForm!: NgForm;
  lostPasswordForm: FormGroup;
  @Output() lostPasswordOpenedEvent = new EventEmitter();
  paswwordLost: boolean = false;
  public model: any = {};
  editForm: any;

  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.requestForm.dirty) {
      $event.returnValue = true;
    }
  }
  modalRefDoctor: BsModalRef;
  constructor(
    private modalService: BsModalService,
    private doctorService: DoctorService,
    private toastr: ToastrService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  sendResetMail() {
    this.doctorService.lostPassword(this.lostPasswordForm.value).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        console.log(error);
      }
    );
  }

  ngOnInit() {
    this.initializationForm();
  }

  openModal(template: TemplateRef<any>) {
    this.lostPasswordOpenedEvent.emit();

    this.modalRefDoctor = this.modalService.show(template);
  }

  initializationForm() {
    this.lostPasswordForm = this.fb.group({
      userNumber: ['', [Validators.required, Validators.pattern('[0-9]{5}')]],
    });
  }
  public Close() {
    console.log('Becsuk');
    this.modalRefDoctor.hide();
  }
}
