using UnityEngine;
using static PlayerMaterial;

namespace TSR.Game.Options.OptionControlUI.OptionTab.Option
{
    public abstract class OptionLevel
    {
        public OptionLevel(string id,Transform parent)
        {
            this.Id = id;
        }
        public string Id { init; get; }
        public const float StandardSpace = 0.2f;
        public const float MainCharacterSize = 2.5f;
        public const float GroupTitleCharacterSize  = 1.5f;
        public Color32 Color { get; protected set; }

        public abstract float GetHeight();

        public abstract float GetWidth();
        /// <summary>
        /// 中心位置とサイズ､
        /// </summary>
        /// <param name="position">あるべき中心位置</param>
        /// <param name="sizeX">サイズx</param>
        /// <param name="parentLayerID">親レイヤーId</param>
        public abstract void Update(Vector2 position,float sizeX);
    }
}
