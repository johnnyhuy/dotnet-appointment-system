import {async, ComponentFixture, TestBed} from '@angular/core/testing';

import {AlertsComponent} from './alerts.component';
import {AlertService} from "../services/alert.service";

describe('AlertsComponent', () => {
  let component: AlertsComponent;
  let fixture: ComponentFixture<AlertsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AlertsComponent
      ],
      providers: [
        AlertService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlertsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get message', () => {
    // Arrange
    let testMessage = 'this is a test message'
    let alertService = new AlertService()
    alertService.addAlert(testMessage)
    let component = new AlertsComponent(alertService)

    // Assert
    expect(component.message).toBe(testMessage)
  })
});
