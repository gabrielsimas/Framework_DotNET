using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Arquitetura.DDD.Aplicacao.Interfaces
{
    public interface IAplicacaoBase<T>
        where T: class
    {
        void Inserir(T entidade);
        void Apagar(T entidade);
        void Alterar(T entidade);
        ICollection<T> ListarTudo();
        T ListarPorId(int id);
        void Dispose();
    }
}
