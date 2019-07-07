import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { CondominiosComponent } from './components/condominios/condominios.component';
import { ComunicadosComponent } from './components/comunicados/comunicados.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CondominiosComponent, 
        ComunicadosComponent,
        UsuariosComponent,
        HomeComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'condominios', component: CondominiosComponent },
            { path: 'comunicados', component: ComunicadosComponent },
            { path: 'usuarios', component: UsuariosComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
