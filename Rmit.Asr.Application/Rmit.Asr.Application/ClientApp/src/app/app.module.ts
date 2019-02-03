import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FlatpickrModule} from 'angularx-flatpickr';

import {AppComponent} from './app.component';
import {AlertsComponent} from './alerts/alerts.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {RoomsIndexComponent} from './rooms/index.component';
import {RoomsEditComponent} from './rooms/edit.component';
import {RoomsCreateComponent} from "./rooms/create.component";

import {RoomService} from "./services/room.service";
import {AlertService} from "./services/alert.service";
import {SlotService} from './services/slot.service';
import {ValidationService} from "./services/validation.service";

import {SlotsComponent} from './slots/slots.component';
import {SlotsEditComponent} from './slots/edit.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SlotsComponent,
    SlotsEditComponent,
    RoomsIndexComponent,
    RoomsCreateComponent,
    RoomsEditComponent,
    AlertsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    FlatpickrModule.forRoot(),
    RouterModule.forRoot([
      { path: 'admin', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin/slots', component: SlotsComponent },
      { path: 'admin/slots/edit', component: SlotsEditComponent },
      { path: 'admin/rooms', component: RoomsIndexComponent },
      { path: 'admin/rooms/create', component: RoomsCreateComponent },
      { path: 'admin/rooms/edit/:id', component: RoomsEditComponent }
    ])
  ],
  providers: [
    AlertService,
    RoomService,
    SlotService,
    ValidationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
