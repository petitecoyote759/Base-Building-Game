using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Short_Tools;
using static Base_Building_Game.General;
using static SDL2.SDL;
using static Short_Tools.General;
using static Short_Tools.ShortDebugger;
using IVect = Short_Tools.General.ShortIntVector2;

// turn off all warnings cause there are so many, like xml missing or silly stuff like that, noone should be here anyway
#pragma warning disable




namespace Base_Building_Game
{
    /// <summary>
    /// <para> Uses SDL2 from sayers SDL2 Core to render to the screen. </para>
    /// <para> Example code for how to create a renderer is as follows: </para>
    /// <code>
    /// internal class Renderer : ShortRenderer
    /// {
    ///     internal Renderer(ulong Flags = 0) : base(Flags) { }
    ///
    ///     public override void Render()
    ///     {
    ///         RenderClear();
    ///     
    ///         Draw(0, 0, screenwidth, screenheight, images["card"]);
    ///         // your code here
    ///     
    ///         RenderDraw();
    ///     }
    /// }
    ///
    /// </code>
    /// </summary>
    public class XXShortRenderer : ISLoggingObject
    {
        /// <summary>
        /// <para> Flags for the short renderer, for more details look in the README. </para>
        /// </summary>
        public enum Flag : ulong
        {
            /// <summary>
            /// <para> Automatically clears the screen before drawing, and draws the items to screen so you dont have to. </para>
            /// <para> write RenderClear(); and at the end RenderDraw(); </para>
            /// </summary>
            Auto_Draw_Clear,

            /// <summary>
            /// <para> If active, debug logs will be printed to the console. </para>
            /// </summary>
            Debug,

            /// <summary>
            /// <para> If active, debug logs will be written to a file on stop, activated by passing a path into the constructor. </para>
            /// </summary>
            Write_Log_To_File
        }



        public ShortDebugger debugger { get; set; }



        /// <summary>
        /// The animations currently active, they will be removed from this list when they are finished.
        /// </summary>
        public List<General.Animation> animations = new List<General.Animation>();
        /// <summary>
        /// Time taken for the last frame to render, calculated after RenderClear();
        /// </summary>
        public int dt { get; private set; } = 1;
        /// <summary>
        /// Last frame time.
        /// </summary>
        protected long LFT = 1;

        protected IntPtr window;
        public IntPtr SDLrenderer;
        protected IntPtr Font;
        public static readonly SDL_Color Black = new SDL_Color() { r = 0, g = 0, b = 0, a = 255 };
        public static readonly SDL_Color White = new SDL_Color() { r = 255, g = 255, b = 255, a = 255 };
        protected SDL_Rect tRect;
        public int screenwidth { get; private set; }
        public int screenheight { get; private set; }
        protected Thread controllerThread;
        public bool Running { get; set; } = true;
        public Dictionary<string, IntPtr> images;
        public Dictionary<string, Tuple<IntPtr, int, int>> animationImages;

        /// <summary>
        /// <para> Currently active flags, find which ones are on via </para>
        /// <code> if (flags.Contains(Flag.flag))
        ///  {
        ///      // do stuff
        ///  }</code>
        /// </summary>
        protected readonly Flag[] flags;




        string CurrentPath;
        IntPtr CurrentLoadedImage;
        bool MainThreadIsWaiting = false;



