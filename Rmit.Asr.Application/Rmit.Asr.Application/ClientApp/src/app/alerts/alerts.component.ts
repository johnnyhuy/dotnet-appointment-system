import {Component, Input} from '@angular/core';

import {AlertService} from "../services/alert.service";

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html'
})
export class AlertsComponent {
  private _message: string

  @Input()
  inputMessage: string

  get message() {
    if (this.inputMessage) {
      return this.inputMessage
    } else {
      return this._message
    }
  }

  set message(value) {
    this._message = value
  }

  constructor(
    private alertService: AlertService
  ) {
    this.message = alertService.alert
  }
}
