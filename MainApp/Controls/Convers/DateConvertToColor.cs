﻿using System;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace MainApp.Controls.Convers
{
    public class DateConvertToColor : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            var calendarDayButton = (CalendarDayButton)values;
            var dateTime = (DateTime)calendarDayButton.DataContext;
            if (!calendarDayButton.IsMouseOver && !calendarDayButton.IsSelected && !calendarDayButton.IsBlackedOut && (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday))
                return new SolidColorBrush(Color.FromArgb(255, 255, 47, 47));
            else
                return new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
