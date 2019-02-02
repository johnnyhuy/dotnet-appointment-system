import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SlotService } from '../services/slot.service';

import { Slot } from '../slot';


@Component(
{
  selector: 'app-slots',
  templateUrl: './slots.component.html',
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