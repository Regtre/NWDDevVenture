using NWDEditor;
using NWDFoundation.Tools;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Windows;
using NWDUnityShared.Tools;
using System;
using UnityEditor;
using UnityEngine;
using NWDToolbox = NWDUnityShared.Tools.NWDToolbox;

namespace NWDUnityEditor.Tools
{

    public class NWDGUI
    {

        private static bool ProSkinEditor = true;

        public static float kFieldMarge = 5.0f;
        //public static float kTopAdjustMarge = 2.0f;
        public static float kFieldIndent = 15.0f;
        public static float kScrollbar = 13f;
        public static GUIStyle kScrollviewFullWidth;
        public static float KTAB_BAR_HEIGHT = 40.0F;
        public static float KTAB_TOOLBAR_HEIGHT = NWDGUI.KTAB_BAR_HEIGHT - NWDGUI.kFieldMarge * 2;
        // window top tabbar 
        public static Color KTAB_BAR_BACK_COLOR;
        public static Color KTAB_BAR_LINE_COLOR;
        public static Color KTAB_BAR_HIGHLIGHT_COLOR;

        public static Texture2D kImageUp = NWDFindPackage.EditorTexture("NWDInterfaceUp");
        public static Texture2D kImageDown = NWDFindPackage.EditorTexture("NWDInterfaceDown");
        public static Texture2D kImageLeft = NWDFindPackage.EditorTexture("NWDInterfaceLeft");
        public static Texture2D kImageRight = NWDFindPackage.EditorTexture("NWDInterfaceRight");
        public static Texture2D kImageBezierTexture = NWDFindPackage.EditorTexture("NWDBezierTexture");

        public static Texture2D kImageSyncGeneralForward = NWDFindPackage.EditorTexture("NWDSyncGeneralForward");

        public static Texture2D kImageSyncGeneralSuccessed = NWDFindPackage.EditorTexture("NWDSyncGeneralSuccessed");
        public static Texture2D kImageSyncGeneralWaiting = NWDFindPackage.EditorTexture("NWDSyncGeneralWaiting");
        public static Texture2D kImageSyncDanger = NWDFindPackage.EditorTexture("NWDSyncDanger");
        public static Texture2D kImageSyncForward = NWDFindPackage.EditorTexture("NWDSyncForward");
        public static Texture2D kImageSyncProceed = NWDFindPackage.EditorTexture("NWDProceed");
        public static Texture2D kImageSyncWaiting = NWDFindPackage.EditorTexture("NWDSyncWaiting");
        public static Texture2D kImageWaiting = NWDFindPackage.EditorTexture("NWDWaiting");
        public static Texture2D kImageDiskUnknow = NWDFindPackage.EditorTexture("NWDDiskUnknow");
        public static Texture2D kImageDiskUpdate = NWDFindPackage.EditorTexture("NWDDiskUpdate");
        public static Texture2D kImageDiskInsert = NWDFindPackage.EditorTexture("NWDDiskInsert");
        public static Texture2D kImageDiskDelete = NWDFindPackage.EditorTexture("NWDDiskDelete");
        public static Texture2D kImageDiskDatabase = NWDFindPackage.EditorTexture("NWDDiskDatabase");

        public static Texture2D kImageCheckValid = NWDFindPackage.EditorTexture("NWDCheckValid");
        public static Texture2D kImageCheckWorkInProgress = NWDFindPackage.EditorTexture("NWDCheckWorkInProgress");
        public static Texture2D kImageCheckWarning = NWDFindPackage.EditorTexture("NWDCheckWarning");


        public static Texture2D kImageSyncForbidden = NWDFindPackage.EditorTexture("NWDSyncForbidden");
        public static Texture2D kImageSyncGeneralForbidden = NWDFindPackage.EditorTexture("NWDSyncGeneralForbidden");
        public static Texture2D NWDSyncGeneralRequired = NWDFindPackage.EditorTexture("NWDSyncGeneralRequired");
        public static Texture2D kImageRed = NWDFindPackage.EditorTexture("NWDRed");
        public static Texture2D kImageGreen = NWDFindPackage.EditorTexture("NWDGreen");
        public static Texture2D kImageSyncRequired = NWDFindPackage.EditorTexture("NWDSyncRequired");
        public static Texture2D kImageSyncSuccessed = NWDFindPackage.EditorTexture("NWDSyncSuccessed");
        public static Texture2D kNetWorkedDataLogo = NWDFindPackage.EditorTexture("NWD3WindowLogo");
        public static Texture2D kImageDefaultIcon = NWDFindPackage.EditorTexture("NWDExample");
        public static GUIContent kNetWorkedDataLogoContent;

        // change color of background interface element
        static Color kOldColor;
        static bool kOldColorInit;
        public static Color kRedElementColor; // invert color from white to fusion over button
        public static Color kGreenElementColor; // invert color from white to fusion over button
        public static Color kYellowElementColor; // invert color from white to fusion over button
        public static Color kBlueElementColor; // invert color from white to fusion over button
        public static Color kOrangeElementColor; // invert color from white to fusion over button
        public static Color kGrayElementColor; // invert color from white to fusion over button
        public static float WarningMinHeight = 48.0F;
        public static float ErrorMinHeight = 48.0F;

        public static GUIStyle kSeparatorStyle;
        static Texture2D kSeparatorTexture;
        static Color kSeparatorColor;
        public static float kSeparatorHeight = 1.0F;

        public static GUIStyle kLineStyle;
        public static GUIStyle kLineWhiteStyle;
        static Texture2D kLineTexture;
        static Texture2D kLineWhiteTexture;
        static Color kLineColor;
        static Color kLineWhiteColor;
        public static float kLineHeight = 1.0F;

        public static GUIStyle kTitleStyle;
        static Texture2D kTitleTexture;
        static Color kTitleColor;

        public static GUIStyle kSectionStyle;
        static Texture2D kSectionTexture;
        static Color kSectionColor;

        public static GUIStyle kSubSectionStyle;
        static Texture2D kSubSectionTexture;
        public static Color kSubSectionColor;

