import { IPhoto } from "./IPhoto.interface";
import { IPropertyBase } from "./IPropertyBase.interface";

export class Property implements IPropertyBase {
  id!: number;
  sellRent!: number;
  name!: string;
  propertyTypeId?: number;
  propertyType!: string;
  bhk!: number;
  furnishingTypeId?: number;
  furnishingType!: string;
  price!: number;
  builtArea!: number;
  carpetArea?: number;
  address!: string;
  address2?: string;
  cityId?: number;
  city!: string;
  floorNo?: string;
  totalFloors?: string;
  readyToMove!: boolean;
  age?: string;
  mainEntrance?: number;
  security?: number;
  gated?: boolean;
  maintenance?: number;
  image?: string;
  estPossessionOn?: string;
  description?: string;
  photos?: IPhoto[];
  postedOn?: string;
  postedBy!: number;
}
