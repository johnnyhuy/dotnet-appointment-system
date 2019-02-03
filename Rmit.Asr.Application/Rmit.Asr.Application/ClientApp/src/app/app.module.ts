import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {AppComponent} from './app.component';
import {AlertsComponent} from './alerts/alerts.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {RoomsIndexComponent} from './rooms/index.component';
import {RoomsEditComponent} from './rooms/edit.component';
import {RoomsCreateComponent} from "./rooms/create.component";

import {RoomService} from "./services/room.service";
import {AlertService} from "./services/alert.service";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
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
    RouterModule.forRoot([
      { path: 'admin', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin/rooms', component: RoomsIndexComponent },
      { path: 'admin/rooms/create', component: RoomsCreateComponent },
      { path: 'admin/rooms/edit/:id', component: RoomsEditComponent }
    ])
  ],
  providers: [
    RoomService,
    AlertService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
