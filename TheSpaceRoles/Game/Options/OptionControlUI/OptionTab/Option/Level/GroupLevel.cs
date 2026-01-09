using AmongUs.QuickChat;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TSR.Assets;
using TSR.Patch;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace TSR.Game.Options.OptionControlUI.OptionTab.Option.Level
{
    // Group{}
    //   - Item1 [id,int]
    //   - Item2 [id,int]

    public class GroupLevel : OptionLevel
    {
        public Transform Parent => _bg.transform.parent;
        private Dictionary<string,OptionLevel> _innerItems = new();
        private readonly TextMeshPro _title;
        private readonly SpriteRenderer _bg;
        public Transform Inner => _inner.transform;
        private readonly GameObject _inner;
        private Vector2 _size;
        private Vector2 _innerSize;
        public Vector2 Position => _bg.transform.localPosition;
        private Color? _color = null;
        public static readonly Vector2 GroupSpace = new(0.05f, -0.3f);
        public const float GroupEndSpace = 0.05f;
        public void SetColor(Color color)=>_color = color;
        public GroupLevel(string id,Transform parent):base(id,parent)
        {
            _innerItems.Clear();
            _bg = new GameObject(SplitterLast(id)).AddComponent<SpriteRenderer>();
            _bg.transform.SetParent(parent, false);
            _bg.transform.localPosition = new Vector3(0,0,-0.02f);
            _bg.drawMode =  SpriteDrawMode.Sliced;
            _bg.sprite = Assets.AssetLoader.Sprites["option_bg"];
            _bg.size = _innerSize;
            _bg.gameObject.layer = Helper.UILayer;
            
            _title = new GameObject("Title").AddComponent<TextMeshPro>();
            _title.transform.SetParent(_bg.transform);
            _title.transform.localPosition = new Vector3(0.5f,-0.15f,-0.02f);
            _title.text = Translation.Get(Id);
            //_title.fontSizeMax = 
            _title.fontSize = GroupTitleCharacterSize;
            //_title.fontSizeMin = 0;
            _title.autoSizeTextContainer = true;
            _title.fontStyle = FontStyles.Bold;
            _title.enableAutoSizing =false;
            _title.color = _color ?? Helper.white;
            _title.gameObject.layer = Helper.UILayer;
            _title.alignment = TextAlignmentOptions.TopLeft;
            _title.color = Helper.black;
            _title.enableWordWrapping = false;
            
            _inner = new GameObject("Inner");
            _inner.transform.SetParent(_bg.transform);
            _inner.transform.localPosition = new Vector3(0,0,-0.1f);
            _inner.gameObject.layer = Helper.UILayer;
            //
            // Material material = new Material();
            // material.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Always);
            // material.SetInt("_StencilOp", (int)UnityEngine.Rendering.StencilOp.Replace);
            // material.SetInt("_Stencil",(int)OptionUIManager.MaskLayerL);
            //material.SetInt(PlayerMaterial.MaskLayer,(int)OptionUIManager.MaskLayerL);
            //_bg.material = material;
            //material.SetInt("_Stencil", (int)OptionUIManager.MaskLayerL);
            //material.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Equal);
            //material.SetInt("_StencilOp", (int)UnityEngine.Rendering.StencilOp.Keep);
            //material.renderQueue = 4000;
            //mask対応
            //_title.material = DestroyableSingleton<ChatBubble>.Instance.Background.material;
            // _title.fontMaterial = new(_title.fontSharedMaterial);
            // _title.fontMaterial.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Equal);
            // _title.fontMaterial.SetInt("_Stencil", (int)OptionUIManager.MaskLayerL);
            // //_title.fontMaterial.SetFloat(PlayerMaterial.MaskLayer,(int)OptionUIManager.MaskLayerL);
            // //_title.material.renderQueue = 4000;
            //
            // _bg.sortingOrder = -1000;
            // _bg.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            
            return;

            string SplitterLast(string textIncludingDot)
            {
                return textIncludingDot.Split('.')[^1];
            }
        }
        public void AddItem(OptionLevel item)
        {
            _innerItems.Add(item.Id, item);
        }

        public override float GetWidth() => _size.x;
        public override float GetHeight() => _size.y;



        /// <summary>
        /// 内部アイテムをUpdateする｡
        /// </summary>
        /// <param name="sizeX"></param>
        /// <returns>Height:高さを返します</returns>
        private float UpdateAndGetHeightOfItems(float sizeX)
        {
            float height = 0;
            var list = _innerItems.Values.ToList();
            for (int i =0;i<list.Count;i++)
            {
                list.ToList()[i].Update( new Vector2(0f, -height),sizeX - Mathf.Abs(GroupSpace.x) );
                height += list[i].GetHeight();
                Logger.Message($"Height:{height}");
            }
            return height;
        }
        //Groupは
        // - GroupSpace
        // - height
        // - GroupEndSpace
        // の順でお送りいたします｡
        /// <summary>
        /// 親の位置を入れること｡
        /// </summary>
        /// <param name="position"></param>
        /// <param name="sizeX"></param>
        public override void Update(Vector2 position, float sizeX)
        {
            float height = UpdateAndGetHeightOfItems(sizeX);
            _bg.size = _size = new Vector2(sizeX ,height + Mathf.Abs(GroupSpace.y)+GroupEndSpace);
            _bg.transform.localPosition = new Vector3(0,0,-0.1f) + (Vector3)GroupSpace + (Vector3)position;
        }

    }
    
}
