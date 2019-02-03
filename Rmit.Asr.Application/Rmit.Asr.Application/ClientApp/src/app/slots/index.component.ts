import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {FormBuilder, FormGroup} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import * as moment from 'moment';

import {ValidationService} from "../services/validation.service";
import {SlotService} from '../services/slot.service';

import {Slot} from '../models/slot';
import {AlertService} from "../services/alert.service";

@Component({
  selector: 'app-slots-index',
  templateUrl: './index.component.html',
})
export class SlotsIndexComponent implements OnInit {
  public slots: Slot[];
  public error: string[];
  public getSlotForm: FormGroup;

  constructor(
    private router: Router,
    private slotService: SlotService,
    private alertService: AlertService,
    private fb: FormBuilder
  ) {
  }

  get userId() { return this.getSlotForm.get('userId'); }

  resetSlots() {
    this.getSlotForm.value.userId = ''
    this.getAll()
  }

  getAll() {
    this.slotService.getSlots().subscribe(slots => {
      this.slots = slots
      }, (errorResult: HttpErrorResponse) => {
      this.error = errorResult.error
    });
  }

  getSlot() {
    if (!this.getSlotForm.valid) {
      return
    }

    this.slotService.getUsersSlots(this.getSlotForm.value.userId).subscribe(slots => {
      this.slots = slots
    }, (errorResult: HttpErrorResponse) => {
      this.error = Object.keys(errorResult.error).reduce(function (r, k) {
        return r.concat(errorResult.error[k]);
      }, []);
    })
  }

  getDate(date: string) {
    return moment(date).format('MMMM DD YYYY')
  }

  date(format: string, date: string) {
    return moment(date).format(format)
  }

  ngOnInit(): void {
    this.getAll()

    this.getSlotForm = this.fb.group({
        userId: ['', ValidationService.invalidUserIdValidator]
      }
    )
  }
}
