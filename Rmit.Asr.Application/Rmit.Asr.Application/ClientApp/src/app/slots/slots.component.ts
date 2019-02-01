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


    constructor(private http: HttpClient, private _router: Router, slotService: SlotService)
    {
        slotService.getSlots().subscribe(data => this.slotList = data);
    }

    ngOnInit()
    {

    }
}


interface Slot
{
    Room: string;
    Staff: string;
    Student: string;
    StartTime: string;
}