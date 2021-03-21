import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CatalogServices } from 'src/app/shared/catalog-services.service';

@Component({
  selector: 'app-catalog-update',
  templateUrl: './catalog-update.component.html',
  // template: '<h1>HI   From Update</h1>',
  styles: [
  ]
})
export class CatalogUpdateComponent implements OnInit {

  public resultCatalogData: any;

  constructor(private _catalogService: CatalogServices,
    private _activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    let catalogId: string = this._activatedRoute.snapshot.params['id'];
    
    this._catalogService.getCatalogById(catalogId)
    .subscribe(
      result => 
      {
        if (result.isFailure)
        {
          console.error("Error in fatching catalog data from server");
          return;
        }
        this.resultCatalogData = result.data;
        this.resultCatalogData.releaseDate = this.resultCatalogData.releaseDate.split('T')[0];
        
      }, error => console.error(error)
    )
  }
}
