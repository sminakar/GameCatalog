import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CatalogServices } from 'src/app/shared/catalog-services.service';
import { IGenre } from 'src/app/shared/IGenre.model';
import { IDeveloper } from 'src/app/shared/IDeveloper.model';
import { ICatalog } from '../shared/Icatalog.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-catalog-update',
  templateUrl: './catalog-update.component.html',
  styles: [
  ]
})

export class CatalogUpdateComponent implements OnInit {

  public resultCatalogData: any;
  public genraData : IGenre[];
  public devloperData: IDeveloper[];
  
  constructor(private _catalogService: CatalogServices,
    private _activatedRoute: ActivatedRoute,
    private _router: Router) { }

  ngOnInit(): void {
    let catalogId: string = this._activatedRoute.snapshot.params['id'];

    //  ================  Retrive Catalog Data to update ==================
    this._catalogService.getCatalogById(catalogId)
    .subscribe(result => 
      {
        if (result.isFailure)
        {
          console.error("Error in fatching catalog data from server");
          return;
        }
        this.resultCatalogData = result.data;
        this.resultCatalogData.releaseDate = this.resultCatalogData.releaseDate.split('T')[0];

        this._catalogService.getGenres()
          .subscribe(gResult => {
            if (gResult.isFailure)
            {
              console.error("Error in fatching Genre data from server");
              return;
            }
            
            this.genraData = gResult.data;
            this.genraData.forEach(e => e.selected = e.id == this.resultCatalogData.genreID ? true: false);

            }, error => console.error(error)
          );

          this._catalogService.getDevelopers()
          .subscribe(dResult => {
            if (dResult.isFailure)
            {
              console.error("Error in fatching Genre data from server");
              return;
            }
            
            this.devloperData = dResult.data;
            this.devloperData.forEach(e => e.selected= e.id == this.resultCatalogData.companyID ? true: false);

          }, error => console.error(error)
        )

      }, error => console.error(error)
    )
  }

  onSubmit(formData: ICatalog): void {
    this._catalogService.updateCatalog(formData)
      .subscribe(result => {
        this._router.navigate(['show-catalog'])
      });
  }
}
