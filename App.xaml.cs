using System;
using System.Linq;
using System.Windows;

namespace ComicVerse
{
    public partial class App : Application
    {
        public static void SelectCulture(string cultureCode)
        {
            ResourceDictionary dict = new ResourceDictionary();     
            dict.Source = new Uri($"Resources/Lang/Lang.{cultureCode}.xaml", UriKind.Relative);

            var oldDict = Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Lang."));

            if (oldDict != null) Current.Resources.MergedDictionaries.Remove(oldDict);
            Current.Resources.MergedDictionaries.Add(dict);
        }

        public static void SelectTheme(string theme)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"Resources/Styles/{theme}Theme.xaml", UriKind.Relative);

            var oldTheme = Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));

            if (oldTheme != null) Current.Resources.MergedDictionaries.Remove(oldTheme);
            Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}