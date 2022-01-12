using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GyverMatrix.Helpers
{
    internal class ParseHelper
    {
        public static async Task SetSettingsAsync(string message)
        {

            string[] parsedArray1 = message.Split(' ', ';');

            string[] parsedArray2 = parsedArray1[1].Split('|');

            for (int i = 0; i < parsedArray2.Length; i++)
            {
                
                string[] parsedArray3 = parsedArray2[i].Split(':');
                try {
                    await SecureStorage.SetAsync(parsedArray3[0] + " - " + parsedArray3[0], parsedArray3[1]);
                    Console.WriteLine(await SecureStorage.GetAsync(parsedArray3[0]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Storage err - "+ex);
                }
                //Console.WriteLine(parsedArray3[0] + " - " + parsedArray3[1]);

                }

        }

        public static async Task SetEffects(string message)
        {

            string[] parsedArray1 = message.Split(':');

            string[] parsedArray2 = parsedArray1[1].Split('[', ']');

            await SecureStorage.SetAsync("Effects", parsedArray2[1]);
            Console.WriteLine(parsedArray2[1]);


        }

        public static async Task SetGames(string message)
        {

            string[] parsedArray1 = message.Split(':');

            string[] parsedArray2 = parsedArray1[1].Split('[', ']');

            await SecureStorage.SetAsync("Games", parsedArray2[1]);
            Console.WriteLine(parsedArray2[1]);


        }

    }
}
