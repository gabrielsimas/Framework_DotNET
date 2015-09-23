using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Validacao
{
    public class ValidacaoHelper
    {
        public static ValidadorDeEntidadeResultado ValidateEntity<T>(T entity)
            where T : class
        {
            return new ValidadorDeEntidade<T>().Validate(entity);
        }
    }
}
