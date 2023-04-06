using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Exception
{
    public class MismatchedEnumLengthsException : System.Exception
    {
        public MismatchedEnumLengthsException() : base("Length of values and enums are different. You are required to pass as many values as there are enums.")
        {
        }
    }
}
