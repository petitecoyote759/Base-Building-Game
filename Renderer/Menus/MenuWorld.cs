using Short_Tools;
using System.Numerics;
using static Short_Tools.General;
using static System.MathF;
using static System.Numerics.Vector2;
using IVect = Short_Tools.General.ShortIntVector2;
using Priority = Short_Tools.ShortDebugger.Priority;
using V2 = System.Numerics.Vector2;
using SDL2;
using static SDL2.SDL;
using Newtonsoft.Json;
using System.Text;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class MenuWorld : IDisposable
        {
            public string path;
            public string name;
            public IntPtr image;
            bool textureDestroyed = false;

            public MenuWorld(string path) 
            {
                this.path = path;
                string[] files = Directory.GetFiles(path);
                string? targetFile = null;
                foreach (string file in files)
                {
                    if (file.Split('\\').Last().Split('.').Last() == "SWrld") 
                    { 
                        string[] pathGroup = file.Split('\\').Take(file.Split('\\').Length - 1).ToArray();
                        targetFile = pathGroup[0] + "\\" + pathGroup[1];
                        break; 
                    }
                }
                if (targetFile is null)
                {
                    debugger.AddLog($"Error during world load, could not find SWrld in {path}", Priority.ERROR);
                }
                else
                {
                    string[] split = targetFile.Split('\\');
                    name = split.Last();
                    image = renderer.L(targetFile + "\\" + name + ".png");
                }
            }


            public void Dispose()
            {
                if (textureDestroyed) { return; }
                textureDestroyed = true;
                SDL_DestroyTexture(image);
            }



            ~MenuWorld()
            {
                if (textureDestroyed) { return; }
                textureDestroyed = true;
                SDL_DestroyTexture(image);
            }
        }
    }
}