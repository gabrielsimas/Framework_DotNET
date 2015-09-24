using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Arquitetura.DDD.Dominio.Interfaces.Repositorios;

namespace Framework.Arquitetura.DDD.InfraEstrutura.Repositorios.EF
{
    /// <summary>
    /// Implementação do RepositorioBase do Entity Framework
    /// </summary>
    /// <typeparam name="T">Classe de Dominio</typeparam>
    /// <typeparam name="D">Classe de acesso a Dados extendendo DbContext</typeparam>
    public class RepositorioBase<T,D>: IRepositorioBase<T>
        where T: class
        where D: DbContext, new()
    {
        #region Atributos

        protected D contexto; 

        #endregion

        #region Construtores

        public RepositorioBase()
        {
            contexto = new D();
        }

        #endregion
        public void Inserir(T entidade)
        {
            contexto.Entry(entidade).State = EntityState.Added;
            contexto.SaveChanges();
        }

        public void Apagar(T entidade)
        {
            contexto.Entry(entidade).State = EntityState.Deleted;
            contexto.SaveChanges();
        }

        public void Alterar(T entidade)
        {
            contexto.Entry(entidade).State = EntityState.Modified;
            contexto.SaveChanges();
        }

        public ICollection<T> ListarTudo()
        {
            return contexto.Set<T>().ToList();
        }

        public T ListarPorId(int id)
        {
            return contexto.Set<T>().Find(id);
        }

        public void Dispose()
        {
            contexto.Dispose();
        }
    }
}
