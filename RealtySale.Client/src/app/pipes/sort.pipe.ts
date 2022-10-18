import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort'
})
export class SortPipe implements PipeTransform {

  transform(value: Array<string>, args: any[]): any {
    const sortField = args[0];
    const sortDirection = args[1];

    let multiplier = (sortDirection === 'desc') ? 1 : -1;

    if (value) {
      value.sort((a: any, b: any) => {
        let aField, bField;
        if (isNaN(a[sortField])){
          aField = a[sortField]?.toLowerCase();
        } else{
          aField = +a[sortField]
        }

        if (isNaN(b[sortField])){
          bField = b[sortField]?.toLowerCase();
        } else{
          bField = +b[sortField]
        }

        if (a[sortField] < b[sortField]) {
          return -1 * multiplier;
        } else if (a[sortField] > b[sortField]) {
          return 1 * multiplier;
        } else {
          return 0;
        }
      });
      return value;
    }
  }

}