        public static float KTableSearchLabelWidth = 80.0F;
        public static float KTableSearchWidth = 120.0F;
        public static float KTableSearchFieldWidth = 200.0F;
        public static float KTableReferenceWidth = 160.0F;
        public static float KTableRowWebModelWidth = 70.0F;
        public static float KTablePageMarge = 5.0F;
        public static float KTableMinWidth = (KTableReferenceWidth + kFieldMarge) * 6.0F;
        public static Color kRowColorSelected;
        public static Color kRowColorSelectedDark;
        public static Color kRowColorError;
        public static Color kRowColorWarning;
        public static Color kRowColorTrash;
        public static Color kRowColorDisactive;
        public static GUIStyle KTableToolbar;
        public static GUIStyle KTableSearchTitle;
        public static GUIStyle KTableSearchMask;
        public static GUIStyle KTableSearchButton;
        public static GUIStyle KTableSearchLabel;
        public static GUIStyle KTableSearchToggle;
        public static GUIStyle KTableSearchEnum;
        public static GUIStyle KTableSearchDescription;
        public static GUIStyle KTableSearchClassIcon;
        public static GUIStyle KTableSearchIcon;
        public static GUIStyle KTableSearchTextfield;
        public static GUIStyle KTableClassToolbar;
        public static GUIStyle KTableClassPopup;
        public static float kTablePrefabWidth = 40.0F;
        public static float kTableSelectWidth = 20.0f;
        //public static float kTableClassIconWidth = 20.0f;
        public static float kTableIDWidth = 45.0f;
        public static float kTableIconWidth = 44.0f;
        public static float kTableHeaderHeight = 30.0f;
        public static Color kTableHeaderColor;
        public static GUIStyle KTableHeaderSelect;
        public static GUIStyle KTableHeaderId;
        public static GUIStyle KTableHeaderPrefab;
        public static GUIStyle KTableHeaderInformations;
        public static GUIStyle KTableHeaderIcon;
        public static GUIStyle KTableHeaderStatut;
        public static GUIStyle KTableHeaderReference;
        public static float kTableRowHeight = 40.0f;
        public static Color kRowColorLineWhite;
        public static Color kRowColorLineBlack;
        public static GUIStyle KTableRowSelect;
        public static GUIStyle KTableRowId;
        public static GUIStyle KTableRowPrefab;
        public static GUIStyle KTableRowInformations;
        public static GUIStyle KTableRowIcon;
        public static GUIStyle KTableRowStatut;
        public static GUIStyle KTableRowReference;

        public static GUIStyle KTableRowSelected;
        public static GUIStyle KTableRowNotSelected;


        // Datas Selector
        public static GUIStyle kDataSelectorPropertieStyle;
        public static GUIStyle kDataSelectorFieldStyle;
        public static GUIStyle kDataSelectorFieldIconStyle;
        public static GUIStyle kDataSelectorTileStyle;
        public static GUIStyle kDataSelectorRowStyle;
        public static float kDatasSelectorYOffset;


        // Data inspector properties
        public static Color kIdentityColor; // inspector identity color area
        public static Color kPropertyColor; // inspector identity color area

        public static float kIconClassWidth = 48.0f;

        public static int kLongString = 5;
        public static int kVeryLongString = 15;
        public static float kPrefabSize = 80.0f;
        public static float kIntWidth = 36.0f;
        public static float kConditionalWidth = 42.0f;
        public static float kEditWidth = 36.0f;
        public static float kLangWidth = 50.0f;
        public static float kEnumWidth = 70.0f;
        public static float kConnectionIndent = 10.0f;
        public static float kUpDownWidth = 18.0f;

        public static GUIStyle kPropertyEntitlementStyle;

        public static GUIStyle kInspectorInternalTitle;
        public static GUIStyle kInspectorReferenceCenter;
        public static GUIStyle kInspectorNoData;


        public static GUIStyle kLabelStyle;
        public static GUIStyle kFooterLabelStyle;
        public static GUIStyle kBoldLabelStyle;
        public static GUIStyle kItalicLabelStyle;
        public static GUIStyle kNoConfigStyle;

        public static GUIStyle kHelpBoxStyle;
        public static GUIStyle kMiniButtonStyle;
        public static GUIStyle kObjectFieldStyle;
        public static GUIStyle kTextFieldStyle;
        public static GUIStyle kFloatFieldStyle;
        public static GUIStyle kIntFieldStyle;
        public static GUIStyle kDoubleFieldStyle;
        public static GUIStyle kLongFieldStyle;
        public static GUIStyle kFoldoutStyle;
        public static GUIStyle kBoldFoldoutStyle;
        public static GUIStyle kColorFieldStyle;
        public static GUIStyle kPopupStyle;
        public static GUIStyle kEnumStyle;
        public static GUIStyle kToggleStyle;

        public static GUIStyle kTextAreaStyle;
        public static GUIStyle kRedLabelStyle;
        public static GUIStyle kGrayLabelStyle;
        public static GUIStyle kLabelRightStyle;
        public static GUIStyle kMiniLabelStyle;

        public static GUIStyle kIconButtonStyle;
        public static GUIStyle kEditButtonStyle;

        public static GUIStyle kIconCenterStyle;
        public static GUIStyle kIconLeftStyle;
        public static GUIStyle kIconLock;

        public static GUIStyle kCredentialTagRightStyle;


        public static GUIContent kNodeContentIcon;

        public static GUIContent kEditContentIcon;
        public static GUIContent kNewContentIcon;
        public static GUIContent kCleanContentIcon;
        public static GUIContent kUpContentIcon;
        public static GUIContent kDownContentIcon;
        public static GUIContent kLeftContentIcon;
        public static GUIContent kRightContentIcon;


        // Nodal Document
        // TODO : all rename!! with right name!

        public static float kNodeCardWidth = 300.0F;
        public static float kNodeCardHeight = 500.0F;
        //public static float kNodeCardMarging = 50.0F;

        public static float kNodeCanvasFraction = 20;
        public static Color kNodeCanvasMajor;
        public static Color kNodeCanvasMinor;
        public static Color kNodeCanvasMargeBlack;

        public static Color kNodeLineColor;
        public static Color kNodeOverLineColor;
        public static float kIconWidth = 36.0f;
        public static float kEditIconSide = 16.0f;
        public static float kEditWidthHalf = 8.0f;
        public static float kEditWidthMini = 12.0f;
        public static float kEditWidthMiniHalf = 6.0f;

        static private bool StyleLoaded = false;

        static NWDGUI()
        {
            StyleLoaded = false;
        }

        public static float AreaHeight(float tFieldHeigt, int tFieldNumber, bool sAlreadyMarged = true)
        {
            if (sAlreadyMarged == true)
            {
                return tFieldNumber * tFieldHeigt + (tFieldNumber - 1) * kFieldMarge;
            }
            else
            {
                return tFieldNumber * tFieldHeigt + (tFieldNumber + 1) * kFieldMarge;
            }
        }

