import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'condominios',
    templateUrl: './condominios.component.html'
})
export class CondominiosComponent {
    public condominios: Condominio[];
    public usuarios: Usuario[];
    public administradoras: Administradora[];

    private _http: Http;
    private _baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;

        http.get(baseUrl + 'api/Configuracao/Condominios').subscribe(result => {
            this.condominios = result.json() as Condominio[];
        }, error => { console.error(error); alert(error._body) });

        http.get(baseUrl + 'api/Configuracao/Usuarios').subscribe(result => {
            this.usuarios = result.json() as Usuario[];
        }, error => { console.error(error); alert(error._body) });

        http.get(baseUrl + 'api/Configuracao/Administradoras').subscribe(result => {
            this.administradoras = result.json() as Administradora[];
        }, error => { console.error(error); alert(error._body) });
    }

    nome = '';
    administradora = 0;
    usuario = 0;

    public salvarCondominio() {
        var novoCondominio: Condominio = {
            id: 0,
            nome: this.nome,
            idAdministradora: this.administradora,
            idResponsavel: this.usuario
        };

        this._http
            .post(this._baseUrl + 'api/Configuracao/Condominio', novoCondominio)
            .subscribe(result => {
                novoCondominio.id = result.json() as number;
                this.condominios.push(novoCondominio);
                this.nome = '';
                this.administradora = 0;
                this.usuario = 0;
            }, error => { console.error(error); alert(error._body) });

    }
}

interface Condominio {
    id: number;
    nome: string;
    idAdministradora: number,
    idResponsavel: number
}

interface Administradora {
    id: number;
    nome: string;
}

interface Usuario {
    id: number;
    nome: string;
}