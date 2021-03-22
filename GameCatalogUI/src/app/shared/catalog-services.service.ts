import { Injectable } from '@angular/core';
import { ICatalog } from './Icatalog.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CatalogServices 
{
  public catalogData : ICatalog;
  constructor(private _http: HttpClient) { }

  getCatalogs() : Observable<ICatalog> {
    return this._http.get<ICatalog>(environment.apiBaseURL + "Catalogs");
  }

  getCatalogById(catalogId: string) : Observable<ICatalog> {
    return this._http.get<ICatalog>(environment.apiBaseURL + "Catalog/" + catalogId);
  }

  getGenres() : Observable<any> {
    return this._http.get<any>(environment.apiBaseURL + "Genres");
  }

  getDevelopers() : Observable<any> {
    return this._http.get<any>(environment.apiBaseURL + "Companies");
  }

}

  
