using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using System.Numerics;
using static System.Numerics.Vector2;
using System.Text.RegularExpressions;





namespace Base_Building_Game
{
    public static partial class General
    {
        public class Cutscene : Animation
        {
            // want array of tuples -> <IntPtr, int> for images and time in milliseconds that they are loaded for
            // make file type .ctscn -> name of image:time of image
            // new line for each one.
            // should we do that or
            // sprite sheet -> one big ass image -> images after each image. Then each animation would be just an int[]
            // could be a bit long tho -> i think ima do the .ctscn thing + bitmap errors

            //public Tuple<IntPtr, int> Frames { get; }
            public IntPtr CurrentFrame { get => Frames[currentFrame].Item1; }
            public bool Finished { get; set; } = false;
            public object ObjGetter { get => null; }


            Tuple<IntPtr, int>[] Frames;
            int dtSum = 0;
            int currentFrame = 0;
            int CurrentFrameDt = 0;

            /* if the dt's are like 10, 12, 51, 32, and currentFrame = 2, then currentFrameDt is the sum
               of the previous, so 10 + 12 = 22 cause then you can do dtSum - CurrentFrameDt, 
               and if thats greater than Frames[currentFrame], then you move to the next frame
            */
            public void Tick(long dt) 
            {

                dtSum += (int)dt;
                if (dtSum - CurrentFrameDt > Frames[currentFrame].Item2)
                {
                    CurrentFrameDt += Frames[currentFrame].Item2;
                    currentFrame++;

                    if (currentFrame >= Frames.Length) 
                    { 
                        renderer.ActiveCutscene = null; 
                        currentFrame--;  
                        Finished = true;
                        dtSum = int.MinValue;
                        return; 
                    }
                }
            }












            public Cutscene(string data, string CutName)
            {
                Regex regex = new Regex(@" *(\w+) *[:\- \|] *(\d+)[,;]? *", RegexOptions.Compiled);

                MatchCollection stuff = regex.Matches(data);

                if (stuff.Count == 0) { debugger.AddLog($"Cutscene {CutName} did not have any data.", ShortDebugger.Priority.WARN); }

                List<Tuple<IntPtr, int>> FrameList = new List<Tuple<IntPtr, int>>();

                foreach (Match match in stuff)
                {
                    if (!match.Success)
                    {
                        debugger.AddLog($"Incorrect format of cutscene {CutName} -> {match.ToString()}, ignoring", ShortDebugger.Priority.ERROR);
                    }

                    string name = match.Groups[1].Value;
                    string time = match.Groups[2].Value;

                    if (CutsceneImages.ContainsKey(name) && int.TryParse(time, out int result))
                    {
                        FrameList.Add(new Tuple<IntPtr, int>(CutsceneImages[name], result));
                    }
                    else
                    {
                        if (!CutsceneImages.ContainsKey(name))
                        {
                            debugger.AddLog($"Cannot find image \"{name}\" in cutscene {CutName}", ShortDebugger.Priority.ERROR);
                        }
                        else
                        {
                            debugger.AddLog($"Time for frame not in correct format -> {time} in cutscene {CutName}", ShortDebugger.Priority.ERROR);
                        }
                    }
                }

                Frames = FrameList.ToArray();
            }








            public static void Cleanup()
            {
                foreach (var pair in Cutscenes)
                {
                    foreach (Tuple<IntPtr, int> ImageSet in pair.Value.Frames)
                    {
                        SDL2.SDL.SDL_DestroyTexture(ImageSet.Item1);
                    }
                }
            }
        }





        public static void PlayCutscene(string name)
        {
            renderer.animations.Add(Cutscenes[name]);
            renderer.ActiveCutscene = Cutscenes[name];
        }
    }
}
