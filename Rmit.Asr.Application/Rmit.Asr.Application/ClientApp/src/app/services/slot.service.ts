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

  static editSlot(roomID:string, startDate:string, startTime:string)
  {
    if ( roomID == null || startDate == null || startTime==null )
      return console.log("incorrect ID, date or time entered...");
  }
}
