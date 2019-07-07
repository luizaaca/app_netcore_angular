import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'usuarios',
    templateUrl: './usuarios.component.html'
})
export class UsuariosComponent {
    public condominios: Condominio[];
    public usuarios: Usuario[];

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
    }

    nome = '';
    condominio = 0;
    tipo = 0;

    tiposUsuario: TipoUsuario[] = [
        { id: 1, nome: 'Morador' },
        { id: 2, nome: 'Sindico' },
        { id: 3, nome: 'Administradora' },
        { id: 2, nome: 'Zelador' }
    ];

    public salvarUsuario() {
        var novoUsuario: Usuario = {
            id: 0,
            nome: this.nome,
            idCondominio: this.condominio,
            tipo: {
                id: this.tipo,
                nome: this.tiposUsuario[this.tipo-1].nome
            }
        };

        this._http
            .post(this._baseUrl + 'api/Configuracao/Usuario', novoUsuario)
            .subscribe(result => {
                novoUsuario.id = result.json() as number;
                this.usuarios.push(novoUsuario);
                this.nome = '';
                this.condominio = 0;
                this.tipo = 0;
            }, error => { console.error(error); alert(error._body) });

    }
}

interface Condominio {
    id: number;
    nome: string;
    idAdministradora: number,
    idResponsavel: number
}

interface Usuario {
    id: number;
    nome: string;
    idCondominio: number;
    tipo: TipoUsuario;
}

interface TipoUsuario {
    id: number;
    nome: string;
}