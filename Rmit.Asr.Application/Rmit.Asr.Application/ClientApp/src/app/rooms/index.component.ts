import {Component, Inject, OnInit} from '@angular/core'

import {RoomService} from "../services/room.service"

import {Room} from "../models/room";

@Component({
  selector: 'app-rooms-index',
  templateUrl: './index.component.html'
})
export class RoomsIndexComponent implements OnInit {
  public rooms: Room[]

  constructor(
    @Inject('BASE_URL') baseUrl: string,
    private roomService: RoomService
  ) {
    roomService.getRooms().subscribe(rooms => {
      this.rooms = rooms
    }, error => console.error(error))
  }

  ngOnInit() {
  }
}
