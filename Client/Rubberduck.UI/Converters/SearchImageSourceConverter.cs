﻿using System;
using System.Globalization;

namespace Rubberduck.UI.Converters
{
    public class SearchImageSourceConverter : ImageSourceConverter
    {
        //private readonly ImageSource _search = ToImageSource(RubberduckUI.magnifier_medium);
        //private readonly ImageSource _clear = ToImageSource(RubberduckUI.cross_script);

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null!;// string.IsNullOrEmpty(value?.ToString()) ? _search : _clear;
        }
    }
}
