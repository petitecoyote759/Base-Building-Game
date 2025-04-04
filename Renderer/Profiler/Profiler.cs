using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public class Profiler
    {
        internal int maxFrames = 240;

        internal List<Frame> frames = new();
        internal Frame? currentFrame;
        internal Dictionary<string, SDL.SDL_Color> colors = new();

        internal List<SDL.SDL_Color> available_colors = new()
        {
            new() { r = 255, g = 50, b = 50, a = 150 },
            new() { r = 50, g = 50, b = 255, a = 150 },
            new() { r = 50, g = 255, b = 50, a = 150 }
        };

        internal Profiler()
        {

        }

        public Frame? StartFrame()
        {
            if (currentFrame is not null)
            {
                Frame frame = currentFrame;
                frame.frameEnd = DateTimeOffset.UtcNow.UtcTicks;
            }

            currentFrame = new()
            {
                frameStart = DateTimeOffset.UtcNow.UtcTicks,
                profiles = new()
            };

            return currentFrame;
        }

        public Frame? EndFrame()
        {
            if (currentFrame is not null)
            {
                Frame thisFrame = (Frame)currentFrame;
                currentFrame = null;

                thisFrame.frameEnd = DateTimeOffset.UtcNow.UtcTicks;

                frames.Add(thisFrame);
                while (frames.Count > maxFrames)
                {
                    frames.RemoveAt(0);
                }

                return thisFrame;
            }
            else { return null; }
        }

        public Profile? StartProfile(string name, [Optional]SDL.SDL_Color? color)
        {
            if (currentFrame is null) { return null; }

            if (!colors.ContainsKey(name)) colors.Add(name, color ?? available_colors[colors.Count % available_colors.Count]);

            Frame thisFrame = currentFrame;

            if (thisFrame.profiles.ContainsKey(name)) return null;

            Profile thisProfile = new()
            {
                name = name,
                start = DateTimeOffset.UtcNow.UtcTicks
            };
            thisFrame.profiles.Add(name, thisProfile);

            return thisProfile;
        }

        public Profile? EndProfile(string name)
        {
            if (currentFrame is null) { return null; }
            Frame thisFrame = currentFrame;

            if (thisFrame.profiles.TryGetValue(name, out Profile thisProfile))
            {
                thisProfile.end = DateTimeOffset.UtcNow.UtcTicks;
                return thisProfile;
            }
            else { return null; }
        }
    }

    public class Frame
    {
        internal double frameStart;
        internal double frameEnd;
        internal Dictionary<string, Profile> profiles;
    }

    public class Profile
    {
        internal string name;
        internal double start;
        internal double end;
    }

    public static partial class General
    {
        public static readonly Profiler profiler = new();
    }
}
