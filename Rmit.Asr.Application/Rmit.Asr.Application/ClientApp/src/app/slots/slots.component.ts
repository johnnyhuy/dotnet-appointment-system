import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SlotService } from '../services/slot.service';
import { forEach } from '@angular/router/src/utils/collection';
import { P } from '@angular/core/src/render3';

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

    _allSlots:Slot[];
    _filterList:Slot[];


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
        if( this.slotList != null  || typeof this.slotList != "undefined" )
            this.slotList.length=0;

        this._slotService.getUsersSlots(this._userID).subscribe(data=>this.slotList=data);
    }

    getAll()
    {
        if( this.slotList != null || typeof this.slotList != "undefined" )
            this.slotList.length=0;

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