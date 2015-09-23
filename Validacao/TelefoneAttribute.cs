using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework.Validacao
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
    AllowMultiple = false, Inherited = true)]
    public class TelefoneAttribute : RegexAttribute
    {
        public TelefoneAttribute()
            : base("^[0-9]*$",RegexOptions.IgnoreCase)
        {

        }
    }
}
