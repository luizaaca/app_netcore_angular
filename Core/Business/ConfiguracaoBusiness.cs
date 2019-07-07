using Core.Business.DTOs;
using Core.Business.Wraps;
using Core.Infrastructure.Querying;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business
{
    public class ConfiguracaoBusiness : IConfiguracaoBusiness
    {
        ICondominioRepository _condominioRepository;
        IAdministradoraRepository _administradoraRepository;
        IUsuarioRepository _usuarioRepository;

        public ConfiguracaoBusiness(
            IAdministradoraRepository administradoraRepository,
            ICondominioRepository condominioRepository,
            IUsuarioRepository usuarioRepository)
        {
            _administradoraRepository = administradoraRepository;
            _condominioRepository = condominioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<BaseResponse<Condominio>> BuscarCondominioAsync(BaseRequest<int> request)
        {
            var response = new BaseResponse<Condominio>();

            try
            {
                var result = await _condominioRepository.FindByAsync(request.Value);

                response.Result = new Condominio
                {
                    Id = result.Id,
                    Nome = result.Nome,
                    IdAdministradora = result.IdAdministradora,
                    IdResponsavel = result.IdResponsavel
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<Usuario>> BuscarUsuarioAsync(BaseRequest<int> request)
        {
            var response = new BaseResponse<Usuario>();

            try
            {
                var result = await _usuarioRepository.FindByAsync(request.Value);

                response.Result = new Usuario
                {
                    Id = result.Id,
                    Nome = result.Nome,
                    Tipo = new TipoUsuario
                    {
                        Id = (byte)result.Tipo,
                        Nome = result.Tipo.ToString()
                    },
                    IdCondominio = result.IdCondominio
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<IList<Administradora>>> ListarAdministradorasAsync()
        {
            var response = new BaseResponse<IList<Administradora>>();

            try
            {
                var administradoras = await _administradoraRepository.FindAllAsync();

                response.Result = administradoras
                    .Select(a => new Administradora
                    {
                        Id = a.Id,
                        Nome = a.Nome
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<IList<Assunto>>> ListarAssuntosAsync()
        {
            var response = new BaseResponse<IList<Assunto>>();

            try
            {
                response.Result =
                    Enum.GetValues(typeof(Model.Assunto))
                    .Cast<Model.Assunto>()
                    .Select(a => new Assunto
                    {
                        Descricao = a.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<IList<Condominio>>> ListarCondominiosAsync()
        {
            var response = new BaseResponse<IList<Condominio>>();

            try
            {
                var condominios = await _condominioRepository.FindAllAsync();

                response.Result = condominios
                    .Select(c => new Condominio
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        IdAdministradora = c.IdAdministradora,
                        IdResponsavel = c.IdResponsavel
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<IList<Usuario>>> ListarUsuariosAsync()
        {
            var response = new BaseResponse<IList<Usuario>>();

            try
            {
                var usuarios = await _usuarioRepository.FindAllAsync();

                response.Result = usuarios
                    .Select(u => new Usuario
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        IdCondominio = u.IdCondominio,
                        Tipo = new TipoUsuario
                        {
                            Id = (byte)u.Tipo,
                            Nome = u.Tipo.ToString()
                        }
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<int>> SalvarCondominioAsync(BaseRequest<Condominio> request)
        {
            var response = new BaseResponse<int>();

            try
            {
                if (request.Value == null)
                    throw new ArgumentException($"O campo {nameof(request)} não pode ser null.");

                var condominio = await _condominioRepository.FindByAsync(request.Value.Id);

                if (condominio == null)
                    condominio = new Model.Condominio();

                var administradora = await _administradoraRepository
                    .FindByAsync(request.Value.IdAdministradora);

                condominio.IdAdministradora = administradora?.Id ??
                    throw new ArgumentException("A administradora informada não foi encontrada na base de dados.");

                var responsavel = await _usuarioRepository.FindByAsync(request.Value.IdResponsavel);

                if (responsavel != null)
                    if (!new Model.TipoUsuario[] { Model.TipoUsuario.Zelador, Model.TipoUsuario.Sindico }.Contains(responsavel.Tipo))
                        throw new ApplicationException("O usuário informado não pode ser atribuido como responsável de um condomínio.");

                condominio.IdResponsavel = request.Value.IdResponsavel;
                condominio.Nome = request.Value.Nome ?? throw new ArgumentException("O nome do condomínio deve ser preenchido.");

                _condominioRepository.Save(condominio);

                await _condominioRepository.CommitAsync();

                response.Result = condominio.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }

        public async Task<BaseResponse<int>> SalvarUsuarioAsync(BaseRequest<Usuario> request)
        {
            var response = new BaseResponse<int>();

            try
            {
                if (request.Value == null)
                    throw new ArgumentException($"O campo {nameof(request)} não pode ser null.");

                var usuario = await _usuarioRepository.FindByAsync(request.Value.Id);

                if (usuario == null)
                    usuario = new Model.Usuario();

                if (await _condominioRepository
                    .CountAsync(new Query<Model.Condominio>(c => c.Id == request.Value.IdCondominio)) == 0)
                    throw new ApplicationException("O condomínio informado não foi encontrado na base de dados.");

                usuario.IdCondominio = request.Value.IdCondominio;
                usuario.Nome = request.Value.Nome ?? throw new ArgumentException("O campo nome deve ser informado.");

                if (Enum.IsDefined(typeof(Model.TipoUsuario), request.Value.Tipo.Id))
                    usuario.Tipo = (Model.TipoUsuario)request.Value.Tipo.Id;
                else
                    throw new ArgumentException("O campo tipo de usuário informado é inválido.");

                _usuarioRepository.Save(usuario);

                await _usuarioRepository.CommitAsync();

                response.Result = usuario.Id;
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
