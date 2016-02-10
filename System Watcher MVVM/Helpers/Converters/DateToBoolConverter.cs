using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace System_Watcher_MVVM.Helpers.Converters
{
    public class DateToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var BTR = false;
            try
            {
                if (value != null)
                {
                    var logdate = (DateTime)value;
                    var duedate = logdate.AddDays(30);

                    if (DateTime.Compare(DateTime.Now, duedate) > 0)
                    {
                        BTR = true;
                    }
                    else
                    {
                        BTR = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return BTR;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