        /// <summary>
        /// <para> Runs the setup, uses default screen dimensions of 1920x1080, writes the logs to the file path given. </para>
        /// </summary>
        /// <param name="flags"> See ShortRenderer.Flag for more info. </param>
        /// <param name="path"> The path where the debug logs are written on renderer stop. </param>
        public XXShortRenderer(string path, params Flag[] flags)
        {
            //screenwidth = 1920;
            //screenheight = 1080;
            flags ??= Array.Empty<Flag>();
            this.flags = flags;
            if (flags.Contains(Flag.Debug))
            {
                debugger = new ShortDebugger("Renderer", path, ShortDebugger.Flags.DISPLAY_ON_ADD_LOG);
            }
            else
            {
                debugger = new ShortDebugger("Renderer", path);
            }
            debugger.AddLog("Renderer has been setup", Priority.INFO);
            if (!flags.Contains(Flag.Write_Log_To_File))
            {
                List<Flag> ListFlag = this.flags.ToList();
                ListFlag.Add(Flag.Write_Log_To_File);
                this.flags = ListFlag.ToArray();
            }
            images = new Dictionary<string, IntPtr>();



            // Initilizes SDL_image for use with png files.
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING); // SDL_INIT_EVERYTHING | SDL_INIT_VIDEO
            CheckSDLErrors();
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
            CheckSDLErrors();

            SDL.SDL_DisplayMode displayMode;
            if (SDL.SDL_GetCurrentDisplayMode(0, out displayMode) != 0)
            {
                Console.WriteLine($"SDL_GetCurrentDisplayMode failed! SDL_Error: {SDL.SDL_GetError()}");

                // Fallback: Use SDL_GetDesktopDisplayMode if SDL_GetCurrentDisplayMode fails
                if (SDL.SDL_GetDesktopDisplayMode(0, out displayMode) != 0)
                {
                    Console.WriteLine($"SDL_GetDesktopDisplayMode also failed! SDL_Error: {SDL.SDL_GetError()}");
                    SDL.SDL_Quit();
                    return;
                }
            }
            screenwidth = displayMode.w / 2;
            screenheight = displayMode.h / 2;

            controllerThread = new Thread(new ThreadStart(() => Controller_Thread(this)));
            controllerThread.Name = "ShortTools Rendering Thread";
        }






        /// <summary>
        /// <para> Sets up the renderer and screen. </para>
        /// </summary>
        public void Setup()
        {
            window = SDL.SDL_CreateWindow(
                "Window", 
                SDL.SDL_WINDOWPOS_CENTERED, 
                SDL.SDL_WINDOWPOS_CENTERED, screenwidth, screenheight, 
                0); //, SDL.SDL_WindowFlags.SDL_WINDOW_METAL | SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            
            SDLrenderer = SDL.SDL_CreateRenderer(window,
                                                    -1,
                                                    0); // SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            //                                             SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC


            //SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");

            //SDL_RenderSetLogicalSize(SDLrenderer, screenwidth, screenheight);

            SDL.SDL_SetRenderDrawColor(SDLrenderer, 60, 10, 70, 255);
            //SDL.SDL_GetPerformanceCounter
            int width = -1;
            int height = -1;
            SDL.SDL_GetWindowSize(window, out width, out height);
            screenwidth = width;
            screenheight = height;
        }

        /// <summary>
        /// <para> Draws, used as a template to be overrided. </para>
        /// </summary>
        public virtual void Render()
        {
            SDL_RenderClear(SDLrenderer);

            Thread.Sleep(100);

            SDL_RenderPresent(SDLrenderer);
        }

        /// <summary>
        /// <para> Clears the old screen, use at the start of the render function. </para>
        /// </summary>
        public void RenderClear()
        {
            SDL.SDL_RenderClear(SDLrenderer);
        }

        /// <summary>
        /// <para> Applies the drawings to the screen, use at the end of the render function. </para>
        /// </summary>
        public void RenderDraw()
        {
            SDL.SDL_RenderPresent(SDLrenderer);
        }

        /// <summary>
        /// <para> Starts the render thread. </para>
        /// </summary>
        public Thread Start()
        {
            if (controllerThread.ThreadState != ThreadState.Running)
            {
                controllerThread.Start();
                debugger.AddLog("Renderer started", Priority.INFO);
                return controllerThread;
            }
            debugger.AddLog("Short renderer \"Start\" function was called despite the thread running.", ShortDebugger.Priority.WARN);

            return controllerThread;
        }

