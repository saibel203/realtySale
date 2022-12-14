import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { City } from 'src/app/models/City';
import { IKeyValuePair } from 'src/app/models/IKeyValuePair.interface';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';
import { Property } from 'src/app/models/Property';
import { UserForProfile } from 'src/app/models/UserForProfile.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-add-property',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.scss']
})
export class AddPropertyComponent implements OnInit {

  @ViewChild('formTabs') formTabs?: TabsetComponent;

  addPropertyForm?: FormGroup;
  addCityForm?: FormGroup;
  nextClicked?: boolean;
  modalRef?: BsModalRef;

  whereCanMove: Array<string> = ['East', 'West', 'South', 'North'];
  property = new Property();
  city = new City();

  propertyTypes?: IKeyValuePair[];
  furnishTypes?: IKeyValuePair[];
  cityList?: Array<any>;

  propertyView: IPropertyBase = {
    id: null,
    name: '',
    price: null,
    sellRent: 0,
    propertyType: '',
    furnishingType: '',
    bhk: null,
    builtArea: null,
    city: '',
    readyToMove: null,
  };

  constructor(private fb: FormBuilder, private alertify: AlertifyService,
    private housingService: HousingService, private router: Router,
    private datePipe: DatePipe, private modalService: BsModalService) { }

  ngOnInit() {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    this.initData();
    this.CreateAddPropertyForm();
    this.CreateAddCityForm();
  }

  CreateAddPropertyForm() {
    this.addPropertyForm = this.fb.group({
      BasicInfo: this.fb.group({
        SellRent: ['0', Validators.required],
        BHK: [null, Validators.required],
        PType: [null, Validators.required],
        FType: [null, Validators.required],
        Name: [null, Validators.required],
        City: [null, Validators.required]
      }),

      PriceInfo: this.fb.group({
        Price: [null, Validators.required],
        BuiltArea: [null, Validators.required],
        CarpetArea: [0],
        Security: [0],
        Maintenance: [0],
      }),

      AddressInfo: this.fb.group({
        FloorNo: [0],
        TotalFloor: [0],
        Address: [null, Validators.required],
        LandMark: ['']
      }),

      OtherInfo: this.fb.group({
        RTM: [null, Validators.required],
        PossessionOn: [null, Validators.required],
        AOP: [],
        Gated: [false],
        MainEntrance: [0],
        Description: [''],
      })
    });
  }

  CreateAddCityForm() {
    this.addCityForm = this.fb.group({
      CityName: [null, [Validators.required, Validators.minLength(5), Validators.pattern('^[a-zA-Z]+$')]],
      Country: [null, Validators.required]
    });
  }

  initData() {
    this.housingService.getAllCities().subscribe(
      data => this.cityList = data
    );

    this.housingService.getAllPropertyTypes().subscribe(
      data => this.propertyTypes = data
    );

    this.housingService.getAllFurnishingTypes().subscribe(
      data => this.furnishTypes = data
    );
  }

  onSubmit() {
    this.nextClicked = true;
    if (this.allTabsValid()) {
      this.mapProperty();
      this.housingService.addProperty(this.property).subscribe(() => {
        this.alertify.success('Your property listed successfully on website');

        if (this.SellRent?.value === '1') {
          this.router.navigate(['/rent-property']);
        } else {
          this.router.navigate(['/']);
        }
      });
    } else {
      this.alertify.error('Please review the form and provide all valid entries');
    }
  }

  onCitySubmit() {
    this.mapCity();

    this.housingService.addNewCity(this.city).subscribe(() => {
      this.alertify.success('City added successfully!');
      window.location.reload();
    });
  }

  allTabsValid(): boolean {
    if (this.BasicInfo?.invalid && this.formTabs) {
      this.formTabs.tabs[0].active = true;
      return false;
    }

    if (this.PriceInfo?.invalid && this.formTabs) {
      this.formTabs.tabs[1].active = true;
      return false;
    }

    if (this.AddressInfo?.invalid && this.formTabs) {
      this.formTabs.tabs[2].active = true;
      return false;
    }

    if (this.OtherInfo?.invalid && this.formTabs) {
      this.formTabs.tabs[3].active = true;
      return false;
    }

    return true;
  }

  selectTab(NextTabId: number, IsCurrentTabValid?: boolean) {
    this.nextClicked = true;
    if (this.formTabs?.tabs[NextTabId] && IsCurrentTabValid)
      this.formTabs.tabs[NextTabId].active = true;
  }

