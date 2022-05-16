import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from 'src/app/_services/message.service';
import { OfficeHoursService } from 'src/app/_services/office-hours.service';

@Component({
  selector: 'app-send-emails',
  templateUrl: './send-emails.component.html',
  styleUrls: ['./send-emails.component.css'],
})
export class SendEmailsComponent implements OnInit {
  sendEmailForm: FormGroup;
  constructor(
    private messageService: MessageService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.sendEmailInitializationForm();
  }

  sendEmailInitializationForm() {
    this.sendEmailForm = this.fb.group({
      subject: new FormControl(''),
      content: new FormControl(''),
    });
  }

  sendMailSubmit() {
    this.messageService
      .sendEmailDoctor(
        this.sendEmailForm.value['subject'],
        this.sendEmailForm.value['content']
      )
      .subscribe((response) => {
        this.sendEmailForm.reset();
        this.toastr.success('Sikeres emailküldés!');
        
       
      });
  }
}
