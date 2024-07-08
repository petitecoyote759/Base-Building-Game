using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        /// <summary>
        /// Interface for image and position animations used in the ShortRenderer.
        /// </summary>
        public interface Animation
        {
            /// <summary>
            /// Bool used to see if the animation is finished.
            /// </summary>
            public bool Finished { get; set; }
            /// <summary>
            /// Function used to progress the animation.
            /// </summary>
            /// <param name="dt"> The time between calls (usually Seconds Per Frame) in milliseconds. </param>
            public void Tick(long dt);

            public object ObjGetter { get; }
        }

    }
}
