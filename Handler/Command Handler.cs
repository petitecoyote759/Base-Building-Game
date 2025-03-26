using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;


namespace Base_Building_Game.Handlers
{
    internal static class CommandHandler
    {
        public static string CommandLine { get => commandLine; }
        internal static string commandLine = "";

        internal static readonly Dictionary<SDL_Keycode, string> validCharacters = new Dictionary<SDL_Keycode, string>()
        {
            { SDL_Keycode.SDLK_a, "a" },
            { SDL_Keycode.SDLK_b, "b" },
            { SDL_Keycode.SDLK_c, "c" },
            { SDL_Keycode.SDLK_d, "d" },
            { SDL_Keycode.SDLK_e, "e" },
            { SDL_Keycode.SDLK_f, "f" },
            { SDL_Keycode.SDLK_g, "g" },
            { SDL_Keycode.SDLK_h, "h" },
            { SDL_Keycode.SDLK_i, "i" },
            { SDL_Keycode.SDLK_j, "j" },
            { SDL_Keycode.SDLK_k, "k" },
            { SDL_Keycode.SDLK_l, "l" },
            { SDL_Keycode.SDLK_m, "m" },
            { SDL_Keycode.SDLK_n, "n" },
            { SDL_Keycode.SDLK_o, "o" },
            { SDL_Keycode.SDLK_p, "p" },
            { SDL_Keycode.SDLK_q, "q" },
            { SDL_Keycode.SDLK_r, "r" },
            { SDL_Keycode.SDLK_s, "s" },
            { SDL_Keycode.SDLK_t, "t" },
            { SDL_Keycode.SDLK_u, "u" },
            { SDL_Keycode.SDLK_v, "v" },
            { SDL_Keycode.SDLK_w, "w" },
            { SDL_Keycode.SDLK_x, "x" },
            { SDL_Keycode.SDLK_y, "y" },
            { SDL_Keycode.SDLK_z, "z" },

            { SDL_Keycode.SDLK_SLASH, "/" },
            { SDL_Keycode.SDLK_SPACE, " " },

            { SDL_Keycode.SDLK_0, "0" },
            { SDL_Keycode.SDLK_1, "1" },
            { SDL_Keycode.SDLK_2, "2" },
            { SDL_Keycode.SDLK_3, "3" },
            { SDL_Keycode.SDLK_4, "4" },
            { SDL_Keycode.SDLK_5, "5" },
            { SDL_Keycode.SDLK_6, "6" },
            { SDL_Keycode.SDLK_7, "7" },
            { SDL_Keycode.SDLK_8, "8" },
            { SDL_Keycode.SDLK_9, "9" },
        };

        public static void HandleCommands(SDL_Keycode inp)
        {
            if (validCharacters.ContainsKey(inp))
            {
                string character = validCharacters[inp];
                if (General.ActiveKeys[SDL_Keycode.SDLK_LSHIFT])
                {
                    commandLine += character.ToUpperInvariant();
                }
                else
                {
                    commandLine += character;
                }
            }

            if (inp == SDL_Keycode.SDLK_BACKSPACE)
            {
                if (commandLine.Length > 0)
                {
                    commandLine = commandLine[..^1];
                }
            }

            if (inp == SDL_Keycode.SDLK_RETURN)
            {
                RunCommand(commandLine);
                commandLine = "";
                General.TextBarOpen = false;
            }
        }




        private static void RunCommand(string commandLine)
        {

        }
    }
}
