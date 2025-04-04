using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;


#pragma warning disable CS0660

namespace Base_Building_Game
{




    /// <summary>
    /// <para> Class for getting user inputs using SDL, only works with the renderer in this libary or with any other SDL screens. </para>
    /// <para> Requires using the .HandleInputs() function at the start of your game loop. </para>
    /// <para> Example is as follows </para>
    /// <code>
    /// internal class Handler : ShortHandler
    /// {
    ///     internal Handler(ulong Flags = 0) : base(Flags) { }
    /// 
    ///     public override void Handle(string inp, bool down)
    ///     {
    ///         switch (inp)
    ///         {
    ///             case "a":
    ///                 // does something
    ///                 break;
    ///             case "d":
    ///                 // does something else
    ///                 break;
    ///         }
    ///     }
    /// }
    /// </code>
    /// </summary>
    public class XXShortHandler
    {
        /// <summary>
        /// <para> Flags for the short handler, for more details look in the README. </para>
        /// </summary>
        public enum Flag : ulong
        {
            /// <summary>
            /// <para> Displays debug info - more info in README. </para>
            /// </summary>
            Debug = 1
        }



        private bool debugging = false;

        /// <summary>
        /// <para> Currently active flags, find which ones are on via </para>
        /// <code> if (flags.Contains(Flag.flag))
        ///  {
        ///      // do stuff
        ///  }</code>
        /// </summary>
        protected readonly Flag[] flags;

        private List<ShortRef<XXShortButton>> buttons = new List<ShortRef<XXShortButton>>();


        /// <summary>
        /// <para> Creates the ShortHandler. </para>
        /// </summary>
        /// <param name="flags"> See ShortHandler.Flag for more info. </param>
        public XXShortHandler(params Flag[] flags)
        {
            flags ??= Array.Empty<Flag>();
            this.flags = flags;
            if (flags.Contains(Flag.Debug))
            {
                debugging = true;
            }
        }




        /// <summary>
        /// <para> Collects the inputs and passes them to the Handle function. </para>
        /// </summary>
        /// <param name="Running"> The running state of the app running the handler. </param>
        public void HandleInputs(ref bool Running)
        {
            General.profiler.StartProfile("Handle Inputs");
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT: // ensures that quitting works and runs cleanup code
                        Running = false;
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        UpdateButtons();
                        HandleMousePress(true);
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        HandleMousePress(false);
                        break;

                    case SDL_EventType.SDL_MOUSEWHEEL:
                        HandleMousePress(e.wheel.y > 0, true);
                        break;

                    case SDL_EventType.SDL_KEYDOWN:
                        if (debugging) { Console.WriteLine(e.key.keysym.sym.ToString().Substring(5)); } // cuts out the starting SDLK_
                        Handle(e.key.keysym.sym, true);
                        break;

                    case SDL_EventType.SDL_KEYUP:
                        if (debugging) { Console.WriteLine(e.key.keysym.sym.ToString().Substring(5)); } // cuts out the starting SDLK_
                        Handle(e.key.keysym.sym, false);
                        break;
                }
            }
            General.profiler.EndProfile("Handle Inputs");
        }


        /// <summary>
        /// <para> Function to be overrided with handling checks. </para>
        /// </summary>
        /// <param name="inp"> Input key. </param>
        /// <param name="down"> If the key is being pressed down. </param>
        public virtual void Handle(SDL_Keycode inp, bool down) { }




        internal virtual void HandleMousePress(bool down, bool mouseWheel = false)
        {

        }




        /// <summary>
        /// <para> Runs through the buttons in the list and sets their pressed state to true if it is clicked on. </para>
        /// </summary>
        private void UpdateButtons()
        {

            Short_Tools.General.ShortIntVector2 pos = Short_Tools.General.getMousePos();
            foreach (ShortRef<XXShortButton> button in buttons)
            {
                button.GetObj().CheckHit(pos);
            }
        }


        /// <summary>
        /// <para> Adds a ShortButton to the list of buttons to be checked every time a click is registered. </para>
        /// </summary>
        /// <param name="button"> The button to be checked. </param>
        public void AddButton(ref XXShortButton button)
        {
            buttons.Add(new ShortRef<XXShortButton>(ref button));
        }


        /// <summary>
        /// <para> Removes a ShortButton from the list of buttons to be checked whenever a click is registered </para>
        /// </summary>
        /// <param name="button"> The button to be removed. </param>
        /// <exception cref="ButtonNotFoundException"> Occurs when the button inputed could not be found in the checking list. </exception>
        public void RemoveButton(ref XXShortButton button)
        {
            foreach (ShortRef<XXShortButton> reference in buttons)
            {
                if (reference.GetObj() == button)
                {
                    buttons.Remove(reference);
                    return;
                }

                throw new ButtonNotFoundException();
            }
        }

    }

    /// <summary>
    /// <para> A class that references another class, useful in certain circumstances. </para>
    /// </summary>
    /// <typeparam name="T"> The type being referenced. </typeparam>
    public class ShortRef<T>
    {
        private T Obj;
        public ShortRef(ref T Obj)
        {
            this.Obj = Obj;
        }
        public ref T GetObj()
        {
            return ref Obj;
        }
    }



    public class XXShortButton
    {
        public Vector2 pos;
        public int width;
        public int height;

        private Action actions = EmptyAction;

        public XXShortButton(Vector2 pos, int w, int h, params Action[] actions)
        {
            this.pos = pos;
            width = w;
            height = h;
            foreach (Action action in actions)
            {
                this.actions += action;
            }
        }
        public XXShortButton(int x, int y, int w, int h, params Action[] actions)
        {
            pos = new Vector2(x, y);
            width = w;
            height = h;
            foreach (Action action in actions)
            {
                this.actions += action;
            }
        }

        public void CheckHit(Vector2 clickPos)
        {
            if (pos.X < clickPos.X && pos.X + width > clickPos.X)
            {
                if (pos.Y < clickPos.Y && pos.Y + height > clickPos.Y)
                {
                    actions();
                }
            }
        }


        private static void EmptyAction() { }
    }









    /// <summary>
    /// Occurs
    /// </summary>
    public class ButtonNotFoundException : Exception
    {
        public override string Message => base.Message;
        public ButtonNotFoundException(string? message = "Could not find button in list") : base(message) { }
    }
}
