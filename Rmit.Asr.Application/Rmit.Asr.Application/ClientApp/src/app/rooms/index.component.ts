import {Component, Inject, OnInit} from '@angular/core'
import {HttpClient} from '@angular/common/http'

import {RoomService} from "../services/room.service"

import {Room} from "../models/room";

@Component({
  selector: 'app-rooms-index',
  templateUrl: './index.component.html'
})
export class RoomsIndexComponent implements OnInit {
  public rooms: Room[]

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private roomService: RoomService
  ) {
    roomService.getRooms().subscribe(result => {
      this.rooms = result
    }, error => console.error(error))
  }

  ngOnInit() {
  }
}
