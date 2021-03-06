﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChoETL
{
    [ChoTypeConverter(typeof(Boolean))]
    public class ChoBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                string txt = value as string;
                txt = txt.NTrim();

                if (txt.IsNull())
                    return false;

                ChoBooleanFormatSpec booleanFormat = parameter.GetValueAt(0, ChoTypeConverterFormatSpec.Instance.BooleanFormat);
                switch (booleanFormat)
                {
                    case ChoBooleanFormatSpec.YOrN:
                        if (txt.Length == 1)
                            return txt[0] == 'Y' || txt[0] == 'y' ? true : false;
                        else
                            return false;
                    case ChoBooleanFormatSpec.TrueOrFalse:
                        return String.Compare(txt, "true", true) == 0 ? true : false;
                    case ChoBooleanFormatSpec.YesOrNo:
                        return String.Compare(txt, "yes", true) == 0 ? true : false;
                    default:
                        if (txt.Length == 1)
                            return txt[0] == '1' ? true : false;
                        else
                            return false;
                }
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    value = false;

                if (value is bool)
                {
                    bool boolValue = (bool)value;

                    ChoBooleanFormatSpec booleanFormat = parameter.GetValueAt(0, ChoTypeConverterFormatSpec.Instance.BooleanFormat);
                    switch (booleanFormat)
                    {
                        case ChoBooleanFormatSpec.YOrN:
                            return boolValue ? "Y" : "N";
                        case ChoBooleanFormatSpec.TrueOrFalse:
                            return boolValue ? "True" : "False";
                        case ChoBooleanFormatSpec.YesOrNo:
                            return boolValue ? "Yes" : "No";
                        default:
                            return boolValue ? "1" : "0";
                    }
                }
            }

            return value;
        }
    }
}
