using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Validacao
{
	public class ValidadorDeEntidadeResultado
	{
		public IList<ValidationResult> Errors { get; private set; }
		public bool HasError
		{
			get { return Errors.Count > 0; }
		}

		public ValidadorDeEntidadeResultado(IList<ValidationResult> errors = null)
		{
			Errors = errors ?? new List<ValidationResult>();
		}
	}
}
