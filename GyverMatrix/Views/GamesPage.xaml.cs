using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage : ContentPage
    {

        Dictionary<string, int> nameToGame = new Dictionary<string, int>
        {
            { "Змейка", 0 },
            { "Тетрис", 1 }, 
            { "Динозаврик", 2 }, 
            { "Лабиринт", 3 }, 
            { "Арканоид", 4 }, 
        };
        
    public GamesPage()
        {

            //foreach (string gameName in nameToGame.Keys)
            //{
            //    Games.Items.Add(gameName);
            //}

            InitializeComponent();
        }
    }
}