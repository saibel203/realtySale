import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value?: any[], filterString?: string, propertyName?: string): any[] {
    const resultArray = [];

    if (value) {
      if (value?.length === 0 || filterString === '' || propertyName === '')
        return value;

      for (const item of value) {
        if (propertyName && item[propertyName].toLocaleLowerCase() === filterString?.toLocaleLowerCase())
          resultArray.push(item);
      }
    }
    return resultArray;
  }

}