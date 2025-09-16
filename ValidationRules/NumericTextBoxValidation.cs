using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CRM.ValidationRules
{
    public class NumericTextBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value is string v && v.Length > 12)
            {
                return new ValidationResult(false, "Значение толжно быть цифровым и не больше 12 символов");
            }

            return ValidationResult.ValidResult;
        }
    }
}
