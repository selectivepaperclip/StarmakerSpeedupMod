using UnityEngine;

namespace StarmakerSpeedupMod
{
    // just a collection of styles and textures used by the GUI
    internal static class ModGUIStyles
    {

        // Styles
        public static GUIStyle BackgroundStyle = new GUIStyle()
        {
            normal = { background = BackTexture, textColor = Color.white },
            fontSize = 18
        };

        public static GUIStyle TitleStyle = new GUIStyle()
        {
            normal = { textColor = Color.white },
            fontSize = 18,
            alignment = TextAnchor.MiddleCenter,
            stretchHeight = true,
            stretchWidth = true,
            margin = new RectOffset(left: ModsGUI.ModWindowPadding, right: ModsGUI.ModWindowPadding, top: ModsGUI.ModWindowPadding, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle LabelStyle = new GUIStyle()
        {
            normal = { background = BtnTexture, textColor = Color.white },
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            stretchHeight = true,
            stretchWidth = true,
            margin = new RectOffset(left: ModsGUI.ModWindowPadding, right: ModsGUI.ModWindowPadding, top: 0, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle HalfWidthContainer = new GUIStyle()
        {
            stretchWidth = false,
            fixedWidth = ModsGUI.ModWindowWidth / 2 + ModsGUI.ModWindowPadding / 2,
            margin = new RectOffset(left: -ModsGUI.ModWindowPadding, right: -ModsGUI.ModWindowPadding, top: 0, bottom: 0)
        };

        public static GUIStyle BtnStyle = new GUIStyle()
        {
            normal = { background = BtnTexture, textColor = Color.white },
            hover = { background = BtnHoverTexture, textColor = Color.white },
            active = { background = BtnPressTexture, textColor = Color.white },
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            fixedWidth = 60,
            stretchHeight = true,
            margin = new RectOffset(left: ModsGUI.ModWindowPadding, right: ModsGUI.ModWindowPadding, top: 0, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle WideBtnStyle = new GUIStyle(BtnStyle)
        {
            fixedWidth = (2 * BtnStyle.fixedWidth) + ModsGUI.ModWindowPadding
        };

        public static GUIStyle SliderStyle = new GUIStyle(GUI.skin.horizontalSlider)
        {
            fixedWidth = WideBtnStyle.fixedWidth,
            margin = new RectOffset(left: ModsGUI.ModWindowPadding, right: ModsGUI.ModWindowPadding, top: (int)(ModsGUI.ModWindowLineHeight / 2) - ModsGUI.ModWindowPadding, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle SliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);

        public static GUIStyle TextFieldLabelStyle = new GUIStyle(LabelStyle)
        {
            stretchWidth = false,
            padding = new RectOffset(left: ModsGUI.ModWindowPadding * 2, right: 0, top: 0, bottom: 0),
            margin = new RectOffset(left: ModsGUI.ModWindowPadding, right: 0, top: 0, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle TextFieldInputStyle = new GUIStyle(LabelStyle)
        {
            stretchWidth = false,
            fixedWidth = (2 * BtnStyle.fixedWidth),
            margin = new RectOffset(left: 0, right: 0, top: 0, bottom: ModsGUI.ModWindowPadding),
        };

        public static GUIStyle TextFieldClearBtn = new GUIStyle(BtnStyle)
        {
            stretchWidth = false,
            fixedWidth = BtnStyle.fixedWidth / 2f,
            padding = new RectOffset(left: ModsGUI.ModWindowPadding / 2, right: 0, top: 0, bottom: 0),
            margin = new RectOffset(left: 0, right: ModsGUI.ModWindowPadding, top: 0, bottom: ModsGUI.ModWindowPadding),
        };

        // Textures
        private static Texture2D _btnTexture;
        private static Texture2D _btnHoverTexture;
        private static Texture2D _btnPressTexture;
        private static Texture2D _backTexture;

        private static Texture2D BtnTexture
        {
            get
            {
                if (_btnTexture == null)
                    _btnTexture = GetTexture(new Color32(70, 70, 70, 255));
                return _btnTexture;
            }
        }

        private static Texture2D BtnHoverTexture
        {
            get
            {
                if (_btnHoverTexture == null)
                    _btnHoverTexture = GetTexture(new Color32(100, 100, 100, 255));
                return _btnHoverTexture;
            }
        }

        private static Texture2D BtnPressTexture
        {
            get
            {
                if (_btnPressTexture == null)
                    _btnPressTexture = GetTexture(new Color32(50, 50, 50, 255));
                return _btnPressTexture;
            }
        }

        private static Texture2D BackTexture
        {
            get
            {
                if (_backTexture == null)
                    _backTexture = GetTexture(new Color32(40, 40, 40, 245));
                return _backTexture;
            }
        }

        // Helper methods
        private static Texture2D GetTexture(Color32 color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

    }
}