        public static Rect AssemblyArea(Rect sA, Rect sB)
        {
            float tXo = Math.Min(sA.x, sB.x);
            float tXf = Math.Max(sA.x + sA.width, sB.x + sB.width);
            float tYo = Math.Min(sA.y, sB.y);
            float tYf = Math.Max(sA.y + sA.height, sB.y + sB.height);
            return new Rect(tXo, tYo, tXf - tXo, tYf - tYo);
        }

        public static Rect[] DiviseArea(Rect sRect, int sX, bool sAlreadyMarged = true)
        {
            Rect[] rReturn = new Rect[sX];
            if (sX < 1)
            {
                sX = 1;
            }
            if (sAlreadyMarged == true)
            {
                float tW = sRect.width;
                float tWW = (tW - (sX - 1) * kFieldMarge) / sX;
                float tWL = (tW + kFieldMarge) / sX;
                for (int tI = 0; tI < sX; tI++)
                {
                    rReturn[tI] = new Rect(sRect.x + tI * tWL, sRect.y, tWW, sRect.height);
                }
            }
            else
            {
                float tW = sRect.width;
                float tWW = (tW - (sX + 1) * kFieldMarge) / sX;
                float tWL = (tW - kFieldMarge) / sX;
                for (int tI = 0; tI < sX; tI++)
                {
                    rReturn[tI] = new Rect(sRect.x + kFieldMarge + tI * tWL, sRect.y, tWW, sRect.height);
                }
            }
            return rReturn;
        }

        public static Rect[,] DiviseArea(Rect sRect, int sX, int sY, bool sAlreadyMarged = true)
        {
            Rect[,] rReturn = new Rect[sX, sY];
            if (sX < 1)
            {
                sX = 1;
            }
            if (sY < 1)
            {
                sY = 1;
            }
            if (sAlreadyMarged == true)
            {
                float tW = sRect.width;
                float tWW = (tW - (sX - 1) * kFieldMarge) / sX;
                float tWL = (tW + kFieldMarge) / sX;

                float tH = sRect.height;
                float tHH = (tH - (sY - 1) * kFieldMarge) / sY;
                float tHL = (tH + kFieldMarge) / sY;

                for (int tI = 0; tI < sX; tI++)
                {
                    for (int tJ = 0; tJ < sY; tJ++)
                    {
                        rReturn[tI, tJ] = new Rect(sRect.x + tI * tWL, sRect.y + tJ * tHL, tWW, tHH);
                    }
                }
            }
            else
            {
                float tW = sRect.width;
                float tWW = (tW - (sX + 1) * kFieldMarge) / sX;
                float tWL = (tW - kFieldMarge) / sX;

                float tH = sRect.height;
                float tHH = (tH - (sY + 1) * kFieldMarge) / sY;
                float tHL = (tH - kFieldMarge) / sY;

                for (int tI = 0; tI < sX; tI++)
                {
                    for (int tJ = 0; tJ < sY; tJ++)
                    {
                        rReturn[tI, tJ] = new Rect(sRect.x + kFieldMarge + tI * tWL, sRect.y + kFieldMarge + tJ * tHL, tWW, tHH);
                    }
                }
            }
            return rReturn;
        }

        public static void LoadStylesReforce()
        {
            StyleLoaded = false;
        }

        public static void LoadColor()
        {

            //Color tBaseColor = NWDAppConfiguration.SharedInstance().TintColor;
            Color tBaseColor = NWDToolbox.Color255(25, 20, 34, 255);

            //Color tBaseColor = NWDToolbox.Color255(25, 20, 34, 255);
            Color tRedColor = NWDToolbox.ColorPercent(tBaseColor, 4F, 2F, 4F);

            //Color tRedColor = NWDToolbox.Color255(102, 36, 131, 255);

            //Debug.Log("LoadStyles()");
            StyleLoaded = true;
            kNetWorkedDataLogoContent = new GUIContent(kNetWorkedDataLogo);

            // color force 

            KTAB_BAR_BACK_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
            KTAB_BAR_LINE_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
            KTAB_BAR_HIGHLIGHT_COLOR = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            kRedElementColor = new Color(0.9F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button

            kGreenElementColor = new Color(0.7F, 0.9F, 0.7F, 1.0F); // invert color from white to fusion over button
            kYellowElementColor = new Color(0.1F, 0.9F, 0.9F, 1.0F); // invert color from white to fusion over button
            kBlueElementColor = new Color(0.7F, 0.7F, 0.9F, 1.0F); // invert color from white to fusion over button
            kOrangeElementColor = new Color(0.9F, 0.8F, 0.7F, 1.0F); // invert color from white to fusion over button
            kGrayElementColor = new Color(0.7F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button

            kSeparatorColor = new Color(0, 0, 0, 0.5F);

            kLineColor = new Color(0, 0, 0, 0.5F);
            kLineWhiteColor = new Color(1, 1, 1, 0.2F);

            kTitleColor = new Color(0, 0, 0, 0.3F);
            //kTitleColor = new Color(102F / 255F, 36F / 255F, 131F / 255F, 0.3F);

            kSectionColor = new Color(0, 0, 0, 0.2F);
            //kSectionColor = new Color(102F / 255F, 36F / 255F, 131F / 255F, 0.2F);

            kSubSectionColor = new Color(0, 0, 0, 0.1F);
            //kSubSectionColor = new Color(102F / 255F, 36F / 255F, 131F / 255F, 0.1F);


            kRowColorSelected = new Color(0.55f, 0.55f, 1.00f, 0.25f);
            kRowColorSelectedDark = new Color(0.55f, 0.55f, 1.00f, 0.75f);


            kRowColorError = new Color(1.00f, 0.00f, 0.00f, 0.55f);
            kRowColorWarning = new Color(1.00f, 0.50f, 0.00f, 0.55f);
            kRowColorTrash = new Color(0.00f, 0.00f, 0.00f, 0.45f);
            kRowColorDisactive = new Color(0.00f, 0.00f, 0.00f, 0.35f);


            kTableHeaderColor = new Color(0.0f, 0.0f, 0.0f, 0.35f);

            kRowColorLineWhite = new Color(1.0f, 1.0f, 1.0f, 0.25f);
            kRowColorLineBlack = new Color(0.0f, 0.0f, 0.0f, 0.25f);

            kNodeCanvasMajor = new Color(1.0F, 1.0F, 1.0F, 0.20F);
            kNodeCanvasMinor = new Color(1.0F, 1.0F, 1.0F, 0.10F);

            kNodeCanvasMargeBlack = new Color(0.1F, 0.1F, 0.1F, 1.0F);

            kNodeLineColor = new Color(1.0F, 1.0F, 1.0F, 0.40F);
            kNodeOverLineColor = new Color(0.50F, 0.50F, 0.50F, 0.70F);

            // Inspector 

            kIdentityColor = new Color(0.7f, 0.7f, 0.7f, 0.4f);
            kPropertyColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);

            if (EditorGUIUtility.isProSkin)
            {
                kIdentityColor = new Color(0.3f, 0.3f, 0.3f, 0.4f);
                kPropertyColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);

                kRedElementColor = tRedColor;

                //KTAB_BAR_LINE_COLOR = NWDToolbox.ColorWithAlpha(tBaseColor, 0.7F);

                // TODO : change color for better color
                KTAB_BAR_BACK_COLOR = NWDToolbox.ColorWithAlpha(tBaseColor, 0.7F);
                KTAB_BAR_LINE_COLOR = new Color(0.3f, 0.3f, 0.3f, 0.4f);
                KTAB_BAR_HIGHLIGHT_COLOR = new Color(0.3f, 0.3f, 0.3f, 0.4f);

                kRowColorSelected = NWDToolbox.ColorWithAlpha(tBaseColor, 0.8F);
                kRowColorSelectedDark = NWDToolbox.ColorWithAlpha(tBaseColor, 0.8F);

                kTitleColor = NWDToolbox.ColorWithAlpha(tBaseColor, 0.7F);
                kSectionColor = NWDToolbox.ColorWithAlpha(tBaseColor, 0.5F);
                kSubSectionColor = NWDToolbox.ColorWithAlpha(tBaseColor, 0.2F);


                kRowColorLineWhite = NWDToolbox.ColorWithAlpha(NWDToolbox.MixColor(tBaseColor, Color.white), 0.25F);
                //kRowColorLineBlack = NWDToolbox.ColorWithAlpha(NWDToolbox.MixColor(tBaseColor,Color.black), 0.25F);
            }
        }


