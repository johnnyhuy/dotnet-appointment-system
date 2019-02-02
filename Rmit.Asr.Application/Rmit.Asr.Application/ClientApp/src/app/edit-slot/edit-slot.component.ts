import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { SlotService } from '../services/slot.service';

import { Slot } from '../slot';


@Component(
{
    selector: 'app-edit-slots',
    templateUrl: './edit-slot.component.html',
})

export class EditSlotComponent implements OnInit
{
    _slotService:SlotService;
    _http:HttpClient;
    _router: Router;

    public slot : Slot;

    public _userID:string;
    public _startDate:string;
    public _startTime:string;

    constructor(private http: HttpClient, private router: Router, slotService: SlotService)
    {
        this._slotService=slotService;
        this._http=http;
        this._router=router;
    }

    ngOnInit() {
    }

    onSubmit()
    {
        this._slotService.editSlot(this._userID, this._startDate, this._startTime);
    }

}
