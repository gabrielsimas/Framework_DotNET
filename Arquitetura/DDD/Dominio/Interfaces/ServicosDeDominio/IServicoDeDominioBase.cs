using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Arquitetura.DDD.Dominio.Interfaces.ServicosDeDominio
{
    public interface IServicoDeDominioBase<T>
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
