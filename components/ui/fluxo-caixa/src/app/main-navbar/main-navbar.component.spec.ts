import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainNavbarComponent } from './main-navbar.component';
import { provideRouter } from '@angular/router';

describe('MainNavbarComponent', () => {
  let component: MainNavbarComponent;
  let fixture: ComponentFixture<MainNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideRouter([])],
      imports: [MainNavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
