import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {FormBuilder, FormGroup} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import * as moment from 'moment';

import {ValidationService} from "../services/validation.service";
import {SlotService} from '../services/slot.service';

import {Slot} from '../models/slot';

@Component({
  selector: 'app-slots',
  templateUrl: './slots.component.html',
})
export class SlotsComponent implements OnInit {
  public slots: Slot[];
  public error: string[];
  public getSlotForm: FormGroup;

  constructor(
    private router: Router,
    private slotService: SlotService,
    private fb: FormBuilder,
  ) {
    this.getAll()
  }

  get userId() { return this.getSlotForm.get('userId'); }

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

  getDate(date: string) {
    return moment(date).format('MMMM Do YYYY')
  }

  date(format: string, date: string) {
    return moment(date).format(format)
  }

  ngOnInit(): void {
    this.getSlotForm = this.fb.group({
        userId: ['', ValidationService.invalidUserIdValidator]
      }
    );
  }
}
