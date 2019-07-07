import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'comunicados',
    templateUrl: './comunicados.component.html'
})
export class ComunicadosComponent {
    public usuarios: Usuario[];
    public assuntos: Assunto[];

    private _http: Http;
    private _baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;

        http.get(baseUrl + 'api/Configuracao/Usuarios').subscribe(result => {
            this.usuarios = (result.json() as Usuario[]).filter(e => e.tipo.id === 1);
        }, error => { console.error(error); alert(error._body) });

        http.get(baseUrl + 'api/Configuracao/Assuntos').subscribe(result => {
            this.assuntos = (result.json() as Assunto[]);
        }, error => { console.error(error); alert(error._body) });
    }

    assunto = '';
    mensagem = '';
    usuario = 0;

    public enviarComunicado() {
        var novoComunicado: Comunicado = {
            idUsuario: this.usuario,
            assunto: this.assunto,
            mensagem: this.mensagem
        };

        this._http
            .post(this._baseUrl + 'api/Comunicacao/Comunicado', novoComunicado)
            .subscribe(result => {                
                this.assunto = '';
                this.mensagem = '';
                this.usuario = 0;
                alert('Mensagem enviada com sucesso!');
            }, error => { console.error(error); alert(error._body) });

    }
}

interface Comunicado {
    idUsuario: number;
    assunto: string;
    mensagem: string;
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

interface Assunto {
    descricao: string;
}