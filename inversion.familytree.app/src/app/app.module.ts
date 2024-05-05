import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { RootAscendantComponent } from './root-ascendant/root-ascendant.component';
import { FamilyTreeComponent } from './family-tree/family-tree.component';
import { FormsModule } from '@angular/forms';
import { TreeListComponent } from './tree-list/tree-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/family-tree', pathMatch: 'full' },
  { path: 'grand-daddy', component: RootAscendantComponent },
  { path: 'family-tree', component: FamilyTreeComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    RootAscendantComponent,
    FamilyTreeComponent,
    TreeListComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(routes),
    FormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
