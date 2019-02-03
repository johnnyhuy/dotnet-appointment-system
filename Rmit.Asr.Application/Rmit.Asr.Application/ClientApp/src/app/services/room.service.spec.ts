import {TestBed} from '@angular/core/testing';
import {HttpClientTestingModule, HttpTestingController} from "@angular/common/http/testing";

import {RoomService} from './room.service';

describe('RoomService', () => {
  let httpTestingController: HttpTestingController
  let service: RoomService

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        RoomService,
        {
          provide: 'BASE_URL', useFactory: () => {
            return 'http://localhost/'
          }, deps: []
        }
      ],
      imports: [
        HttpClientTestingModule
      ]
    });

    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(RoomService);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('expects service to get rooms', () => {
    service.getRooms().subscribe(rooms => {
      expect(rooms.data.length).toBe(1)
    });

    const req = httpTestingController.expectOne('http://localhost/api/room');
    expect(req.request.method).toEqual('GET');

    const mockData = [
      {
        'id': 'Test',
        'name': 'Room Name'
      }
    ]
    req.flush({data: mockData});
  });
});
