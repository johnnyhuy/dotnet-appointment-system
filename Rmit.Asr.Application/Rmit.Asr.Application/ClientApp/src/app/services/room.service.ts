import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {Room} from "../models/room";

@Injectable()
export class RoomService {
  private readonly baseUrl: string;

  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  )
  {
    this.baseUrl = baseUrl;
  }

  getRooms()
  {
    return this.http.get<Room[]>(this.baseUrl + "api/room")
  }

  deleteRoom(roomName)
  {
    return this.http.delete(this.baseUrl + "api/room/:name", roomName)
  }

  updateRoom(roomName, room)
  {
    return this.http.put(`${this.baseUrl}api/room/${encodeURIComponent(roomName)}`, room)
  }
}