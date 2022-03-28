import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  TemplateRef,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Client } from 'src/app/_models/client';
import { Doctor } from 'src/app/_models/doctor';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-referral',
  templateUrl: './referral.component.html',
  styleUrls: ['./referral.component.css'],
})
export class ReferralComponent implements OnInit {
  @Output() ReferralOpenedEvent = new EventEmitter();
  modalRef!: BsModalRef;
  referralForm: FormGroup;
  doctors: Doctor[];
  client: Client;
  constructor(
    private modalService: BsModalService,
    private fb: FormBuilder,
    private doctorService: DoctorService
  ) {}

  ngOnInit() {
    this.initializationForm();
  }
  openModal(template: TemplateRef<any>) {
    this.ReferralOpenedEvent.emit();
    this.modalRef = this.modalService.show(template);
    this.loadDoctors();
  }
  initializationForm() {
    this.referralForm = this.fb.group({
      userNumber: [
        '',
        [Validators.required, Validators.pattern('[0-9]{3}[0-9]{3}[0-9]{3}')],
      ],
      Doctor: ['', [Validators.required]],

      doctorSealNumber: ['', [Validators.required]],
    });
    this.referralForm.controls['Doctor'].valueChanges.subscribe((x) => {
      let exist = this.doctors.find(
        (y) => y.name + ' ' + '-' + ' ' + y.place.postCode == x
      );

      if (exist != null)
        this.referralForm.controls['doctorSealNumber'].setValue(
          exist.sealNumber
        );
      else this.referralForm.controls['doctorSealNumber'].setValue('');
    });
  }
  public Close() {
    this.modalRef.hide();
    this.referralForm.reset();
  }
  closeModal() {
    if (this.modalRef != null) {
      this.modalRef.hide();
      this.referralForm.reset();
    }
  }

  loadDoctors() {
    this.doctorService.getDoctors().subscribe((doctors) => {
      this.doctors = doctors.sort((one, two) => (one.name < two.name ? -1 : 1));
    });
  }
}
