using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer 
        {
            public Dictionary<short, IntPtr> TileImages;
            public Dictionary<short, IntPtr[]> BuildingImages;
            public Dictionary<short, IntPtr> ItemImages;

            public Dictionary<short, IntPtr[]> BoatImages;
        }






        static void UpdateDictionaries()
        {
            renderer.TileImages = new Dictionary<short, IntPtr>()
            {
                { (short)TileID.Grass,     renderer.images["Grass"] },
                { (short)TileID.DeepOcean, renderer.images["Deep Ocean"] },
                { (short)TileID.Ocean,     renderer.images["Ocean"] },
                { (short)TileID.Sand,      renderer.images["Sand"] },
                { (short)TileID.Iron,      renderer.images["Iron"] },
                { (short)TileID.Oil,       renderer.images["Oil"] },
                { (short)TileID.Stone,     renderer.images["Stone"] },
                { (short)TileID.Wood,      renderer.images["Wood"] },
                { (short)TileID.Diamond,   renderer.images["Diamond"] },

                { (short)TileID.Error, renderer.images["Error"] }
            };





            renderer.BuildingImages = new Dictionary<short, IntPtr[]>()
            {
                { 
                    (short)BuildingID.Bridge, new IntPtr[] 
                    { 
                        renderer.images["BridgeNode0"],
                        renderer.images["BridgeNode0"],
                        renderer.images["BridgeNode0"],
                        renderer.images["BridgeNode0"],
                        renderer.images["BridgeNode0"],
                    } 
                },


                {
                    (short)BuildingID.Wall, new IntPtr[]
                    {
                        renderer.images["WallNode0"],
                        renderer.images["WallNode0"],
                        renderer.images["WallNode0"],
                        renderer.images["WallNode0"],
                        renderer.images["WallNode0"],
                    }
                },


                {
                    (short)BuildingID.Extractor, new IntPtr[]
                    {
                        renderer.images["Extractor0"],
                        renderer.images["Extractor0"],
                        renderer.images["Extractor0"],
                        renderer.images["Extractor0"],
                        renderer.images["Extractor0"],
                    }
                },


                 {
                    (short)BuildingID.DropPod, new IntPtr[]
                    {
                        renderer.images["DropPod0"],
                        renderer.images["DropPod0"],
                        renderer.images["DropPod0"],
                        renderer.images["DropPod0"],
                        renderer.images["DropPod0"],
                    }
                },


                 {
                    (short)BuildingID.SmallPort, new IntPtr[]
                    {
                        renderer.images["SmallPort0"],
                        renderer.images["SmallPort0"],
                        renderer.images["SmallPort0"],
                        renderer.images["SmallPort0"],
                        renderer.images["SmallPort0"],
                    }
                },


                 {
                    (short)BuildingID.MedPort, new IntPtr[]
                    {
                        renderer.images["MediumPort0"],
                        renderer.images["MediumPort0"],
                        renderer.images["MediumPort0"],
                        renderer.images["MediumPort0"],
                        renderer.images["MediumPort0"],
                    }
                },
            };






            renderer.ItemImages = new Dictionary<short, IntPtr>()
            {
                { (short)ItemID.Wood,    renderer.images["woodItem"] },
                { (short)ItemID.Stone,   renderer.images["StoneItem"] },
                { (short)ItemID.Iron,    renderer.images["IronItem"] },
                { (short)ItemID.Diamond, renderer.images["DiamondItem"] },
            };












            renderer.BoatImages = new Dictionary<short, IntPtr[]>()
            {
                {
                    (short)BuildingID.Bridge, new IntPtr[]
                    {
                        renderer.images["Skiff0"],
                        renderer.images["Skiff0"],
                        renderer.images["Skiff0"],
                        renderer.images["Skiff0"],
                        renderer.images["Skiff0"],
                    }
                },
            };
        }
    }
}