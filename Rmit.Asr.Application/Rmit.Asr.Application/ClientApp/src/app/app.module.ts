import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {RoomsIndexComponent} from './rooms/index.component';
import {RoomsEditComponent} from './rooms/edit.component';

import {RoomService} from "./services/room.service";
import {AlertService} from "./services/alert.service";
import {AlertsComponent} from './alerts/alerts/alerts.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RoomsIndexComponent,
    RoomsEditComponent,
    AlertsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'admin', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin/counter', component: CounterComponent },
      { path: 'admin/fetch-data', component: FetchDataComponent },
      { path: 'admin/rooms', component: RoomsIndexComponent },
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
