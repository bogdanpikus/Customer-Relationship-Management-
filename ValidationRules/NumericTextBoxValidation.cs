using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CRM.ValidationRules
{
    public class NumericTextBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? text = value.ToString();
            MessageBox.Show($"{text}");
            Regex regex = new(@"^[\d]{12}$");

            if (text.Length > 2)
            {
                return new ValidationResult(false, "Разрешен только ввод цифр");
            }

            return ValidationResult.ValidResult;
        }
    }
}
