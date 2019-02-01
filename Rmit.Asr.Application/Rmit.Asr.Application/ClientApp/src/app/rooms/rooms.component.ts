import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html'
})
export class RoomsComponent {
  public rooms: Room[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Room[]>(baseUrl + 'api/').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }
}

interface Room {
  roomId: string,
  name: string
}
