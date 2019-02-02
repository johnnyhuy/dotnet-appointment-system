import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SlotService } from '../services/slot.service';

@Component(
{
  selector: 'app-slots',
  templateUrl: './slots.component.html'
})

export class SlotsComponent implements OnInit
{
    public slotList: Slot[];

    public _userID:string;
    _slotService:SlotService;
    _http:HttpClient;
    _router: Router;

    constructor(private http: HttpClient, private router: Router, slotService: SlotService)
    {
        //slotService.getSlots().subscribe(data => this.slotList = data);
        this._slotService=slotService;
        this._http=http;
        this._router=router;
    }

    ngOnInit()
    {

    }

    onSubmit()
    {
        console.log("In onSubmit");
        console.log("_userID : " + this._userID);
        this._slotService.getUsersSlots(this._userID).subscribe(data=>this.slotList=data);
    }

    getAll()
    {
        this._slotService.getSlots().subscribe(data => this.slotList = data);
    }

}


interface Slot
{
    Room: string;
    Staff: string;
    Student: string;
    StartTime: string;
}