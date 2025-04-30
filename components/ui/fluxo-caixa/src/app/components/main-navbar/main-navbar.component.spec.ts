import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MainNavbarComponent } from './main-navbar.component';
import { provideRouter } from '@angular/router';
import { provideOAuthClient } from 'angular-oauth2-oidc';
import { provideHttpClient } from '@angular/common/http';

describe('MainNavbarComponent', () => {
  let component: MainNavbarComponent;
  let fixture: ComponentFixture<MainNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideOAuthClient()
      ],
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
