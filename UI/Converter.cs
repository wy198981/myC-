using System;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UI
{
    public class ItosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BinModel.lstOpenType[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < BinModel.lstOpenType.Count; i++)
            {
                if ((string)value == BinModel.lstOpenType[i])
                {
                    return i;
                } 
            }
            return 0;
        }
    }

    public class JHConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BinModel.lstCtrlNumber[(int)value - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class IOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BinModel.lstInOut[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < BinModel.lstInOut.Count; i++)
            {
                if ((string)value == BinModel.lstInOut[i])
                {
                    return i;
                }
            }
            return 0;
        }
    }

    public class BSConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BinModel.lstBigSmall[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < BinModel.lstBigSmall.Count; i++)
            {
                if ((string)value == BinModel.lstBigSmall[i])
                {
                    return i;
                }
            }
            return 1;
        }
    }

    public class XYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BinModel.lstXieYi[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < BinModel.lstXieYi.Count; i++)
            {
                if ((string)value == BinModel.lstXieYi[i])
                {
                    return i;
                }
            }
            return 1;
        }
    }


    public class CIPConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            string val = value.ToString();

            return val.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }


    public class OperatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            string val = value.ToString();

            return val.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    public class VideoTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class PersonVideoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (BinModel.lstPersonVideo.Count > 0)
                return BinModel.lstPersonVideo[(int)value];
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < BinModel.lstInOut.Count; i++)
            {
                if ((string)value == BinModel.lstPersonVideo[i])
                {
                    return i;
                }
            }
            return 0;
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            if (tmp.Equals("0001-01-01 00:00:00")) 
            {
                tmp = "";
            }
            return tmp;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CardStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = (string)value;
            if (tmp.Equals("0"))
            {
                tmp = "正常使用";
            }
            return tmp;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
