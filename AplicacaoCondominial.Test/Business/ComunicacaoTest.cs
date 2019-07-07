using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AplicacaoCondominial.Test.Business
{
    public class ComunicacaoTest
    {
        Config config;

        public ComunicacaoTest()
        {
            config = new Config();
        }

        private void Salvar<T>(T objeto) where T : class
        {
            config.Context.Add<T>(objeto);
            config.Context.SaveChanges();
        }

        [Theory]
        [InlineData(Core.Model.TipoUsuario.Administradora)]
        [InlineData(Core.Model.TipoUsuario.Sindico)]
        [InlineData(Core.Model.TipoUsuario.Zelador)]
        public async void MensagemEnviadaCorretamente(Core.Model.TipoUsuario tipoUsuario)
        {
            var administradora = GerarAdministradora();
            var condominio = GerarCondominio(administradora.Id);
            var usuarioDestino = GerarUsuario(condominio.Id, tipoUsuario);
            var usuarioMorador = GerarUsuario(condominio.Id, Core.Model.TipoUsuario.Morador);

            if (tipoUsuario != Core.Model.TipoUsuario.Administradora)
            {
                condominio.IdResponsavel = usuarioDestino.Id;
                config.Context.Condominios.Update(condominio);
                config.Context.SaveChanges();
            }

            var mensagem = $"Mensagem usuário morador {usuarioMorador.Id} para usuario {usuarioDestino.Id}.";

            var response = await config.ComunicacaoBusiness
                .EnviarComunicadoAsync(new Core.Business.Wraps.BaseRequest<Core.Business.DTOs.Comunicado>(
                    new Core.Business.DTOs.Comunicado
                    {
                        Assunto = (tipoUsuario == Core.Model.TipoUsuario.Administradora
                            ? Core.Model.Assunto.Administrativo.ToString()
                            : Core.Model.Assunto.Condominial.ToString()),
                        IdUsuario = usuarioMorador.Id,
                        Mensagem = mensagem
                    }));

            Assert.True(response.Success, response.Message);

            //Validar mensagem
        }

        private Core.Model.Usuario GerarUsuario(int idCondominio, Core.Model.TipoUsuario tipoUsuario)
        {
            var usuario = new Core.Model.Usuario
            {
                IdCondominio = idCondominio,
                Tipo = tipoUsuario,
                Nome = $"Usuario {tipoUsuario.ToString()} {DateTime.Now.Ticks}"
            };

            config.Context.Usuarios.Add(usuario);
            config.Context.SaveChanges();

            return usuario;
        }

        private Core.Model.Condominio GerarCondominio(int idAdministradora)
        {
            var condominio = new Core.Model.Condominio
            {
                IdAdministradora = idAdministradora,
                Nome = $"Condomínio Teste {DateTime.Now.Ticks}"
            };

            config.Context.Condominios.Add(condominio);
            config.Context.SaveChanges();

            Assert.Single(config.Context.Condominios
                    .Where(c => c.Id == condominio.Id)
                    .ToList());

            return condominio;
        }

        private Core.Model.Administradora GerarAdministradora()
        {
            var administradora = new Core.Model.Administradora
            {
                Nome = "Administradora Comunicacao"
            };

            config.Context.Administradoras.Add(administradora);
            config.Context.SaveChanges();

            Assert.Single(config.Context.Administradoras
                    .Where(a => a.Id == administradora.Id && a.Id > 0)
                    .ToList());

            return administradora;
        }
    }
}
