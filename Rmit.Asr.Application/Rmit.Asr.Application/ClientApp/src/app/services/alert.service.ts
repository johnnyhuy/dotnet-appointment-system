import {Injectable} from '@angular/core';

@Injectable()
export class AlertService {
  private message: string
  private alerted: boolean = false

  get alert() {
    if (this.alerted) {
      this.alerted = false

      console.log("Get alert: " + this.message)
      return this.message
    }
    else {
      this.message = null
      return this.message
    }
  }

  addAlert(message) {
    this.message = message
    this.alerted = true
  }
}
