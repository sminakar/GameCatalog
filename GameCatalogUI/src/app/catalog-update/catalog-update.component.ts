import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CatalogServices } from 'src/app/shared/catalog-services.service';

@Component({
  selector: 'app-catalog-update',
  // templateUrl: './catalog-update.component.html',
  template: '<h1>HI   From Update</h1>',
  styles: [
  ]
})
export class CatalogUpdateComponent implements OnInit {

  constructor(private _catalogService: CatalogServices,
    private _activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    let catalogID: string = this._activatedRoute.snapshot.params['id'];
    console.log(catalogID);
  }

}
