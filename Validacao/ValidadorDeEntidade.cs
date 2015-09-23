using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Validacao
{
    public class ValidadorDeEntidade<T> where T: class
    {
        public ValidadorDeEntidadeResultado Validate(T entity)
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new ValidadorDeEntidadeResultado(validationResults);
        }
    }
}
