import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";

import {SlotService} from '../services/slot.service';
import {AlertService} from "../services/alert.service";

import {Slot} from '../models/slot';
import * as moment from 'moment';

@Component(
{
    selector: 'app-slots-edit',
    templateUrl: './edit.component.html',
})
export class SlotsEditComponent implements OnInit
{
  public slot : Slot
  public editSlotForm: FormGroup
  public error: string[]

  get roomName() {
    return this.route.snapshot.paramMap.get('id')
  }

  get startTime() {
    return this.route.snapshot.paramMap.get('start_time')
  }

  get startDate() {
    return this.route.snapshot.paramMap.get('start_date')
  }

  get startDateTime() {
    return moment(this.startDate + ' ' + this.startTime).format('YYYY-MM-DD h:mm a')
  }

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private fb: FormBuilder,
    private slotService: SlotService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.editSlotForm = this.fb.group({
        studentId: ['', Validators.required]
      }
    );
  }

  edit() {
    this.slotService.editSlot(this.roomName, this.startDate, this.startTime, this.editSlotForm.value.studentId).subscribe(() => {
      this.alertService.addAlert("Successfully updated the slot!")
      this.router.navigateByUrl("/admin/slots")
    }, (errorResult: HttpErrorResponse) => {
      console.log(errorResult.error)
      this.error = Object.keys(errorResult.error).reduce(function (r, k) {
        return r.concat(errorResult.error[k]);
      }, []);
    });
  }
}
