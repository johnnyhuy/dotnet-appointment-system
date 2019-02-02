import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from "@angular/http";
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RoomsComponent } from './rooms/rooms.component';

import { SlotsComponent } from './slots/slots.component';
import { EditSlotComponent } from './edit-slot/edit-slot.component';
import { ListSlotComponent } from './list-slot/list-slot.component';

import { SlotService } from './services/slot.service';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RoomsComponent,
    SlotsComponent,
    EditSlotComponent,
    ListSlotComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'admin', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin/counter', component: CounterComponent },
      { path: 'admin/fetch-data', component: FetchDataComponent },
      { path: 'admin/rooms', component: RoomsComponent },
      { path: 'admin/slots', component: SlotsComponent },
      { path: 'admin/edit-slot', component: EditSlotComponent },
      { path: 'admin/list-slot', component: ListSlotComponent }
    ])
  ],
  providers: [SlotService],
  bootstrap: [AppComponent]
})
export class AppModule { }
