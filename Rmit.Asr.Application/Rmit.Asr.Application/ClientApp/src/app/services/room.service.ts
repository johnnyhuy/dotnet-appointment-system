import {Inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {Room} from "../models/room";
import {Observable} from "rxjs";

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

  getRooms(): Observable<Room[]>
  {
    return this.http.get<Room[]>(this.baseUrl + "api/room")
  }

  createRoom(room)
  {
    return this.http.post(this.baseUrl + "api/room", room)
  }

  updateRoom(roomName, room)
  {
    return this.http.put(`${this.baseUrl}api/room/${encodeURIComponent(roomName)}`, room)
  }
}