        /// <summary>
        /// <para> Stops the render thread and cleans up all textures. </para>
        /// </summary>
        public void Stop()
        {
            Running = false;
            //controllerThread.Join();
            Cleanup();
            debugger.AddLog("Renderer has been stopped", Priority.INFO);
            if (flags.Contains(Flag.Write_Log_To_File))
            {
                debugger.Save();
                debugger.CleanFiles(5);
            }
        }


        /// <summary>
        /// Draws the given image to the screen at the given coordinates, and streched to fit the width and height parameters.
        /// </summary>
        /// <param name="xPos"> X position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="yPos"> Y position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="width"> Width of the image in pixels.</param>
        /// <param name="height"> Height of the image in pixels.</param>
        /// <param name="image"> The image given to draw.</param>
        public void Draw(int xPos, int yPos, int width, int height, IntPtr image)
        {
            tRect.x = xPos; tRect.y = yPos;
            tRect.w = width; tRect.h = height;
            SDL_RenderCopy(SDLrenderer, image, IntPtr.Zero, ref tRect);
        }

        /// <summary>
        /// Draws the given image to the screen at the given coordinates, and streched to fit the width and height parameters.
        /// </summary>
        /// <param name="xPos"> X position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="yPos"> Y position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="width"> Width of the image in pixels.</param>
        /// <param name="height"> Height of the image in pixels.</param>
        /// <param name="image"> The name of the image given to draw, as used in the images dictionary.</param>
        public void Draw(int xPos, int yPos, int width, int height, string image)
        {
            tRect.x = xPos; tRect.y = yPos;
            tRect.w = width; tRect.h = height;
            SDL_RenderCopy(SDLrenderer, images[image], IntPtr.Zero, ref tRect);
        }





        /// <summary>
        /// Draws the given image to the screen at the given coordinates, and streched to fit the width and height parameters.
        /// </summary>
        /// <param name="xPos"> X position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="yPos"> Y position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="width"> Width of the image in pixels.</param>
        /// <param name="height"> Height of the image in pixels.</param>
        /// <param name="image"> The image given to draw.</param>
        /// <param name="angle"> Angle of the image to be drawn in degrees from vertical clockwise.</param>
        public void Draw(int xPos, int yPos, int width, int height, IntPtr image, double angle)
        {
            tRect.x = xPos; tRect.y = yPos;
            tRect.w = width; tRect.h = height;
            SDL_RenderCopyEx(SDLrenderer, image, IntPtr.Zero, ref tRect, angle, IntPtr.Zero, SDL_RendererFlip.SDL_FLIP_NONE);
        }


        /// <summary>
        /// Draws the given image to the screen at the given coordinates, and streched to fit the width and height parameters.
        /// </summary>
        /// <param name="xPos"> X position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="yPos"> Y position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="width"> Width of the image in pixels.</param>
        /// <param name="height"> Height of the image in pixels.</param>
        /// <param name="image"> The name of the image given to draw, as used in the images dictionary.</param>
        /// <param name="angle"> Angle of the image to be drawn in degrees from vertical clockwise.</param>
        public void Draw(int xPos, int yPos, int width, int height, string image, double angle)
        {
            tRect.x = xPos; tRect.y = yPos;
            tRect.w = width; tRect.h = height;
            SDL_RenderCopyEx(SDLrenderer, images[image], IntPtr.Zero, ref tRect, angle, IntPtr.Zero, SDL_RendererFlip.SDL_FLIP_NONE);
        }