        public static void LoadStyles()
        {


            if (Application.isPlaying)
            {
                if (ProSkinEditor == true)
                {
                    ProSkinEditor = false;
                    StyleLoaded = false;
                }
            }
            else
            {
                if (ProSkinEditor == false)
                {
                    ProSkinEditor = true;
                    StyleLoaded = false;
                }
            }

            //NWDBenchmark.Start();
            if (StyleLoaded == false)
            {
                LoadColor();

                kIconLock = new GUIStyle(EditorStyles.label);
                kIconLock.imagePosition = ImagePosition.ImageOnly;
                kIconLock.alignment = TextAnchor.MiddleCenter;
                kIconLock.fixedHeight = 16;
                kIconLock.fixedWidth = 16;


                // texture apply
                kSeparatorTexture = new Texture2D(1, 1);
                kSeparatorTexture.SetPixel(0, 0, kSeparatorColor);
                kSeparatorTexture.Apply();

                kLineTexture = new Texture2D(1, 1);
                kLineTexture.SetPixel(0, 0, kLineColor);
                kLineTexture.Apply();
                kLineWhiteTexture = new Texture2D(1, 1);
                kLineWhiteTexture.SetPixel(0, 0, kLineWhiteColor);
                kLineWhiteTexture.Apply();

                kTitleTexture = new Texture2D(1, 1);
                kTitleTexture.SetPixel(0, 0, kTitleColor);
                kTitleTexture.Apply();

                kSectionTexture = new Texture2D(1, 1);
                kSectionTexture.SetPixel(0, 0, kSectionColor);
                kSectionTexture.Apply();

                kSubSectionTexture = new Texture2D(1, 1);
                kSubSectionTexture.SetPixel(0, 0, kSubSectionColor);
                kSubSectionTexture.Apply();
                // Scrollview full marging
                kScrollviewFullWidth = new GUIStyle(EditorStyles.inspectorFullWidthMargins);
                kScrollviewFullWidth.padding = new RectOffset(0, 0, 0, 0);
                kScrollviewFullWidth.margin = new RectOffset(0, 0, 0, 0);
                // Separator
                kSeparatorStyle = new GUIStyle(EditorStyles.label);
                kSeparatorStyle.fontSize = 0;
                kSeparatorStyle.normal.background = kSeparatorTexture;
                kSeparatorStyle.padding = new RectOffset(2, 2, 2, 2);
                kSeparatorStyle.margin = new RectOffset(0, 0, 1, 1);
                kSeparatorStyle.fixedHeight = kSeparatorHeight;
                // Line
                kLineStyle = new GUIStyle(EditorStyles.label);
                kLineStyle.fontSize = 0;
                kLineStyle.normal.background = kLineTexture;
                kLineStyle.padding = new RectOffset(0, 0, 0, 0);
                kLineStyle.margin = new RectOffset(0, 0, 0, 0);
                kLineStyle.fixedHeight = kLineHeight;
                kLineWhiteStyle = new GUIStyle(kLineStyle);
                kLineWhiteStyle.normal.background = kLineWhiteTexture;
                // general windows design 
                kTitleStyle = new GUIStyle(EditorStyles.label);
                kTitleStyle.fontSize = 14;
                //kTitleStyle.fontStyle = FontStyle.Bold;
                kTitleStyle.normal.background = kTitleTexture;
                kTitleStyle.padding = new RectOffset(6, 2, 16, 2);
                kTitleStyle.margin = new RectOffset(0, 0, 1, 1);
                kTitleStyle.fixedHeight = 48F;
                kTitleStyle.richText = true;
                kSectionStyle = new GUIStyle(EditorStyles.label);
                kSectionStyle.fontSize = 12;
                kSectionStyle.fontStyle = FontStyle.Italic;
                kSectionStyle.normal.background = kSectionTexture;
                kSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSectionStyle.fixedHeight = kSectionStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kSubSectionStyle = new GUIStyle(EditorStyles.label);
                kSubSectionStyle.fontSize = 12;
                kSubSectionStyle.fontStyle = FontStyle.Italic;
                kSubSectionStyle.normal.background = kSubSectionTexture;
                kSubSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSubSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSubSectionStyle.fixedHeight = kSubSectionStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                // Table design
                KTableClassToolbar = new GUIStyle(GUI.skin.button);
                KTableClassToolbar.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                KTableClassPopup = new GUIStyle(EditorStyles.popup);
                KTableClassPopup.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                KTableToolbar = new GUIStyle(EditorStyles.toolbar);
                KTableSearchIcon = new GUIStyle(EditorStyles.label);
                KTableSearchTitle = new GUIStyle(EditorStyles.boldLabel);
                KTableSearchDescription = new GUIStyle(EditorStyles.helpBox);
                KTableSearchClassIcon = new GUIStyle(EditorStyles.label);
                KTableSearchClassIcon.imagePosition = ImagePosition.ImageAbove;
                KTableSearchClassIcon.alignment = TextAnchor.MiddleCenter;
                KTableSearchTextfield = new GUIStyle(EditorStyles.textField);
                KTableSearchToggle = new GUIStyle(EditorStyles.toggle);
                KTableSearchEnum = new GUIStyle(EditorStyles.popup);
                KTableSearchMask = new GUIStyle(EditorStyles.layerMaskField);
                KTableSearchButton = new GUIStyle(EditorStyles.miniButton);
                KTableSearchLabel = new GUIStyle(EditorStyles.label);
                int tTextSize = 10;
                KTableSearchButton.fontSize = tTextSize;
                KTableSearchLabel.fontSize = tTextSize;
                KTableSearchTextfield.fontSize = tTextSize;
                KTableSearchToggle.fontSize = tTextSize;
                KTableSearchEnum.fontSize = tTextSize;
                KTableSearchMask.fontSize = tTextSize;

                KTableToolbar.fixedHeight = KTableToolbar.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                KTableSearchIcon.alignment = TextAnchor.MiddleCenter;
                float tTableSearchHeight = KTableSearchTextfield.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                KTableSearchTitle.fixedHeight = tTableSearchHeight;
                KTableSearchTextfield.fixedHeight = tTableSearchHeight;
                KTableSearchToggle.fixedHeight = tTableSearchHeight;
                KTableSearchEnum.fixedHeight = tTableSearchHeight;
                KTableSearchMask.fixedHeight = tTableSearchHeight;
                KTableSearchButton.fixedHeight = tTableSearchHeight;
                KTableSearchLabel.fixedHeight = tTableSearchHeight;
                KTableSearchTitle.alignment = TextAnchor.LowerLeft;
                KTableSearchIcon.alignment = TextAnchor.MiddleCenter;
                KTableHeaderSelect = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderPrefab = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderId = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderIcon = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderInformations = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderStatut = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderReference = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderSelect.alignment = TextAnchor.MiddleCenter;
                KTableHeaderPrefab.alignment = TextAnchor.MiddleCenter;
                KTableHeaderId.alignment = TextAnchor.MiddleCenter;
                KTableHeaderIcon.alignment = TextAnchor.MiddleCenter;
                KTableHeaderInformations.alignment = TextAnchor.MiddleCenter;
                KTableHeaderStatut.alignment = TextAnchor.MiddleCenter;
                KTableHeaderReference.alignment = TextAnchor.MiddleCenter;
                KTableRowSelect = new GUIStyle(EditorStyles.label);
                KTableRowPrefab = new GUIStyle(EditorStyles.label);
                KTableRowId = new GUIStyle(EditorStyles.label);
                KTableRowIcon = new GUIStyle(EditorStyles.label);
                KTableRowInformations = new GUIStyle(EditorStyles.label);
                KTableRowStatut = new GUIStyle(EditorStyles.label);
                KTableRowReference = new GUIStyle(EditorStyles.label);
                KTableRowSelect.fixedHeight = KTableRowSelect.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
#if UNITY_2019_3_OR_NEWER
                KTableRowSelect.fixedHeight = 36.0F;
#endif
                KTableRowSelect.alignment = TextAnchor.MiddleCenter;

                KTableRowNotSelected = new GUIStyle(EditorStyles.label);
                KTableRowNotSelected.border = new RectOffset(0, 0, 0, 0);
                KTableRowNotSelected.margin = new RectOffset(0, 0, 0, 0);
                KTableRowSelected = new GUIStyle(KTableRowNotSelected);
                Texture2D tLineTexture = new Texture2D(1, 1);
                tLineTexture.SetPixel(0, 0, NWDGUI.kRowColorSelected);
                tLineTexture.Apply();
                KTableRowSelected.normal.background = tLineTexture;


                KTableRowPrefab.alignment = TextAnchor.MiddleCenter;
                KTableRowId.alignment = TextAnchor.MiddleRight;
                KTableRowIcon.alignment = TextAnchor.MiddleCenter;
                KTableRowInformations.alignment = TextAnchor.MiddleLeft;
                KTableRowStatut.alignment = TextAnchor.MiddleCenter;
                KTableRowReference.alignment = TextAnchor.MiddleRight;
                KTableRowIcon.richText = true;
                KTableRowInformations.richText = true;
                KTableRowStatut.richText = true;
                KTableRowIcon.padding = new RectOffset(10, 10, 10, 10);
                KTableRowInformations.wordWrap = true;
                // Inspector 
                kInspectorInternalTitle = new GUIStyle(EditorStyles.boldLabel);
                kInspectorInternalTitle.alignment = TextAnchor.MiddleCenter;
                kInspectorInternalTitle.fontSize = 14;
                kInspectorInternalTitle.fixedHeight = kInspectorInternalTitle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kInspectorInternalTitle.richText = true;
                kInspectorReferenceCenter = new GUIStyle(EditorStyles.miniLabel);
                kInspectorReferenceCenter.fixedHeight = kInspectorReferenceCenter.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kInspectorReferenceCenter.alignment = TextAnchor.MiddleCenter;
                kInspectorNoData = new GUIStyle(EditorStyles.miniLabel);
                kInspectorNoData.alignment = TextAnchor.MiddleCenter;
                kLabelStyle = new GUIStyle(EditorStyles.label);
                kLabelStyle.fixedHeight = kLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kFooterLabelStyle = new GUIStyle(EditorStyles.label);
                kFooterLabelStyle.fixedHeight = kLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kFooterLabelStyle.richText = true;
                kMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                kMiniLabelStyle.fixedHeight = kMiniLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                kBoldLabelStyle.fixedHeight = kBoldLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kItalicLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                kItalicLabelStyle.fixedHeight = kItalicLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kItalicLabelStyle.fontStyle = FontStyle.Italic;
                kNoConfigStyle = new GUIStyle(EditorStyles.boldLabel);
                kNoConfigStyle.fixedHeight = kItalicLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kNoConfigStyle.fontStyle = FontStyle.Italic;
                kNoConfigStyle.alignment = TextAnchor.MiddleCenter;
                kHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                kHelpBoxStyle.fixedHeight = kHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), 100);
                kMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kMiniButtonStyle.fixedHeight = kMiniButtonStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
                kObjectFieldStyle.fixedHeight = kObjectFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kTextFieldStyle = new GUIStyle(EditorStyles.textField);
                kTextFieldStyle.fixedHeight = kTextFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);
                kTextFieldStyle.richText = true;
                kFloatFieldStyle = new GUIStyle(EditorStyles.numberField);
                kFloatFieldStyle.fixedHeight = kFloatFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kIntFieldStyle = new GUIStyle(EditorStyles.numberField);
                kIntFieldStyle.fixedHeight = kIntFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kLongFieldStyle = new GUIStyle(EditorStyles.numberField);
                kLongFieldStyle.fixedHeight = kLongFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kDoubleFieldStyle = new GUIStyle(EditorStyles.numberField);
                kDoubleFieldStyle.fixedHeight = kDoubleFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                kFoldoutStyle.fixedHeight = kFoldoutStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kColorFieldStyle = new GUIStyle(EditorStyles.colorField);
                kColorFieldStyle.fixedHeight = kColorFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kPopupStyle = new GUIStyle(EditorStyles.popup);
                kPopupStyle.fixedHeight = kPopupStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kEnumStyle = new GUIStyle(EditorStyles.popup);
                kEnumStyle.fixedHeight = kEnumStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kToggleStyle = new GUIStyle(EditorStyles.toggle);
                kToggleStyle.fixedHeight = kToggleStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                kBoldFoldoutStyle.fontStyle = FontStyle.Bold;
                kBoldFoldoutStyle.fixedHeight = kBoldFoldoutStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100);

                kLabelRightStyle = new GUIStyle(EditorStyles.label);
                kLabelRightStyle.alignment = TextAnchor.MiddleRight;
                kLabelRightStyle.fixedHeight = kLabelRightStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);

                kCredentialTagRightStyle = new GUIStyle(EditorStyles.label);
                kCredentialTagRightStyle.alignment = TextAnchor.LowerRight;
                kCredentialTagRightStyle.fixedHeight = kCredentialTagRightStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kCredentialTagRightStyle.fontStyle = FontStyle.Italic;

                kTextAreaStyle = new GUIStyle(EditorStyles.textField);
                kTextAreaStyle.wordWrap = true;

                kRedLabelStyle = new GUIStyle(EditorStyles.label);
                kRedLabelStyle.fixedHeight = kRedLabelStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kRedLabelStyle.normal.textColor = Color.red;

                kMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kMiniButtonStyle.fixedHeight = kMiniButtonStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);


                kIconCenterStyle = new GUIStyle(EditorStyles.label);
                kIconCenterStyle.fixedHeight = 36;
                kIconCenterStyle.alignment = TextAnchor.MiddleCenter;
                kIconCenterStyle.imagePosition = ImagePosition.ImageOnly;

                kIconLeftStyle = new GUIStyle(EditorStyles.label);
                kIconLeftStyle.fixedHeight = 32;
                kIconLeftStyle.alignment = TextAnchor.MiddleLeft;
                //kIconLeftStyle.imagePosition = ImagePosition.ImageOnly;

                // Data Selector design

                kPropertyEntitlementStyle = new GUIStyle(EditorStyles.label);
                kPropertyEntitlementStyle.fixedHeight = kPropertyEntitlementStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kPropertyEntitlementStyle.richText = true;
                kPropertyEntitlementStyle.alignment = TextAnchor.MiddleLeft;

                kDataSelectorPropertieStyle = new GUIStyle(EditorStyles.label);

                kDataSelectorFieldStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorFieldStyle.richText = true;
                kDataSelectorFieldStyle.fontSize = 12;
                kDataSelectorFieldStyle.wordWrap = false;
                kDataSelectorFieldStyle.alignment = TextAnchor.MiddleLeft;
                kDataSelectorFieldStyle.imagePosition = ImagePosition.TextOnly;
                kDataSelectorFieldStyle.border = new RectOffset(2, 2, 2, 2);
                kDataSelectorFieldStyle.fixedHeight = kDataSelectorFieldStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);

                kDataSelectorPropertieStyle = new GUIStyle(EditorStyles.label);
                kDataSelectorPropertieStyle.richText = true;
                kDataSelectorPropertieStyle.alignment = TextAnchor.MiddleLeft;
                kDataSelectorPropertieStyle.fixedHeight = kDataSelectorFieldStyle.fixedHeight;
                // Selector design
                //kDataSelectorFieldIconStyle = new GUIStyle(kDataSelectorFieldStyle);
                kDataSelectorFieldIconStyle = new GUIStyle(EditorStyles.label);
                kDataSelectorFieldIconStyle.imagePosition = ImagePosition.ImageOnly;
                kDataSelectorFieldIconStyle.border = new RectOffset(0, 0, 0, 0);
                kDataSelectorFieldIconStyle.margin = new RectOffset(0, 0, 0, 0);
                kDataSelectorFieldIconStyle.fixedHeight = 24;
                kDataSelectorFieldIconStyle.fixedWidth = 24;

                kDataSelectorTileStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorTileStyle.richText = true;
                kDataSelectorTileStyle.fontSize = 10;
                kDataSelectorTileStyle.imagePosition = ImagePosition.ImageAbove;
                kDataSelectorTileStyle.border = new RectOffset(2, 2, 2, 4);
                kDataSelectorTileStyle.alignment = TextAnchor.LowerCenter;
                kDataSelectorTileStyle.fixedHeight = kDataSelectorTileStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);

                kDataSelectorRowStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorRowStyle.richText = true;
                kDataSelectorRowStyle.fontSize = 14;
                kDataSelectorRowStyle.imagePosition = ImagePosition.ImageLeft;
                kDataSelectorRowStyle.border = new RectOffset(2, 4, 2, 2);
                kDataSelectorRowStyle.alignment = TextAnchor.MiddleLeft;
                kDataSelectorRowStyle.fixedHeight = kDataSelectorRowStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);

                // References content

                kIconButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kIconButtonStyle.fixedHeight = kIconButtonStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kIconButtonStyle.padding = new RectOffset(2, 2, 2, 2);
                kUpDownWidth = kIconButtonStyle.fixedHeight;

                kEditButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kEditButtonStyle.fixedHeight = kEditButtonStyle.CalcHeight(new GUIContent(NWDConstants.K_A), 100.0F);
                kEditButtonStyle.padding = new RectOffset(2, 2, 2, 2);

                kNodeContentIcon = new GUIContent(/*kImageSelectionUpdate,*/ "node");

                kEditContentIcon = new GUIContent(/*kImageTabReduce,*/ "edit");
                kNewContentIcon = new GUIContent(/*kImageNew, */"new");
                kCleanContentIcon = new GUIContent(/*kImageAction, */"clean");

                kUpContentIcon = new GUIContent(kImageUp, "up");
                kDownContentIcon = new GUIContent(kImageDown, "down");
                kLeftContentIcon = new GUIContent(kImageLeft, "<");
                kRightContentIcon = new GUIContent(kImageRight, ">");

                kDatasSelectorYOffset = 2;

            }
            //NWDBenchmark.Finish();
        }

        public static Rect MargeLeftRight(Rect sRect)
        {
            return new Rect(sRect.x + kFieldMarge, sRect.y, sRect.width - kFieldMarge * 2, sRect.height);
        }

        public static Rect MargeTopBottom(Rect sRect)
        {
            return new Rect(sRect.x, sRect.y + kFieldMarge, sRect.width, sRect.height - kFieldMarge * 2);
        }

        public static Rect MargeAll(Rect sRect)
        {
            return new Rect(sRect.x + kFieldMarge, sRect.y + kFieldMarge, sRect.width - kFieldMarge * 2, sRect.height - kFieldMarge * 2);
        }

        public static Rect UnMargeAll(Rect sRect)
        {
            return new Rect(sRect.x - kFieldMarge, sRect.y - kFieldMarge, sRect.width + kFieldMarge * 2, sRect.height + kFieldMarge * 2);
        }

        public static Rect MargeInspector(Rect sRect, bool sLeft, bool sRight, bool sTop, bool sBottom)
        {
            Rect tReturn = new Rect(sRect.x, sRect.y, sRect.width, sRect.height);
            if (sLeft)
            {
                tReturn.x += kFieldMarge;
                tReturn.width -= kFieldMarge;
            }
            if (sRight)
            {
                tReturn.width -= kFieldMarge;
            }
            if (sTop)
            {
                tReturn.y += kFieldMarge;
                tReturn.height -= kFieldMarge;
            }
            if (sBottom)
            {
                tReturn.height -= kFieldMarge;
            }
            return tReturn;
        }

        public static Rect UnMargeInspector(Rect sRect, bool sLeft, bool sRight, bool sTop, bool sBottom)
        {
            Rect tReturn = new Rect(sRect.x, sRect.y, sRect.width, sRect.height);
            if (sLeft)
            {
                tReturn.x -= kFieldMarge;
                tReturn.width += kFieldMarge;
            }
            if (sRight)
            {
                tReturn.width += kFieldMarge;
            }
            if (sTop)
            {
                tReturn.y -= kFieldMarge;
                tReturn.height += kFieldMarge;
            }
            if (sBottom)
            {
                tReturn.height += kFieldMarge;
            }
            return tReturn;
        }

        public static Rect UnMargeLeftRight(Rect sRect)
        {
            return new Rect(sRect.x - kFieldMarge, sRect.y, sRect.width + kFieldMarge * 2, sRect.height);
        }

        public static Rect Line(Rect sRect)
        {
            sRect.height = kLineStyle.fixedHeight;
            GUI.Label(sRect, string.Empty, kLineStyle);
            sRect.y += kLineStyle.fixedHeight;
            return sRect;
        }

        public static Rect Line(Rect sRect, Color sColor)
        {
            Texture2D tOldLineTexture = kLineStyle.normal.background;
            Texture2D tLineTexture = new Texture2D(1, 1);
            tLineTexture.SetPixel(0, 0, sColor);
            tLineTexture.Apply();
            kLineStyle.normal.background = tLineTexture;
            GUI.Label(sRect, string.Empty, kLineStyle);
            sRect.y += kLineStyle.fixedHeight;
            kLineStyle.normal.background = tOldLineTexture;
            return sRect;
        }

        public static Rect LineVertical(Rect sRect)
        {
            float tOld = kLineStyle.fixedHeight;
            sRect.width = kLineStyle.fixedHeight;
            kLineStyle.fixedHeight = sRect.height;
            GUI.Label(sRect, string.Empty, kLineStyle);
            kLineStyle.fixedHeight = tOld;
            sRect.y += kLineStyle.fixedHeight;
            return sRect;
        }

        public static Rect LineVertical(Rect sRect, Color sColor)
        {
            Texture2D tOldLineTexture = kLineStyle.normal.background;
            Texture2D tLineTexture = new Texture2D(1, 1);
            tLineTexture.SetPixel(0, 0, sColor);
            tLineTexture.Apply();
            float tOld = kLineStyle.fixedHeight;
            sRect.width = kLineStyle.fixedHeight;
            kLineStyle.fixedHeight = sRect.height;
            kLineStyle.normal.background = tLineTexture;
            GUI.Label(sRect, string.Empty, kLineStyle);
            kLineStyle.fixedHeight = tOld;
            sRect.y += kLineStyle.fixedHeight;
            kLineStyle.normal.background = tOldLineTexture;
            return sRect;
        }

        public static Rect Separator(Rect sRect)
        {
            sRect.y += kFieldMarge;
            sRect.height = kSeparatorStyle.fixedHeight;
            GUI.Label(sRect, string.Empty, kSeparatorStyle);
            sRect.y += kSeparatorStyle.fixedHeight + kFieldMarge;
            sRect.height = kSeparatorStyle.fixedHeight + kFieldMarge * 2;
            return sRect;
        }

        public static float SeparatorHeight()
        {
            return kSeparatorStyle.fixedHeight + kFieldMarge * 2;
        }

        public static void BeginRedArea(bool sCondition = true)
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            GUI.backgroundColor = kRedElementColor;
        }

        public static void EndRedArea()
        {
            if (kOldColorInit == true)
            {
                GUI.backgroundColor = kOldColor;
            }
        }

        public static Color DefaultColorArea()
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            return kOldColor;
        }

        public static void BeginColorArea(Color sColor)
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            GUI.backgroundColor = sColor;
        }

        public static void EndColorArea()
        {
            if (kOldColorInit == true)
            {
                GUI.backgroundColor = kOldColor;
            }
        }

        public static Rect Title(Rect sRect, string sTitle)
        {
            sRect.height = kTitleStyle.fixedHeight;
            GUI.Label(sRect, sTitle, NWDGUI.kTitleStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            Line(new Rect(sRect.x, sRect.y + kTitleStyle.fixedHeight - 1, sRect.width, 1));
            return sRect;
        }

        public static Rect Title(Rect sRect, GUIContent sTitle)
        {
            sRect.height = kTitleStyle.fixedHeight;
            GUI.Label(sRect, sTitle, NWDGUI.kTitleStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            Line(new Rect(sRect.x, sRect.y + kTitleStyle.fixedHeight - 1, sRect.width, 1));
            return sRect;
        }

        public static Rect Section(Rect sRect, string sTitle)
        {
            sRect.height = kSectionStyle.fixedHeight;
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, 1));
            return sRect;
        }

        public static Rect Section(Rect sRect, GUIContent sTitle)
        {
            sRect.height = kSectionStyle.fixedHeight;
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, 1));
            return sRect;
        }

        public static Rect SubSection(Rect sRect, string sTitle)
        {
            sRect.height = kSubSectionStyle.fixedHeight;
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight + NWDGUI.kFieldMarge;
            return sRect;
        }

        public static Rect SubSection(Rect sRect, GUIContent sTitle)
        {
            sRect.height = kSubSectionStyle.fixedHeight;
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight + NWDGUI.kFieldMarge;
            return sRect;
        }

        public static Rect Informations(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.None);
            //sRect.height += kFieldMarge * 2;
            return sRect;
        }

        public static Rect HelpBox(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Info);
            //sRect.height += kFieldMarge * 2;
            return sRect;
        }

        public static Rect WarningBox(Rect sRect, string sTitle)
        {
            BeginColorArea(kYellowElementColor);
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (sRect.height < WarningMinHeight)
            {
                sRect.height = WarningMinHeight;
            }
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Warning);
            EndColorArea();
            return sRect;
        }

        public static float WarningBoxHeight(Rect sRect, string sTitle)
        {
            float tHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (tHeight < WarningMinHeight)
            {
                tHeight = WarningMinHeight;
            }
            return tHeight;
        }

        public static Rect ErrorBox(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            BeginColorArea(kRedElementColor);
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (sRect.height < ErrorMinHeight)
            {
                sRect.height = ErrorMinHeight;
            }
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Error);
            EndColorArea();
            //sRect.height += kFieldMarge*2;
            return sRect;
        }

        public static ulong DataField (Rect sPosition, GUIContent sLabel, Type sDataType, ulong sReference, ref bool sFoldout)
        {
            int tControlId = GUIUtility.GetControlID(FocusType.Passive);
            Rect tRect = sPosition;
            tRect.height = EditorGUIUtility.singleLineHeight;
            sFoldout = EditorGUI.Foldout(tRect, sFoldout, GUIContent.none);

            tRect = EditorGUI.PrefixLabel(tRect, sLabel);

            NWDMetaData tMetaData = NWDUnityEngineEditor.Instance.GetDataManager().GetMetaDataByReference(sReference);
            string tContent;
            if (tMetaData != null)
            {
                tContent = tMetaData.Title + " (" + tMetaData.Reference + ")";
            }
            else
            {
                tContent = "None (" + sDataType.Name + ")";
            }

            Rect sDisplayRect = tRect;

            GUI.Box(sDisplayRect, tContent, new GUIStyle("ObjectField"));

            sDisplayRect = new Rect(tRect.xMax - 19, tRect.y + 1, 18, 16);

            GUI.Box(sDisplayRect, GUIContent.none, new GUIStyle("ObjectFieldButton"));

            sDisplayRect = tRect;
            sDisplayRect.xMax -= 20;

            if (GUI.Button(sDisplayRect, GUIContent.none, GUIStyle.none))
            {
                if (tMetaData != null)
                {
                    NWDModelWindow.ShowEditionWindowForType(sDataType, tMetaData.Reference);
                }
                else
                {
                    NWDModelWindow.ShowEditionWindowForType(sDataType);
                }
            }

            tRect.xMin = tRect.xMax - 20;

            if (GUI.Button(tRect, GUIContent.none, GUIStyle.none))
            {
                // Open data selection window
                NWDDataPicker.ShowWindow(tControlId, sDataType);
            }

            if (sFoldout)
            {

            }

            if (NWDDataPicker.TryGetPickedReference(tControlId, out ulong tReference))
            {
                GUI.changed = true;
                sReference = tReference;
            }

            return sReference;
        }

        public static DateTime DateField (Rect sPosition, GUIContent sLabel, DateTime sData)
        {
            int tYear = sData.Year;
            int tMonth = sData.Month;
            int tDay = sData.Day;

            sPosition = EditorGUI.PrefixLabel(sPosition, sLabel);

            sPosition.xMax -= 80 + 40;
            tYear = EditorGUI.IntField(sPosition, tYear);

            sPosition.xMin = sPosition.xMax;
            sPosition.xMax = sPosition.xMin + 20;
            EditorGUI.LabelField(sPosition, "/");

            sPosition.xMin = sPosition.xMax;
            sPosition.xMax = sPosition.xMin + 40;
            tMonth = EditorGUI.IntField(sPosition, tMonth);


            sPosition.xMin = sPosition.xMax;
            sPosition.xMax = sPosition.xMin + 20;
            EditorGUI.LabelField(sPosition, "/");

            sPosition.xMin = sPosition.xMax;
            sPosition.xMax = sPosition.xMin + 40;
            tDay = EditorGUI.IntField(sPosition, tDay);

            if (tYear >= DateTime.MinValue.Year && tYear <= DateTime.MaxValue.Year
                && tMonth >= DateTime.MinValue.Month && tMonth <= DateTime.MaxValue.Month
                && tDay >= DateTime.MinValue.Day && tDay <= DateTime.MaxValue.Day)
            {
                try
                {
                    sData = new DateTime(tYear, tMonth, tDay);
                }
                catch { }
            }

            return sData;
        }

        public static long DateField(Rect sPosition, GUIContent sLabel, long sNWDTimestamp)
        {
            DateTime tDate = DateField(sPosition, sLabel, NWDTimestamp.TimeStampToDateTime(sNWDTimestamp));
            return NWDTimestamp.Timestamp(tDate);
        }
    }
}