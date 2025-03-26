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
        internal static int commandLinePosition = 0;

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


            { SDL_Keycode.SDLK_LEFTBRACKET, "[" },
            { SDL_Keycode.SDLK_RIGHTBRACKET, "]" },
            { SDL_Keycode.SDLK_HASH, "#" },
            { SDL_Keycode.SDLK_SEMICOLON, ";" },
        };


        internal static readonly Dictionary<string, string> specialUpperCharacters = new Dictionary<string, string>()
        {
            { "[", "{" },
            { "]", "}" },
            { "#", "~" },
            { ";", ":" },

            { "0", ")" },
            { "1", "!" },
            { "2", "\"" },
            { "3", "£" },
            { "4", "$" },
            { "5", "%" },
            { "6", "^" },
            { "7", "&" },
            { "8", "*" },
            { "9", "(" },
        };



        public static void HandleCommands(SDL_Keycode inp)
        {
            commandLinePosition = Math.Min(commandLine.Length, commandLinePosition);

            if (inp == SDL_Keycode.SDLK_LEFT)
            {
                commandLinePosition = Math.Max(0, commandLinePosition - 1);
                return;
            }
            if (inp == SDL_Keycode.SDLK_RIGHT)
            {
                commandLinePosition = Math.Min(commandLine.Length, commandLinePosition + 1);
                return;
            }



            if (validCharacters.ContainsKey(inp))
            {
                string character = validCharacters[inp];
                if (General.ActiveKeys[SDL_Keycode.SDLK_LSHIFT])
                {
                    if (specialUpperCharacters.ContainsKey(character))
                    {
                        commandLine = AddChar(specialUpperCharacters[character], commandLinePosition, commandLine);
                        commandLinePosition++;
                    }
                    else
                    {
                        commandLine = AddChar(character.ToUpperInvariant(), commandLinePosition, commandLine);
                        commandLinePosition++;
                    }
                }
                else
                {
                    commandLine = AddChar(character, commandLinePosition, commandLine);
                    commandLinePosition++;
                }
            }

            if (inp == SDL_Keycode.SDLK_BACKSPACE)
            {
                if (commandLine.Length > 0)
                {
                    commandLine = RemoveChar(commandLinePosition, commandLine);
                    commandLinePosition--;
                }
            }
            if (inp == SDL_Keycode.SDLK_DELETE)
            {
                if (commandLine.Length > 0)
                {
                    commandLine = RemoveChar(commandLinePosition, commandLine, true);
                }
            }


            if (inp == SDL_Keycode.SDLK_RETURN)
            {
                RunCommand(commandLine);
                commandLine = "";
                General.TextBarOpen = false;
                commandLinePosition = 0;
            }
        }


        public static string AddChar(string character, int position, string data)
        {
            if (position <= 0) { return data; }
            if (position > data.Length) 
            { 
                General.debugger.AddLog($"Issue with data, char: {character}, pos: {position}, data: {data}", Short_Tools.ShortDebugger.Priority.ERROR); 
                return data; 
            }
            if (data.Length == 0) { return data + character; }

            string start = data.Substring(0, position);
            string end = position >= data.Length ? "" : data.Substring(position, data.Length - position);
            return start + character + end;
        }

        public static string RemoveChar(int position, string data, bool delete = false)
        {
            if (data.Length == 0) { return ""; }
            if (position == 0) { return data; }


            string start;
            string end;

            if (!delete)
            {
                start = data.Substring(0, position - 1);
                end = position >= data.Length ? "" : data.Substring(position, data.Length - position);
            }
            else
            {
                start = data.Substring(0, position);
                end = position + 1 >= data.Length ? "" : data.Substring(position + 1, data.Length - position - 1);
            }

            return start + end;
        }




        private static void RunCommand(string commandLine)
        {
            if (commandLine == "/seed") { General.debugger.AddLog($"Seed: {General.world.seed}"); }

            if (commandLine == "/creative off") { General.settings.Cheats = false; }
            if (commandLine == "/creative on") { General.settings.Cheats = true; }
        }
    }
}
