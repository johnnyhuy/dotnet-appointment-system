import { Injectable, Inject } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";

import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";
import "rxjs/add/observable/throw";


@Injectable()
export class SlotService
{
  constructor(private http: Http) {
      this.http = http
  }

  getSlots()
  {
      return this.http.get("http://localhost:5000/Api/Slot/").map((response: Response) => response.json()).catch(this.errorHandler);
  }

  // gets booked slots based on the id given
  getUsersSlots(usersID:string)
  {
        if ( usersID == null || (!usersID.startsWith('s')&&!usersID.startsWith('e')) )
            return console.log("incorrect ID entered...");


        if ( usersID.startsWith('s') )
            return this.http.get("http://localhost:5000/Api/Slot/Student/" + usersID).map((response:Response)=> response.json()).catch(this.errorHandler);

        if ( usersID.startsWith('e') )
            return  this.http.get("http://localhost:5000/Api/Slot/Staff/" + usersID).map((response:Response)=> response.json()).catch(this.errorHandler);
  }


  editSlot(roomID:string,startDate:string,startTime:string)
  {
    if ( roomID == null || startDate == null || startTime==null )
            return console.log("incorrect ID, date or time entered...");

    console.log(roomID + " " + startDate + " " + startTime)

    // return  this.http.put("http://localhost:5000/Api/Slot/" + roomID + "/" + startDate + "/" + startTime, slot).map((response:Response)=> response.json()).catch(this.errorHandler);

  }

  errorHandler(error: Response)
  {
      console.log(error)
      console.log("cant get slots")
      return Observable.throw(error)
  }
}
