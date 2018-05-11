using System;
using System.Text.RegularExpressions;

namespace YH.Etms.Settlement.Api.Application.Validations.Validators
{
    public class RegexHelper
    {
        public static Regex CreateRegEx(string expression)
        {
#if NETSTANDARD1_0
			return new Regex(expression, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2.0));
#else
            try
            {
                if (AppDomain.CurrentDomain.GetData("REGEX_DEFAULT_MATCH_TIMEOUT") == null)
                {
                    return new Regex(expression, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2.0));
                }
            }
            catch
            {
            }


            return new Regex(expression, RegexOptions.IgnoreCase);
#endif
        }
    }
}
