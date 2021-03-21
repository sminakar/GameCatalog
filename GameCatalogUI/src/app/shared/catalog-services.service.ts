import { Injectable } from '@angular/core';
import { ICatalog } from './catalog.model';
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
    return this._http.get<ICatalog>(environment.apiBaseURL + "Catalogs")
  }
}

  