  mapProperty(): void {
    this.property.id = this.housingService.newPropertyId();
    this.property.sellRent = +this.SellRent.value;
    this.property.bhk = this.BHK.value;
    this.property.propertyTypeId = this.PType.value;
    this.property.name = this.Name.value;
    this.property.cityId = this.City.value;
    this.property.furnishingTypeId = this.FType.value;
    this.property.price = this.Price.value;
    this.property.security = this.Security.value;
    this.property.maintenance = this.Maintenance.value;
    this.property.builtArea = this.BuiltArea.value;
    this.property.carpetArea = this.CarpetArea.value;
    this.property.floorNo = this.FloorNo.value;
    this.property.totalFloors = this.TotalFloor.value;
    this.property.address = this.Address.value;
    this.property.address2 = this.LandMark.value;
    this.property.readyToMove = this.RTM.value;
    this.property.gated = this.Gated.value;
    this.property.mainEntrance = this.MainEntrance.value;
    this.property.estPossessionOn = this.datePipe.transform(this.PossessionOn.value, 'MM/dd/yyyy') ?? undefined;
    this.property.description = this.Description.value;
  }

  mapCity(): void {
    this.city.name = this.CityName.value;
    this.city.country = this.Country.value;
  }

  openModalAddCity(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  // #region <Getter Methods>
  // #region <FormGroups>
  get BasicInfo() {
    return this.addPropertyForm?.controls?.['BasicInfo'] as FormGroup;
  }

  get PriceInfo() {
    return this.addPropertyForm?.controls?.['PriceInfo'] as FormGroup;
  }

  get AddressInfo() {
    return this.addPropertyForm?.controls?.['AddressInfo'] as FormGroup;
  }

  get OtherInfo() {
    return this.addPropertyForm?.controls?.['OtherInfo'] as FormGroup;
  }
  // #endregion
  // #region <FormControls>
  get SellRent() {
    return this.BasicInfo?.controls?.['SellRent'] as FormControl;
  }

  get BHK() {
    return this.BasicInfo?.controls?.['BHK'] as FormControl;
  }

  get PType() {
    return this.BasicInfo?.controls?.['PType'] as FormControl;
  }

  get FType() {
    return this.BasicInfo?.controls?.['FType'] as FormControl;
  }

  get Name() {
    return this.BasicInfo?.controls?.['Name'] as FormControl;
  }

  get City() {
    return this.BasicInfo?.controls?.['City'] as FormControl;
  }

  get Price() {
    return this.PriceInfo?.controls?.['Price'] as FormControl;
  }

  get BuiltArea() {
    return this.PriceInfo?.controls?.['BuiltArea'] as FormControl;
  }

  get CarpetArea() {
    return this.PriceInfo?.controls?.['CarpetArea'] as FormControl;
  }

  get Security() {
    return this.PriceInfo?.controls?.['Security'] as FormControl;
  }

  get Maintenance() {
    return this.PriceInfo?.controls?.['Maintenance'] as FormControl;
  }

  get FloorNo() {
    return this.AddressInfo?.controls?.['FloorNo'] as FormControl;
  }

  get TotalFloor() {
    return this.AddressInfo?.controls?.['TotalFloor'] as FormControl;
  }

  get Address() {
    return this.AddressInfo?.controls?.['Address'] as FormControl;
  }

  get LandMark() {
    return this.AddressInfo?.controls?.['LandMark'] as FormControl;
  }

  get RTM() {
    return this.OtherInfo?.controls?.['RTM'] as FormControl;
  }

  get PossessionOn() {
    return this.OtherInfo?.controls?.['PossessionOn'] as FormControl;
  }

  get AOP() {
    return this.OtherInfo?.controls?.['AOP'] as FormControl;
  }

  get Gated() {
    return this.OtherInfo?.controls?.['Gated'] as FormControl;
  }

  get MainEntrance() {
    return this.OtherInfo?.controls?.['MainEntrance'] as FormControl;
  }

  get Description() {
    return this.OtherInfo?.controls?.['Description'] as FormControl;
  }

  get CityName() {
    return this.addCityForm?.controls?.['CityName'] as FormControl;
  }

  get Country() {
    return this.addCityForm?.controls?.['Country'] as FormControl;
  }
  // #endregion
  // #endregion
}
