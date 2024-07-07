using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<string, string> images = new Dictionary<string, string>()
        {
            { "Short Studios Logo", "SSLogo.png" },


            { "Background", "Background\\Background.png" },

            // tiles
            { "Grass", "Tiles\\grass.png" },
            { "Diamond", "Tiles\\diamond.png" },
            { "Error", "Tiles\\Error.png" },
            { "Iron", "Tiles\\iron.png" },
            { "Deep Ocean", "Tiles\\ocean.png" },
            { "Oil", "Tiles\\Oil.png" },
            { "Sand", "Tiles\\sand.png" },
            { "Stone", "Tiles\\stone.png" },
            { "Ocean", "Tiles\\water.png" },
            { "Wood", "Tiles\\wood.png" },


            // fancy tiles
            { "GrassSS", "Tiles\\grass sprite sheet.bmp" },
            { "SandSS", "Tiles\\sand sprite sheet.bmp" },




            // buildings
            { "BridgeNode0", "Buildings\\bridge node0.png" },


            { "WallNode0", "Buildings\\wall node0.png" },

            { "WallSegment0", "Buildings\\wall segment0.png" },


            { "Extractor0", "Buildings\\extractor.png" },


            { "DropPod0", "Buildings\\DropPod0.png" },


            { "SmallPort0", "Buildings\\port up.png" },


            { "MediumPort0", "Buildings\\Medium Port.png" },


            {"WorkCamp0","Buildings\\workcamp.png"},

            //TODO: Fix temp image.
            {"Barrel0","Entities\\Turret2.png" },

            
            
            // entities
            { "Player", "Entities\\Body.png" },
            { "woodItem", "Entities\\woodItem.png" },
            { "StoneItem", "Entities\\rock.png" },
            { "IronItem", "Entities\\iron.png" },
            { "DiamondItem", "Entities\\diamond.png" },
            { "Man" , "Entities\\man.png" },



            // boats
            { "Skiff0", "Entities\\Skiff0.png" },

            { "Destroyer2", "Entities\\Destroyer2.png" },


            //  turrets
            { "Turret2", "Entities\\Turret2.png" },


            // bullets
            { "Projectile2", "Entities\\Projectile2.png" },





            //
            { "MouseBox", "UI\\box.png" },
            { "Hotbar", "UI\\hotbar.png" },
            { "SelectBox", "UI\\selectBox.png" },

            { "CollectBox", "UI\\collect box.png" }, // for men
            { "DeliverBox", "UI\\deliver box.png" },

            { "Port UI", "UI\\Port UI.png" },
            { "Interact", "UI\\Interact.png" },






            { "Map", ".png" }
        };
    }
}
