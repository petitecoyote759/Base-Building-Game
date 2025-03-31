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
            #region Misc
            { "Short Studios Logo", "SSLogo.png" },


            { "Background", "Background\\Background.png" },
            #endregion Misc



            #region Tiles
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
            #endregion Tiles

            

            #region Tile Sprite Sheets
            { "GrassSS", "Tiles\\grass sprite sheet.bmp" },
            { "SandSS", "Tiles\\sand sprite sheet.bmp" },
            { "OceanSS", "Tiles\\ocean sprite sheet.bmp" },
            #endregion Tile Sprite Sheets



            #region Buildings
            { "Enemy Shader", "Buildings\\Enemy Shader.png" },




            { "BridgeNode0", "Buildings\\bridge node0.png" },


            { "WallNode0", "Buildings\\wall node0.png" },

            { "WallSegment0", "Buildings\\wall segment0.png" },


            { "Extractor0", "Buildings\\extractor.png" },


            { "DropPod0", "Buildings\\DropPod0.png" },


            { "SmallPort0", "Buildings\\port up.png" },


            { "MediumPort0", "Buildings\\Medium Port.png" },


            { "WorkCamp0", "Buildings\\workcamp.png" },

            
            { "Barrel0" , "Buildings\\barrel0.png" },


            { "Pipe0", "Buildings\\Pipe Node.png" },

            { "PipeSegment0", "Buildings\\Pipe Segment.png" },


            { "Path", "Buildings\\path.png" },
            #endregion Buildings



            #region Shadows
            { "Wall Node Shadow", "Shadows\\wall.png" },
            { "Wall Top Shadow", "Shadows\\wall segment top.png" },
            { "Wall Bottom Shadow", "Shadows\\wall segment bottom.png" },

            { "Night Filter", "Shadows\\blue.png" },
            #endregion Shadows



            #region Entities
            { "Player", "Entities\\Body.png" },
            { "PlayerLeg", "Entities\\Leg.png" },

            { "woodItem", "Entities\\woodItem.png" },
            { "StoneItem", "Entities\\rock.png" },
            { "IronItem", "Entities\\iron.png" },
            { "DiamondItem", "Entities\\diamond.png" },

            { "Man" , "Entities\\man.png" },

            { "EnemyUnit", "Entities\\Enemy Guardian.png" },
            #endregion Entities



            #region Boats
            { "Skiff0", "Entities\\Skiff0.png" },

            { "Destroyer2", "Entities\\Destroyer2.png" },
            #endregion Boats



            #region Turrets
            { "Turret2", "Entities\\Turret2.png" },
            #endregion Turrets



            #region Projectiles
            { "Projectile2", "Entities\\Projectile2.png" },
            #endregion Projectiles



            #region UI
            { "MouseBox", "UI\\box.png" },
            { "Hotbar", "UI\\hotbar.png" },
            { "SelectBox", "UI\\selectBox.png" },

            { "CollectBox", "UI\\collect box.png" }, // for men
            { "DeliverBox", "UI\\deliver box.png" },

            { "Port UI", "UI\\Port UI.png" },
            { "Interact", "UI\\Interact.png" },

            { "MenuButton", "UI\\MenuButton.png" },
            { "SelectedButton", "UI\\SelectedButton.png" },

            { "Loading Spinner", "UI\\Loading Spinner.png" },

            { "Text Bar", "UI\\text bar.png" }, 

            { "Map", "" },
            #endregion UI



            #region Debug
            { "Circle", "UI\\Circle.png" },
            #endregion Debug
        };
    }
}
