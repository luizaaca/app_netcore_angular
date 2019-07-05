using Core.Business.DTOs;
using Core.Business.Wraps;
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

                var usuario = await _usuarioRepository.FindUsuarioCompletoAsync(request.Value.IdUsuario);

                if (usuario == null)
                    throw new ApplicationException("O usuário informado não está cadastrado na base de dados.");

                if (!Enum.GetValues(typeof(Model.Assunto))
                    .Cast<Model.Assunto>()
                    .Select(a => a.ToString())
                    .Contains(request.Value.Assunto))
                    throw new ApplicationException("O tipo de comunicado informado é inválido.");


                using (StreamWriter writer = new StreamWriter(PATH))
                {
                    await writer.WriteLineAsync(Environment.NewLine);
                    await writer.WriteLineAsync($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - Condomínio {usuario.Condominio.Nome}");

                    if (request.Value.Assunto == Model.Assunto.Administrativo.ToString())
                        await writer.WriteLineAsync($"Mensagem de {usuario.Nome} para Administradora {usuario.Condominio.Administradora.Nome}");
                    else
                        await writer.WriteLineAsync($"Mensagem de {usuario.Nome} para o {usuario.Condominio.Responsavel.Tipo.ToString()}  {usuario.Condominio.Responsavel.Nome}");

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
