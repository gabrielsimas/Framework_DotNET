using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Arquitetura.DDD.Dominio.Interfaces.Repositorios;
using Framework.Arquitetura.DDD.Dominio.Interfaces.ServicosDeDominio;

namespace Framework.Arquitetura.DDD.Dominio.Implementacao.EF
{
    /// <summary>
    /// Implementação da Camada de Serviços de Domínio.
    /// NECESSÁRIO ENTITY FRAMEWORK
    /// </summary>
    /// <typeparam name="T">Classe de Dominio</typeparam>
    public class ServicoDeDominioBase<T>: IServicoDeDominioBase<T>, IDisposable
        where T: class
    {

        #region Atributos

        private IRepositorioBase<T> repositorio;

        #endregion

        #region Construtores
        public ServicoDeDominioBase(IRepositorioBase<T> repositorio)
        {
            this.repositorio = repositorio;
        }

        #endregion

        #region Métodos de Seviços de Domínio
        public void Inserir(T entidade)
        {
            this.repositorio.Inserir(entidade);
        }

        public void Apagar(T entidade)
        {
            this.repositorio.Apagar(entidade);
        }

        public void Alterar(T entidade)
        {
            this.repositorio.Alterar(entidade);
        }

        public ICollection<T> ListarTudo()
        {
            return this.repositorio.ListarTudo();
        }

        public T ListarPorId(int id)
        {
            return repositorio.ListarPorId(id);
        }

        #endregion
        public void Dispose()
        {
            this.repositorio.Dispose();
        }
    }
}
