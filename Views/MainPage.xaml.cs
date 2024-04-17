using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using ThemeDump.ViewModels;

namespace ThemeDump.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private void onDumpTheme(object sender, RoutedEventArgs e)
    {
        var keys = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("Assets/keys.json"));
        var theme = new Dictionary<string, Dictionary<string, string>>();

        var brushes = keys["brush"];
        var colors = keys["color"];

        theme.Add("brush", new Dictionary<string, string>());
        theme.Add("color", new Dictionary<string, string>());

        foreach (var key in brushes)
        {
            var brush = Application.Current.Resources[key] as SolidColorBrush;
            var color = brush != null ? brush.Color.ToString() : "null";
            theme["brush"].Add(key, color);
        }

        foreach (var key in colors)
        {
            var color = Application.Current.Resources[key].ToString();
            theme["color"].Add(key, color != null ? color.ToString() : "null");
        }

        File.WriteAllText($"Assets/theme-{Application.Current.RequestedTheme}.json", JsonConvert.SerializeObject(theme, Formatting.Indented));
    }
}
