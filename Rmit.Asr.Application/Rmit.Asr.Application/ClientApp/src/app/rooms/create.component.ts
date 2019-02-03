import {Component, Inject} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';

import {RoomService} from "../services/room.service";
import {AlertService} from "../services/alert.service";

import {Room} from "../models/room";

@Component({
  selector: 'app-rooms-create',
  templateUrl: './create.component.html'
})
export class RoomsCreateComponent {
  public rooms: Room[];
  public error: Room;

  createRoomForm = this.fb.group({
      name: ['', Validators.required]
    }
  );

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private fb: FormBuilder,
    private roomService: RoomService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.fb = fb;

    http.get<Room[]>(baseUrl + 'api/room').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }

  create() {
    this.roomService.createRoom(this.createRoomForm.value).subscribe(() => {
      this.alertService.addAlert("Successfully updated the room!")
      this.router.navigateByUrl("/admin/rooms")
    }, (errorResult: HttpErrorResponse) => {
      this.error = errorResult.error;
    });
  }
}
