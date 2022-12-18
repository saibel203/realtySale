import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { PropertyCardComponent } from './property/property-card/property-card.component';
import { PropertyListComponent } from './property/property-list/property-list.component';
import { HousingService } from './services/housing.service';
import { AddPropertyComponent } from './property/add-property/add-property.component';
import { PropertyDetailComponent } from './property/property-detail/property-detail.component';
import { UserLoginComponent } from './user/user-login/user-login.component';
import { UserRegisterComponent } from './user/user-register/user-register.component';
import { AlertifyService } from './services/alertify.service';
import { AuthService } from './services/auth.service';
import { PropertyDetailResolverService } from './services/property-detail-resolver.service';
import { FilterPipe } from './pipes/filter.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { HttpErrorInterceptorService } from './services/httperror-interceptor.service';
import { DatePipe } from '@angular/common';
import { UserChangePasswordComponent } from './user/user-change-password/user-change-password.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { AllPropertiesComponent } from './property/all-properties/all-properties.component';
import { FavouritesPropertiesComponent } from './property/favourites-properties/favourites-properties.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { SearchSettingsComponent } from './components/search-settings/search-settings.component';
import { FooterComponent } from './footer/footer.component';
import { PhotoEditorComponent } from './property/photo-editor/photo-editor.component';

const appRoutes: Routes = [
  { path: '', component: PropertyListComponent },
  { path: 'rent-property', component: PropertyListComponent },
  { path: 'add-property', component: AddPropertyComponent },
  {
    path: 'property-detail/:id', component: PropertyDetailComponent,
    resolve: { prp: PropertyDetailResolverService }
  },
  { path: 'user/login', component: UserLoginComponent },
  { path: 'user/register', component: UserRegisterComponent },
  { path: 'user/change-password', component: UserChangePasswordComponent },
  { path: 'user/profile', component: UserProfileComponent },
  { path: 'user/favourites', component: FavouritesPropertiesComponent },
  { path: 'user/all-properties/:username', component: AllPropertiesComponent },
  { path: '**', component: PropertyListComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    PropertyCardComponent,
    PropertyListComponent,
    AddPropertyComponent,
    PropertyDetailComponent,
    UserLoginComponent,
    UserRegisterComponent,
    FilterPipe,
    SortPipe,
    UserChangePasswordComponent,
    UserProfileComponent,
    AllPropertiesComponent,
    FavouritesPropertiesComponent,
    PaginationComponent,
    SearchSettingsComponent,
    FooterComponent,
    PhotoEditorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    NgxPaginationModule,
    RouterModule.forRoot(appRoutes),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ButtonsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    CarouselModule.forRoot(),
    ModalModule.forRoot(),
  ],
  providers: [
    HousingService,
    AuthService,
    AlertifyService,
    PropertyDetailResolverService,
    DatePipe,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
