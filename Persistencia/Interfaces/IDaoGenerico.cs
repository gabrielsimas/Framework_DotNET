using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistencia.Interfaces
{
    public interface IDaoGenerico<T>  where T : class        
    {
        void Criar(T entidade);
        T BuscarPorId(Nullable<long> id);
        IList<T> BuscarTodos();
        T FiltrarResultadoPor(Func<T, bool> predicado);
        IList<T> FiltrarResultadosPor(Func<T, bool> predicado);       
        void Atualizar(T entidade);
        void Excluir(T entidade);       
    }
}
