import {Injectable} from '@angular/core';
import {FormControl} from "@angular/forms";

@Injectable()
export class ValidationService {
  constructor() { }

  static invalidUserIdValidator(control: FormControl) {
    const invalidUserId = (!control.value.startsWith('s') && !control.value.startsWith('e'))
    return invalidUserId ? {'invalidUserId': {value: true}} : null
  }
}
