using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawInitMenu()
            {
                RenderClear();

                Draw((screenwidth / 2) - (screenheight / 4), 0, screenheight / 2, screenheight / 2, InitialImages["SSLogo"]);



                Draw(screenwidth / 10, (screenheight * 9) / 10, (screenwidth * 3) / 10, screenheight / 20, InitialImages["ProgressBar"]);
                Draw(
                    screenwidth / 10 + 3, 
                    screenheight * 9 / 10, 
                    (screenwidth * 3 / 10 - 6) * InitialisePercent / 100,
                    screenheight / 20, InitialImages["ProgressInSection"]);
                Write(
                    screenwidth / 10, screenheight * 8 / 10, 
                    (screenwidth * 3 / 10) / "Loading... (000%)".Length,
                    screenheight / 20,
                    $"Loading... ({GetPercentage(InitialisePercent)})",
                    White);


                Draw((screenwidth * 6) / 10, (screenheight * 9) / 10, (screenwidth * 3) / 10, screenheight / 20, InitialImages["ProgressBar"]);
                Draw(
                        screenwidth * 6 / 10 + 3,
                        screenheight * 9 / 10,
                        (screenwidth * 3 / 10 - 6) * InitFunctionsProgress / InitFunctions.Count,
                        screenheight / 20, InitialImages["ProgressInSection"]);
                if (CurrentLoaderFunction.Length != 0)
                {
                    Write(
                        screenwidth * 6 / 10, screenheight * 8 / 10,
                        (screenwidth * 3 / 10) / CurrentLoaderFunction.Length,
                        screenheight / 20,
                        CurrentLoaderFunction,
                        White);
                }
            }




            public static string GetPercentage(int value)
            {
                if (value < 10)
                {
                    return "  " + value.ToString(CultureInfo.CurrentCulture) + "%";
                }
                else if (value != 100)
                {
                    return " " + value.ToString(CultureInfo.CurrentCulture) + "%";
                }
                return value.ToString(CultureInfo.CurrentCulture) + "%";
            }



            public static string CutoffString(string text, int length)
            {
                text ??= "";
                if (text.Length < length)
                {
                    return text;
                }
                else
                {
                    return string.Concat(text.AsSpan(length - 3), "...");
                }
            }
        }
    }
}
