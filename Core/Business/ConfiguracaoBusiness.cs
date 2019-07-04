using Core.Business.DTOs;
using Core.Business.Wraps;
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
        IAssuntoRepository _assuntoRepository;

        public ConfiguracaoBusiness(
            IAdministradoraRepository administradoraRepository,
            IAssuntoRepository assuntoRepository,
            ICondominioRepository condominioRepository,
            IUsuarioRepository usuarioRepository)
        {
            _administradoraRepository = administradoraRepository;
            _assuntoRepository = assuntoRepository;
            _condominioRepository = condominioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public Task<BaseResponse<Condominio>> BuscarCondominioAsync(BaseRequest<int> request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<Usuario>> BuscarUsuarioAsync(BaseRequest<int> request)
        {
            throw new NotImplementedException();
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
                var assuntos = await _assuntoRepository.FindAllAsync();

                response.Result = assuntos
                    .Select(a => new Assunto
                    {
                        Id = a.Id,
                        Descricao = a.Descricao
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
                        Nome = c.Nome
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
                        Nome = u.Nome                        
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

        public Task<BaseResponse> SalvarCondominioAsync(BaseRequest<Condominio> request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> SalvarUsuarioAsync(BaseRequest<Usuario> request)
        {
            throw new NotImplementedException();
        }
    }
}
