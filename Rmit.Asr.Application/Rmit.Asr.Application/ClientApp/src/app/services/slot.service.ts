import {Inject, Injectable} from "@angular/core";
import {HttpClient} from '@angular/common/http';
import {Observable} from "rxjs";

import {Slot} from "../models/slot";

@Injectable()
export class SlotService
{
  private readonly baseUrl: string

  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.baseUrl = baseUrl;
  }

  getSlots()
  {
    return this.http.get<Slot[]>(`${this.baseUrl}api/slot/`)
  }

  getUsersSlots(userId:string): Observable<Slot[]>
  {
    if (userId.startsWith('s'))
      return this.http.get<Slot[]>(`${this.baseUrl}api/slot/student/${encodeURIComponent(userId)}`)

    if (userId.startsWith('e'))
      return this.http.get<Slot[]>(`${this.baseUrl}api/slot/staff/${encodeURIComponent(userId)}`)
  }

  editSlot(roomName:string, startDate:string, startTime:string, studentID: string)
  {
    return this.http.put<Slot[]>(`${this.baseUrl}api/slot/${encodeURIComponent(roomName)}/${encodeURIComponent(startDate)}/${encodeURIComponent(startTime)}`, { 'studentId': studentID })
  }

  deleteSlot(roomName:string, startDate:string, startTime:string)
  {
    return this.http.delete<Slot[]>(`${this.baseUrl}api/slot/${encodeURIComponent(roomName)}/${encodeURIComponent(startDate)}/${encodeURIComponent(startTime)}`)
  }
}