        /// <summary>
        /// <para> Writes the text given to the screen. </para>
        /// </summary>
        /// <param name="posx"> X position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="posy"> Y position of the bottom left corner of the image destination in pixels.</param>
        /// <param name="width"> Width of the image in pixels.</param>
        /// <param name="height"> Height of the image in pixels.</param>
        /// <param name="text"> Text to be written to the screen.</param>
        public void Write(int posx, int posy, int width, int height, string text, [Optional] SDL_Color? InColour)
        {
            SDL_Color colour;
            if (InColour is null) { colour = Black; }
            else { colour = (SDL_Color)InColour; }

            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(Font, text, colour);
            IntPtr Message = SDL_CreateTextureFromSurface(SDLrenderer, surfaceMessage);
            Draw(posx, posy, (int)(width * text.Length * 0.75f), height, Message);
            SDL_DestroyTexture(Message);
            SDL_FreeSurface(surfaceMessage);
        }


        /// <summary>
        /// Sets up text to be able to use with the Write function. Call after Setup.
        /// </summary>
        /// <param name="path"> Path to the ttf file of the font.</param>
        /// <exception cref="FontLoadingException"> Occurs when the text file given could not be found.</exception>
        public void SetupText(string path = "")
        {
            SDL_ttf.TTF_Init();
            Font = SDL_ttf.TTF_OpenFont(path, 24);
            if (Font == IntPtr.Zero)
            {
                //throw new FontLoadingException("Cannot find text file.");
                debugger.AddLog("Text could not load, Path = " + path, Priority.ERROR);
            }
            else
            {
                debugger.AddLog("Text sucessfully loaded", Priority.INFO);
            }
        }


        private void Animate()
        {
            List<General.Animation> ToRemove = new List<General.Animation>();

            General.Animation[] CurrentAnimations = animations.ToArray();
            foreach (General.Animation animation in CurrentAnimations)
            {
                animation.Tick(dt);
                if (animation.Finished) { ToRemove.Add(animation); }
            }

            //animations.RemoveAll(a => ToRemove.IndexOf(a) != -1);
            foreach (General.Animation animation in ToRemove)
            {
                animations.Remove(animation);
            }
        }


        public void CreateMoveAnimation<T>(Vector2 End, T obj, float speed, PositionAnimation<T>.Flags flag) where T : ISHasPosition
        {
            animations.Add(new PositionAnimation<T>(End, obj, speed, flag));
        }
        public void CreateImageAnimation<T>(T obj, IntPtr[] AnimationSheet, int timePerFrame) where T : ISAnimatableImage
        {
            animations.Add(new ImageAnimator<T>(obj, AnimationSheet, timePerFrame));
        }
        public void CreateImageAnimation<T>(T obj, string AnimationSheet, int timePerFrame) where T : ISAnimatableImage
        {
            animations.Add(new ImageAnimator<T>(obj, AnimationSheet, this, timePerFrame));
        }

        /// <summary>
        /// Removes any given animations from the animations list.
        /// </summary>
        /// <param name="InAnimations"> The animations to be removed </param>
        public void RemoveAnimation(params General.Animation[] InAnimations)
        {
            if (InAnimations == new General.Animation[0]) { return; }
            foreach (General.Animation animation in InAnimations)
            {
                animations.Remove(animation);
            }
        }





        /// <summary>
        /// <para> Loads images into the images dictionary for use with the draw function, example as follows. </para>
        /// <code>
        /// renderer.Load_Images
        /// (
        ///     new Dictionary()
        ///     {
        ///         { "image", "image.png" }
        ///         { "image2", "otherImage.png" }
        ///     }
        /// );
        /// </code>
        /// </summary>
        /// <param name="imagePaths"> Name of the image and the path of the image from the exe.</param>
        public void Load_Images(Dictionary<string, string>? imagePaths = null)
        {
            images = new Dictionary<string, IntPtr>();

            if (imagePaths is null)
            { 
                return;
            }

            foreach (var image in imagePaths)
            {
                if (image.Value.Split('\\').Last() != "")
                {
                    images.Add(image.Key, LoadImage(image.Value));
                }
                else
                {
                    images.Add(image.Key, IntPtr.Zero);
                }
            }



            // images.Add("image name", L("res\\image.png"));



            string missing = "";
            foreach (var image in images)
            {
                if (image.Value == IntPtr.Zero && imagePaths[image.Key].Split('\\').Last() != "")
                {
                    if (missing != "") { missing += ", "; }
                    missing += image.Key;
                }
            }
            if (missing.Length != 0)
            {
                debugger.AddLog("Error during image loading - could not complete. Textures missing : " + missing, Priority.WARN);
            }
            else
            {
                debugger.AddLog("Images Loaded sucessfully", Priority.INFO);
            }
        }



