using AmongUs.QuickChat;
using System;
using System.Collections.Generic;
using TMPro;
using TSR.Patch;
using UnityEngine;
using UnityEngine.Events;
using static PlayerMaterial;
using Object = UnityEngine.Object;

namespace TSR.Game.Options.OptionControlUI.OptionTab.Option.Level
{
    public class StandardLevel : OptionLevel
    {
        public TextMeshPro Title;//text mesh pro
        private SpriteRenderer _bg;
        private PassiveButton _left;//sprite renderer
        private PassiveButton _right;//sprite renderer
        public string Name;
        //Button - Text
        //       - ( + value -)
        //Ex : [ Sheriff - 2 + ]
        //public PassiveButton Button;
        public TextMeshPro Text;
        private Vector2 _size = new Vector2(0,0.4f);
        private Color _color;
        public void SetColor(Color color) => _color = color;
        CustomOptionAttribute OptionAttribute { get; init; }
        public StandardLevel(string id,Transform parent):base(id,parent)
        {
            OptionAttribute = CustomOption.Get(id);
            if (OptionAttribute == null)
            {
                throw new KeyNotFoundException($"Option ID {id} does not exist");
                return;
            }
            
            //bg:Create
            _bg = new GameObject(SplitterLast(id)).AddComponent<SpriteRenderer>();
            _bg.transform.SetParent(parent, false);
            _bg.transform.localPosition = Vector3.zero;
            _bg.sprite = Assets.AssetLoader.Sprites["option_bg"]; //サイズはまだ決めれないのでソレ以外をやります
            _bg.drawMode = SpriteDrawMode.Sliced;
            _bg.color = _color;
            _bg.size = new Vector2(0.5f,0.5f);
            _bg.gameObject.layer = Helper.UILayer;
            _bg.color = Helper.ColorFromColorcode("#05c0c5");
            
            //left:Create
            _left = new GameObject("Left").AddComponent<PassiveButton>();
            _left.transform.SetParent(_bg.transform, false);
            var leftSprite = _left.gameObject.AddComponent<SpriteRenderer>();
            leftSprite.sprite = Assets.AssetLoader.Sprites["double_left"];
            leftSprite.color = Helper.white;
            _left.gameObject.layer = Helper.UILayer;
            _left.transform.SetParent(leftSprite.transform, false);
            _left.ClickSound = Assets.AssetLoader.AudioClips["mouseclick1"];
            _left.OnClick.AddListener((UnityAction)(() =>
            {
                SelectionUpdate(Selection-1);
            }));
            _left.transform.localScale = Vector3.one;
            
            //right:Create
            _right = new GameObject("Right").AddComponent<PassiveButton>();
            _right.transform.SetParent(_bg.transform, false);
            var rightSprite = _right.gameObject.AddComponent<SpriteRenderer>();
            rightSprite.sprite = Assets.AssetLoader.Sprites["double_right"];
            rightSprite.color = Helper.white;
            _right.gameObject.layer = Helper.UILayer;
            _right.transform.SetParent(rightSprite.transform, false);
            _right.ClickSound = Assets.AssetLoader.AudioClips["mouseclick1"];
            _right.OnClick.AddListener((UnityAction)(() =>
            {
                SelectionUpdate(Selection+1);
            }));
            _right.transform.localScale = Vector3.one;
            
            //
            //Material material = UnityEngine.Object.Instantiate(Helper.ClassLib<PlayerVoteArea>().Background.material);
            // material.SetInt("_Stencil", (int)OptionUIManager.MaskLayerL);
            // material.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Equal);
            // material.SetInt("_StencilOp", (int)UnityEngine.Rendering.StencilOp.Replace);
            
            //material.SetInt(PlayerMaterial.MaskLayer,(int)OptionUIManager.MaskLayerL);
            // material.renderQueue = 4000;
            //mask対応
            //var mat = _bg.material = leftSprite.material =rightSprite.material = material;
            // _bg.sortingOrder = -1000;
            // leftSprite.sortingOrder = -1000;
            // rightSprite.sortingOrder = -1000;
            // _bg.maskInteraction =  SpriteMaskInteraction.VisibleInsideMask;
            // leftSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            // rightSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            
            
            return;

            string SplitterLast(string textIncludingDot)
            {
                return textIncludingDot.Split('.')[^1];
            }
        }

        private int Selection;
        private void SelectionUpdate(int i)
        {
            int count = OptionAttribute.GetOptions().Length;
            Selection = (i+ count )% OptionAttribute.GetOptions().Length;
        }
        public override float GetHeight()
        {
            return _size.y;
        }
        public override float GetWidth()
        {
            return _size.x; 
        }

        public override void Update(Vector2 position, float sizeX)
        {
            _bg.transform.localPosition = new Vector3(0,0,-0.01f) + (Vector3)position+ (Vector3)GroupLevel.GroupSpace;
            _bg.size = _size = new Vector2(sizeX -Mathf.Abs(GroupLevel.GroupSpace.x) , _size.y);
            _left.transform.localPosition = new Vector3( sizeX - 0.6f,-0.2f, -0.02f);
            _right.transform.localPosition = new Vector3( sizeX - 0.2f, -0.2f,-0.02f);
        }

    }
} 