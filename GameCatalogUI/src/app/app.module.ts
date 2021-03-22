import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AboutComponent} from './about/about.component';
import { CatalogListComponent } from './catalog-list/catalog-list.component';
import { CatalogServices } from './shared/catalog-services.service';
import { CatalogUpdateComponent } from './catalog-update/catalog-update.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AboutComponent,
    CatalogListComponent,
    CatalogUpdateComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'show-catalog', component: CatalogListComponent },
      { path: 'update-catalog/:id', component: CatalogUpdateComponent},
      { path: 'about', component: AboutComponent, pathMatch: 'full' },
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: '**', component: HomeComponent, pathMatch: 'full' }
    ])
  ],
  providers: [CatalogServices],
  bootstrap: [AppComponent]
})
export class AppModule { }
