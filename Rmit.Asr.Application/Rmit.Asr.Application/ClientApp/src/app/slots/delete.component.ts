import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";

import {SlotService} from '../services/slot.service';
import {AlertService} from "../services/alert.service";

import {Slot} from '../models/slot';
import * as moment from 'moment';

@Component(
{
    selector: 'app-slots-delete',
    templateUrl: './delete.component.html',
})
export class SlotsDeleteComponent implements OnInit
{
  public slot : Slot
  public deleteSlotForm: FormGroup;
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
    this.slotService.getSlot(this.roomName, this.startDate, this.startTime).subscribe(slot => {
      this.slot = slot
    })

    this.deleteSlotForm = this.fb.group({
      roomName: {value: this.roomName, disabled: true},
      startDate: {value: this.startDate, disabled: true},
      startTime: {value: this.startTime, disabled: true}
    })
  }

  delete() {
    this.slotService.deleteSlot(this.deleteSlotForm.value.roomName, this.deleteSlotForm.value.startDate, this.deleteSlotForm.value.startTime).subscribe(() => {
      this.alertService.addAlert("Successfully deleted the slot!")
      this.router.navigateByUrl("/admin/slots")
    }, (errorResult: HttpErrorResponse) => {
      this.error = Object.keys(errorResult.error).reduce(function (r, k) {
        return r.concat(errorResult.error[k]);
      }, [])
    })
  }
}