        public IntPtr LoadImage(string path)
        {
            CurrentPath = path;
            MainThreadIsWaiting = true;
            while (MainThreadIsWaiting) { Thread.Sleep(5); }
            return CurrentLoadedImage;
        }




        /// <summary>
        /// Loads a png or bitmap from the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IntPtr L(string path) // load image
        {
            if (path.Substring(path.Length - 3) == "bmp") 
            { 
                IntPtr image = SDL_LoadBMP(path);
                CheckSDLErrors();
                return image;
            }

            if (path.Substring(path.Length - 3) == "png") 
            { 
                IntPtr image = SDL_image.IMG_LoadTexture(SDLrenderer, path);
                CheckSDLErrors();
                return image;
            }



            debugger.AddLog("Invalid file type -> " + path.Substring(path.Length - 3), Priority.WARN);
            return IntPtr.Zero;
        }




        /// <summary>
        /// Destroys all textures, the renderer, and the screen, before quitting SDL. Called automatically by Stop();
        /// </summary>
        public void Cleanup()
        {
            foreach (var image in images)
            {
                SDL.SDL_DestroyTexture(image.Value);
            }

            SDL.SDL_DestroyRenderer(SDLrenderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }


        /// <summary>
        /// Starting point for the thread that runs the renderer
        /// </summary>
        /// <param name="renderer"> The renderer being controlled</param>
        private static void Controller_Thread(XXShortRenderer renderer)
        {
            renderer.Setup();

            SDL.SDL_SetRenderDrawColor(renderer.SDLrenderer, 0, 0, 0, 255);
            SDL_RenderClear(renderer.SDLrenderer);
            renderer.RenderDraw();


            

            //SDL.SDL_Rect viewport = new SDL.SDL_Rect { x = 0, y = 0, w = renderer.screenwidth, h = renderer.screenheight };
            //SDL.SDL_RenderSetViewport(renderer.SDLrenderer, ref viewport);



            if (renderer.flags.Contains(Flag.Auto_Draw_Clear))
            {
                while (renderer.Running)
                {
                    renderer.Animate();
                    renderer.RenderClear();
                    renderer.Render();
                    renderer.RenderDraw();
                    renderer.dt = (int)GetDt(ref renderer.LFT);
                }
            }
            else
            {
                while (renderer.Running)
                {
                    if (renderer.MainThreadIsWaiting)
                    {
                        renderer.CurrentLoadedImage = renderer.L(renderer.CurrentPath);
                        renderer.MainThreadIsWaiting = false;
                    }
                    if (renderer.TryingToEnlarge)
                    {
                        renderer.InternalEnlarge();
                        renderer.TryingToEnlarge = false;
                    }



                    renderer.Animate();
                    renderer.Render();
                    renderer.dt = (int)GetDt(ref renderer.LFT);
                }
            }
        }


        bool TryingToEnlarge = false;

        public void Enlarge()
        {
            TryingToEnlarge = true;
        }
        public void InternalEnlarge()
        { 
            screenwidth *= 2;
            screenheight *= 2;

            SDL_SetWindowSize(window, screenwidth, screenheight);
            SDL_SetWindowPosition(window, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        }




        public void CheckSDLErrors()
        {
            if (!General.Running) { return; }

            if (SDL_GetError() != "")
            {
                debugger.AddLog("SDLError -> " + SDL_GetError(), Priority.ERROR);
                SDL_ClearError();
            }
        }







        public void DrawTextureBetweenPoints(IntPtr texture, int x1, int y1, int x2, int y2, int w, int h)
        {

            // Calculate the distance between the two points
            int dx = x2 - x1;
            int dy = y2 - y1;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            // Calculate the angle of the line
            float angle = MathF.Atan2(dy, dx) * (180.0f / MathF.PI);

            // Calculate the number of tiles needed
            int numTiles = (int)(distance / w) + 1;

            // Draw the texture tiles
            for (int i = 0; i < numTiles; i++)
            {
                float t = (float)i / numTiles;
                int x = (int)(x1 + t * dx);
                int y = (int)(y1 + t * dy);

                SDL_Rect dstRect = new SDL_Rect
                {
                    x = x,
                    y = y,
                    w = w,
                    h = h
                };

                SDL_RenderCopyEx(SDLrenderer, texture, IntPtr.Zero, ref dstRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
        }








        /// <summary>
        /// Non wrapping texture between points
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void DrawNWTextureBetweenPoints(IntPtr texture, int x1, int y1, int x2, int y2)
        {
            // Calculate the distance and angle between the points
            float deltaX = x2 - x1;
            float deltaY = y2 - y1;
            float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            float angle = (float)(Math.Atan2(deltaY, deltaX) * (180.0 / Math.PI));

            // Get the width and height of the texture
            SDL.SDL_QueryTexture(texture, out _, out _, out int textureWidth, out int textureHeight);

            // Create the destination rectangle
            SDL.SDL_Rect destRect = new SDL.SDL_Rect
            {
                x = x1,
                y = y1,
                w = (int)distance,
                h = textureHeight
            };

            // Render the texture with rotation
            SDL.SDL_RenderCopyEx(SDLrenderer, texture, IntPtr.Zero, ref destRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }


































    /// <summary>
    /// Image animator that changes the image of an object.
    /// </summary>
    /// <typeparam name="T"> The type of the object to have its image changed. </typeparam>
    public class ImageAnimator<T> : General.Animation where T : ISAnimatableImage
    {
        T obj;
        public object ObjGetter { get => obj; }
        IntPtr[] AnimationSheet;
        IntPtr baseImage;
        long TimePerFrame;
        long TimeRemaining;
        int index;
        public bool Finished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="AnimationSheet"></param>
        /// <param name="TimePerFrame"> Time per frame in milliseconds. </param>
        public ImageAnimator(T obj, IntPtr[] AnimationSheet, int TimePerFrame)
        {
            this.obj = obj;
            baseImage = obj.image;
            this.AnimationSheet = AnimationSheet;
            this.TimePerFrame = TimePerFrame;
            TimeRemaining = TimePerFrame;
            index = 0;
            obj.image = AnimationSheet[index];
            Finished = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="AnimationSheet"></param>
        /// <param name="renderer"> The sdl renderer contained in the ShortRenderer class. </param>
        /// <param name="TimePerFrame"> Time per frame in milliseconds. </param>
        public ImageAnimator(T obj, string AnimationSheet, XXShortRenderer renderer, int TimePerFrame)
        {
            this.obj = obj;
            baseImage = obj.image;
            this.AnimationSheet = CreateSheet(AnimationSheet, renderer);
            this.TimePerFrame = TimePerFrame;
            TimeRemaining = TimePerFrame;
            index = 0;
            obj.image = this.AnimationSheet[index];
            Finished = false;
        }

        private IntPtr[] CreateSheet(string texture, XXShortRenderer renderer)
        {
            int w = renderer.animationImages[texture].Item2;
            int h = renderer.animationImages[texture].Item3;
            IntPtr image = renderer.animationImages[texture].Item1;

            IntPtr[] images = new IntPtr[w / h];

            SDL_Rect destrect = new SDL_Rect() { x = 0, y = 0, w = h, h = h };


            for (int i = 0; i < w; i += h)
            {
                IntPtr surface = SDL_CreateRGBSurface(0, h, h, 32, 0, 0, 0, 0);

                SDL_Rect srcrect = new SDL_Rect() { x = i, y = 0, w = h, h = h };

                SDL_BlitSurface(image, ref srcrect, surface, ref destrect);


                images[i / h] = SDL_CreateTextureFromSurface(renderer.SDLrenderer, surface);
            }


            return images;
        }






        public void Tick(long dt)
        {
            TimeRemaining -= dt;
            if (TimeRemaining < 0)
            {
                TimeRemaining = TimePerFrame + TimeRemaining;
                index++;
                if (index >= AnimationSheet.Length)
                {
                    Finished = true;
                    obj.image = baseImage;
                    return;
                }
                obj.image = AnimationSheet[index];
            }
        }
    }









    public class PositionAnimation<T> : General.Animation where T : ISHasPosition
    {
        /// <summary>
        /// <para> Flags used to determine which type of movement the objects will perform. </para>
        /// </summary>
        public enum Flags : byte
        {
            /// <summary>
            /// Speed is constant.
            /// </summary>
            Linear = 1,
            /// <summary>
            /// Speed increases exponentially over time.
            /// </summary>
            Exponential = 2,
            /// <summary>
            /// Speed decreases over time.
            /// </summary>
            Root = 3,
            /// <summary>
            /// Speed increases at the start, and decreases at the end, leads to the smoothest movement.
            /// </summary>
            Sigmoid = 4
        }



        public bool Finished { get; set; } = false;

        Vector2 Start;
        Vector2 End;
        T obj;
        public object ObjGetter { get => obj; }
        float speed;
        long dtSum;

        Action<long> TickFunc;

        public PositionAnimation(Vector2 target, T obj, float speed, Flags flag)
        {
            Start = obj.pos;
            End = target;
            this.obj = obj;
            this.speed = speed;

            TickFunc = flag switch
            {
                Flags.Linear => LinTick,
                Flags.Sigmoid => SigTick,
                Flags.Root => RootTick,
                Flags.Exponential => ExpoTick,
                _ => LinTick,
            };
        }


        public void Tick(long dt)
        {
            TickFunc(dt);
        }



        private void LinTick(long dt)
        {
            dtSum += dt;

            float ratio = speed * dtSum / 1000f;
            obj.pos = new Vector2(
                ratio * (End.X - Start.X) + Start.X,
                ratio * (End.Y - Start.Y) + Start.Y
                );

            if (ratio >= 1)
            {
                obj.pos = End;
                Finished = true;
            }
        }


        private void ExpoTick(long dt)
        {
            dtSum += dt;

            float ratio = (float)Math.Pow(2, (speed * dtSum / 1000f) * (speed * dtSum / 1000f)) - 1;
            obj.pos = new Vector2(
                ratio * (End.X - Start.X) + Start.X,
                ratio * (End.Y - Start.Y) + Start.Y
                );

            if (ratio >= 1)
            {
                obj.pos = End;
                Finished = true;
            }
        }

        private void RootTick(long dt)
        {
            dtSum += dt;

            float ratio = (float)Math.Sqrt(speed * dtSum / 1000f);
            obj.pos = new Vector2(
                ratio * (End.X - Start.X) + Start.X,
                ratio * (End.Y - Start.Y) + Start.Y
                );

            if (ratio >= 1)
            {
                obj.pos = End;
                Finished = true;
            }
        }

        private void SigTick(long dt)
        {
            dtSum += dt;

            float x = speed * dtSum / 1000f;
            float ratio = x * x * x * (10 + x * (6 * x - 15));
            obj.pos = new Vector2(
                ratio * (End.X - Start.X) + Start.X,
                ratio * (End.Y - Start.Y) + Start.Y
                );

            if (ratio >= 1)
            {
                obj.pos = End;
                Finished = true;
            }
        }
    }
}
