export interface IPropertyBase {
  id: number | null;
  sellRent: number;
  name: string;
  propertyType: string;
  furnishingType: string;
  price: number | null;
  bhk: number | null;
  builtArea: number | null;
  city: string;
  readyToMove: boolean | null;
  image?: string;
  estPossessionOn?: string;
  postedOn?: string;
}
