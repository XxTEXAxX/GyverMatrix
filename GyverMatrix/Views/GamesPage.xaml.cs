﻿using Xamarin.Forms.Xaml;

namespace GyverMatrix.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage {
        //Dictionary<string, int> nameToGame = new Dictionary<string, int>
        //{
        //    { "Змейка", 0 },
        //    { "Тетрис", 1 },
        //    { "Динозаврик", 2 },
        //    { "Лабиринт", 3 },
        //    { "Арканоид", 4 },
        //};
        public GamesPage() {
            //foreach (string gameName in nameToGame.Keys)
            //{
            //    Games.Items.Add(gameName);
            //}
            InitializeComponent();
        }
        private void GameSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {

        }
    }
}