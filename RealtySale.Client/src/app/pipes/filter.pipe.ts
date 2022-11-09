import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(value?: any[], filterString?: string, propertyName?: string): any[] {
    const resultArray = [];

    if (value && propertyName) {
      if (value?.length === 0 || filterString === '' || propertyName === '')
        return value;

      for (const item of value) {
        let lowerProperty = item[propertyName]?.toLowerCase();
        let lowerFilter = filterString?.toLowerCase();

        if (lowerProperty?.includes(lowerFilter))
          resultArray.push(item);
      }
    }

    return resultArray;
  }

}
