using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Arquitetura.DDD.Aplicacao.Interfaces;
using Framework.Arquitetura.DDD.Dominio.Interfaces.ServicosDeDominio;

namespace Framework.Arquitetura.DDD.Aplicacao.Implementacao
{
    public class AplicacaoServicoBase<T>: IAplicacaoBase<T>, IDisposable
        where T: class
    {
        #region Atributos

        private IServicoDeDominioBase<T> dominio;

        #endregion

        #region Construtores
        public AplicacaoServicoBase(IServicoDeDominioBase<T> dominio)
        {
            this.dominio = dominio;
        }
        #endregion

        #region Métodos de Aplicação
        
        public void Inserir(T entidade)
        {
            dominio.Inserir(entidade);
        }

        public void Apagar(T entidade)
        {
            dominio.Apagar(entidade);
        }

        public void Alterar(T entidade)
        {
            dominio.Alterar(entidade);
        }

        public ICollection<T> ListarTudo()
        {
            return dominio.ListarTudo();
        }

        public T ListarPorId(int id)
        {
            return dominio.ListarPorId(id);
        }

        #endregion

        public void Dispose()
        {
            this.dominio.Dispose();
        }
    }
}
