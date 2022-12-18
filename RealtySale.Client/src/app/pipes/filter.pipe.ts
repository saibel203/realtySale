import { Pipe, PipeTransform } from '@angular/core';
import { IPropertyBase } from '../models/IPropertyBase.interface';

@Pipe({
  name: 'filter'
})

export class FilterPipe implements PipeTransform {
  transform(value?: Array<IPropertyBase>, filterString?: Array<any>,
  propertyName?: Array<string>): Array<any> {
    let resultArray: Array<IPropertyBase> = [];



    if (value && propertyName && filterString) {
      if (value?.length === 0 || filterString?.length === 0 || propertyName?.length === 0)
        return value;

      if (propertyName.includes('city')) {
        for (const item of value) {
          let lowerProperty = item['city']?.toLowerCase();
          let lowerFilter = filterString[0].toLowerCase();

          if (lowerProperty?.includes(lowerFilter))
            resultArray.push(item);
        }
      }

      if (propertyName.includes('bhk') && filterString[1] !== 0) {
        resultArray = resultArray.filter(element => element['bhk'] === filterString[1]);
      }

      if (propertyName.includes('price')) {
        if (filterString[3] === 0) filterString[3] = Infinity;
        resultArray = resultArray.filter(element => element['price']! >= filterString[2] &&
                      element['price']! <= filterString[3]);
      }

      if (propertyName.includes('builtArea')) {
        if (filterString[5] === 0) filterString[5] = Infinity;
        resultArray = resultArray.filter(element => element['builtArea']! >= filterString[4] &&
                      element['builtArea']! <= filterString[5]);
      }
    }

    return resultArray.reverse();
  }
}
