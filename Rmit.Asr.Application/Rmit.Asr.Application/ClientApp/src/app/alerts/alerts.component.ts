import {Component} from '@angular/core';

import {AlertService} from "../services/alert.service";

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html'
})
export class AlertsComponent {
  public message: string;

  constructor(
    private alertService: AlertService
  ) {
    this.message = alertService.alert
  }
}
