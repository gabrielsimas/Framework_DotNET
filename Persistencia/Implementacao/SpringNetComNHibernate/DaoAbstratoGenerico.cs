using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Persistencia.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;

namespace Framework.Persistencia.Implementacao.SpringNetComNHibernate
{
    
    [Repository]
    public class DaoAbstratoGenerico<T>: IDaoGenerico<T> where T: class
    {
        #region Attributos
        private ISessionFactory sessionFactory;
        #endregion
        
        #region Construtores
        public DaoAbstratoGenerico()
        {

        }
        #endregion

        #region Propriedades
        public ISessionFactory SessionFactory
        {
            get
            {
                return this.sessionFactory;
            }

            set
            {
                this.sessionFactory = value;
            }
        }
        #endregion

        #region Métodos de Acesso a Dados

        [Transaction(ReadOnly = false)]
        public void Criar(T entidade)
        {
            this.SessionFactory.GetCurrentSession().Save(entidade);
        }

        [Transaction(ReadOnly = true)]
        public T BuscarPorId(Nullable<long> id)
        {
            //return this.SessionFactory.GetCurrentSession().Get<T>(id);            
            return this.SessionFactory.OpenStatelessSession().Get<T>(id);
        }

        [Transaction(ReadOnly = true)]
        public IList<T> BuscarTodos()
        {
            return this.SessionFactory.GetCurrentSession().CreateCriteria(typeof(T)).List<T>().OfType<T>().ToList();
        }

        [Transaction(ReadOnly = true)]
        public T FiltrarResultadoPor(Func<T, bool> predicado)
        {
            return this.SessionFactory.GetCurrentSession().Get<T>(predicado);
        }

        [Transaction(ReadOnly = true)]
        public IList<T> FiltrarResultadosPor(Func<T, bool> predicado)
        {
            return this.SessionFactory.GetCurrentSession().CreateCriteria(typeof(T)).List<T>().OfType<T>().Where(predicado).ToList();
        }

        [Transaction(ReadOnly = false)]
        public void Atualizar(T entidade)
        {
            this.SessionFactory.GetCurrentSession().Update(entidade);
        }

        [Transaction(ReadOnly = false)]
        public void Excluir(T entidade)
        {
            this.SessionFactory.GetCurrentSession().Delete(entidade);
        }

        #endregion                   
    }
}
