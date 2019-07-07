using Core.Business.DTOs;
using Core.Business.Wraps;
using Core.Infrastructure.Querying;
using Core.Repository;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business
{
    public class ComunicacaoBusiness : IComunicacaoBusiness
    {
        ICondominioRepository _condominioRepository;
        IAdministradoraRepository _administradoraRepository;
        IUsuarioRepository _usuarioRepository;

        static readonly string PATH = "comunicados.txt";

        public ComunicacaoBusiness(
            IAdministradoraRepository administradoraRepository,
            ICondominioRepository condominioRepository,
            IUsuarioRepository usuarioRepository)
        {
            _administradoraRepository = administradoraRepository;
            _condominioRepository = condominioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<BaseResponse> EnviarComunicadoAsync(BaseRequest<Comunicado> request)
        {
            var response = new BaseResponse();

            try
            {
                if (request.Value == null)
                    throw new ArgumentException($"O campo {nameof(request)} não pode ser null.");

                if (string.IsNullOrWhiteSpace(request.Value.Mensagem))
                    throw new ArgumentException($"O campo mensagem deve ser preenchido.", nameof(request.Value.Mensagem));

                var usuario = await _usuarioRepository.FindUsuarioAsync(request.Value.IdUsuario);

                if (usuario == null)
                    throw new ApplicationException("O usuário morador informado não está cadastrado na base de dados.");

                if (!Enum.GetValues(typeof(Model.Assunto))
                    .Cast<Model.Assunto>()
                    .Select(a => a.ToString())
                    .Contains(request.Value.Assunto))
                    throw new ApplicationException("O tipo de comunicado informado é inválido.");


                using (StreamWriter writer = File.AppendText(PATH))
                {
                    await writer.WriteLineAsync(Environment.NewLine);
                    await writer.WriteLineAsync($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Condomínio {usuario.Condominio.Nome}");

                    if (request.Value.Assunto == Model.Assunto.Administrativo.ToString())
                    {
                        var usuarioAdministradora = (await _usuarioRepository
                            .FindByAsync(new Query<Model.Usuario>(u
                                => u.IdCondominio == usuario.IdCondominio
                                && u.Tipo == Model.TipoUsuario.Administradora)))
                            .FirstOrDefault();

                        if(usuarioAdministradora == null)
                            throw new ApplicationException("O usuário administrador do condominio não está cadastrado na base de dados.");

                        await writer.WriteLineAsync($"Mensagem de {usuario.Nome} para Administradora {usuarioAdministradora.Nome}");
                    }
                    else
                    {
                        var responsavel = await _usuarioRepository
                            .FindByAsync(usuario.Condominio.IdResponsavel);

                        if (responsavel == null)
                            responsavel = (await _usuarioRepository
                                .FindByAsync(new Query<Model.Usuario>(u
                                    => u.IdCondominio == usuario.IdCondominio
                                    && u.Tipo == Model.TipoUsuario.Sindico)))
                                .FirstOrDefault();

                        if (responsavel == null)
                            responsavel = (await _usuarioRepository
                                .FindByAsync(new Query<Model.Usuario>(u
                                    => u.IdCondominio == usuario.IdCondominio
                                    && u.Tipo == Model.TipoUsuario.Zelador)))
                                .FirstOrDefault();

                        if (responsavel == null)
                            throw new ApplicationException("O usuário responsável não está cadastrado na base de dados.");

                        await writer.WriteLineAsync($"Mensagem de {usuario.Nome} para o {responsavel.Tipo.ToString()} {responsavel.Nome}");
                    }

                    await writer.WriteLineAsync(request.Value.Mensagem);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }
    }
}
