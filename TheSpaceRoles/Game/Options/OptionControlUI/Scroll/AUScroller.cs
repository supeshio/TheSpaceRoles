using AmongUs.QuickChat;
using System.Linq;
using TSR.Assets;
using TSR.Patch;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.UIR;
using Object = UnityEngine.Object;

namespace TSR.Game.Options.OptionControlUI.Scroll
{
    public class AUScroller
    {
        public GameObject ScrollGroup;
        public QuickChatMenuScroller QuickChatMenuScroller;
        public Scroller Scroller;
        public Scrollbar ScrollBar;
        public SpriteRenderer ScrollBG;
        public Transform Inner;
        public SpriteRenderer ScrollAreaBG;
        public BoxCollider2D HitBox;
        public const float Space = 0.05f;
        public const float ScrollerWidth = 0.05f;
        public const float ScrollerSpace = 0.05f;
        public static AUScroller CreateScroller(int maskLayer,
            Transform parent,
            string group,
            Vector3 scrollPosition,
            float scrollerSize,
            Vector2 scrollAreaSize)
        {
            
            var go = Helper.ClassLib<AmongUs.QuickChat.QuickChatMenuScroller>();//.transform.FindChild("HatsGroup");

            var sc = new AUScroller { ScrollGroup = Object.Instantiate(go.gameObject) };
            sc.ScrollGroup.name = group;

            sc.QuickChatMenuScroller = sc.ScrollGroup.GetComponent<QuickChatMenuScroller>();

            //Component.Destroy(sc.ScrollGroup.GetComponent<QuickChatMenuScroller>());
            sc.ScrollGroup.gameObject.SetActive(true);
            sc.ScrollGroup.transform.SetParent(parent);
            sc.ScrollGroup.transform.localPosition = new Vector3(scrollAreaSize.x+ScrollerSpace, -Space, -20)+ scrollPosition;
            sc.Scroller = sc.ScrollGroup.GetComponent<Scroller>();
            sc.ScrollBar = sc.ScrollGroup.transform.FindChild("ScrollBar_Handle").GetComponent<Scrollbar>();
            //sc.Scroller_.set

            sc.ScrollBG = sc.ScrollGroup.transform.FindChild("ScrollBar_Track").GetComponent<SpriteRenderer>();
            //sc.ScrollBG.transform.FindChild("");
            sc.QuickChatMenuScroller.SetSize(ScrollerWidth, scrollerSize - sc.ScrollBar.gameObject.GetComponent<BoxCollider2D>().size.y - Space*2);
            //sc.QuickChatMenuScroller.set
            //sc.scroller.ScrollbarYBounds = new FloatRange(-ScrollerSize, 0);
            //sc.scroller.SetYBoundsMin(-ScrollerSize);   
            sc.HitBox = sc.Scroller.transform.FindChild("Collider").GetComponent<BoxCollider2D>();
            //sc.ScrollBG.size = new Vector2(0.05f, ScrollerSize);
            //var ScrollArea = new GameObject("BGArea");
            //ScrollArea.transform.SetParent(sc.scroller.transform, false);

            sc.ScrollAreaBG = sc.Scroller.transform.FindChild("BGSprite") == null ? new GameObject("BGSprite").AddComponent<SpriteRenderer>() : sc.Scroller.transform.FindChild("BGSprite").GetComponent<SpriteRenderer>();
            
            Vector2 scrollTopLeft= new Vector2(-scrollAreaSize.x - ScrollerSpace,+Space);
            Vector2 listPos = new Vector2(-scrollAreaSize.x-ScrollerSpace+Space,0);
            //Vector2 hitBoxPos = new Vector2(ScrollAreaSize.x/2 , -ScrollAreaSize.y/2);
            sc.ScrollAreaBG.transform.SetParent(sc.Scroller.transform, false);
            sc.ScrollAreaBG.transform.localPosition = scrollTopLeft;
            sc.ScrollAreaBG.sprite = Assets.AssetLoader.Sprites["option_bg"];
            sc.ScrollAreaBG.tileMode = SpriteTileMode.Adaptive;
            sc.ScrollAreaBG.drawMode = SpriteDrawMode.Sliced;
            sc.ScrollAreaBG.size = scrollAreaSize;
            sc.ScrollAreaBG.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
            sc.ScrollAreaBG.gameObject.layer = 5;//UILayer
            sc.HitBox.transform.localScale = scrollAreaSize + new Vector2(ScrollerWidth*2+ ScrollerSpace*2,0);
            sc.HitBox.size = Vector3.one;
            sc.HitBox.transform.localPosition = scrollTopLeft+ new Vector2(sc.HitBox.transform.localScale.x,-sc.HitBox.transform.localScale.y)/2;
            
            var rect = new GameObject("Mask").AddComponent<RectMask2D>();
            var rt = rect.GetComponent<RectTransform>();
            rt.SetParent(sc.ScrollGroup.transform);
            rt.transform.transform.localPosition = Vector3.zero;
            rt.pivot = new Vector2(0, 0);
            rt.sizeDelta = scrollAreaSize;
            
            sc.Inner = sc.Scroller.Inner;
            sc.Scroller.Inner.transform.SetParent(rt.transform);
            sc.Inner.transform.localPosition = listPos;
            
            Object.Destroy(sc.Inner.transform.GetChild(0).gameObject);
            //mask付け
            //Logger.Info(AssetLoader.Materials["cropped"] ==null ? "null" : "not null");
            //Logger.Info(AssetLoader.Materials["cropper"] ==null ? "null" : "not null");
            //Logger.Info(AssetLoader.Shaders["UI/D"] ==null ? "null" : "not null");
            //Logger.Info(AssetLoader.Sprites["option_bg"] ==null ? "null" : "not null");
            //
            // var mat  = sc.ScrollAreaBG.material = new Material(DestroyableSingleton<ChatBubble>.Instance.Background.material);
            // mat.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.Always);
            // mat.SetInt("_StencilOp", (int)UnityEngine.Rendering.StencilOp.Replace);
            // mat.SetInt("_Stencil", maskLayer);
            // //mat.SetInt(PlayerMaterial.MaskLayer,maskLayer);
            // mat.renderQueue = 2000;
            // Object.Destroy(sc.Inner.transform.GetChild(0).gameObject);
            
            var top = sc.Scroller.transform.FindChild("TopGradient");
            var bottom = sc.Scroller.transform.FindChild("BottomGradient");
            top.gameObject.SetActive(false);
            bottom.gameObject.SetActive(false);
            //sc.scroller.Colliders = new[] { sc.scroller.transform.FindChild("HitBox").GetComponent<BoxCollider2D>() };
            //sc.scroller.Inner.DestroyChildren();
            //for(int i = 0;i <sc.scroller.Inner.childCount ; i++ )
            //{
            //    GameObject.Destroy(sc.scroller.Inner.GetChild(0).gameObject);
            //}
            //sc.scroller = new GameObject("Scroller").AddComponent<Scroller>();
            //sc.scroller.transform.SetParent(sc.ScrollGroup.transform, false);

            //var ScrollTo = sc.scroller.gameObject.AddComponent<ScrollToSelection>();
            //ScrollTo.scrollRect = sc.scroller;

            //sc.ScrollBar = new GameObject("scrollBar").AddComponent<Scrollbar>();
            //var rend = sc.ScrollBar.gameObject.AddComponent<SpriteRenderer>();
            //rend.transform.SetParent(sc.ScrollGroup.transform, false);
            //rend.sprite = AssetLoader.Sprites["option_bg"];
            //rend.drawMode = SpriteDrawMode.Tiled;
            //rend.size = new Vector2(0.5f, 0.5f);
            //rend.gameObject.layer = 5;//UILayer
            //var k = rend.gameObject.AddComponent<BoxCollider2D>();
            //k.size = new Vector2(0.5f, 0.5f);

            //sc.ScrollBG = new GameObject("UI_ScrollBG").AddComponent<SpriteRenderer>();
            //sc.ScrollBG.transform.SetParent(sc.ScrollGroup.transform, false);
            //sc.ScrollBG.sprite = AssetLoader.Sprites["option_bg"];
            //sc.ScrollBG.size = new Vector2(0.5f, 0.5f);
            //sc.ScrollBG.gameObject.layer = 5;//UILayer

            //sc.HitBox = new GameObject("HitBox").AddComponent<BoxCollider2D>();
            //sc.HitBox.transform.SetParent(sc.scroller.transform, false);
            //sc.HitBox.size = new Vector2();

            //sc.Inner = new GameObject("Inner").transform;
            //sc.Inner.transform.SetParent(sc.ScrollGroup.transform, false);
            //sc.Inner.gameObject.layer = 5;

            //sc.scroller.allowY = true;
            //sc.scroller.allowX = false;
            //sc.scroller.ClickMask = sc.HitBox;
            //sc.scroller.ContentYBounds = new FloatRange(0f, -5f);
            //sc.scroller.ScrollbarY = sc.ScrollBar;
            //sc.scroller.MouseMustBeOverToScroll = true;
            //sc.scroller.Hitbox

            return sc;
        }
    }
}