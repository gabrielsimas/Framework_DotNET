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
    public class EmailAttribute: RegexAttribute
    {
        public EmailAttribute()
            :base(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",RegexOptions.IgnoreCase)
        {
        }
    }
}
