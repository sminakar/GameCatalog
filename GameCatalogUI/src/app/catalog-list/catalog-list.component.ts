import { Component, OnInit } from '@angular/core';
import { CatalogServices } from 'src/app/shared/catalog-services.service';

@Component({
  selector: 'app-catalog-list',
  templateUrl: './catalog-list.component.html',
  providers: [CatalogServices]
})
export class CatalogListComponent implements OnInit {

  public resultData: any;

  constructor(private _catalogService: CatalogServices) { }

  ngOnInit(): void 
  {
      this._catalogService.getCatalogs()
      .subscribe(
        result => 
        {
          if (result.isFailure)
          {
            console.error("Error in fatching catalog data from server");
            return;
          }
          this.resultData = result.data
        }, error => console.error(error)
      )
  }
}

