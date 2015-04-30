﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 12.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace UI_Test
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public partial class UIMap
    {
        
        #region Properties
        public UIAtlasEngineWindow UIAtlasEngineWindow
        {
            get
            {
                if ((this.mUIAtlasEngineWindow == null))
                {
                    this.mUIAtlasEngineWindow = new UIAtlasEngineWindow();
                }
                return this.mUIAtlasEngineWindow;
            }
        }
        
        public UIAreYouSureWindow UIAreYouSureWindow
        {
            get
            {
                if ((this.mUIAreYouSureWindow == null))
                {
                    this.mUIAreYouSureWindow = new UIAreYouSureWindow();
                }
                return this.mUIAreYouSureWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIAtlasEngineWindow mUIAtlasEngineWindow;
        
        private UIAreYouSureWindow mUIAreYouSureWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class UIAtlasEngineWindow : WpfWindow
    {
        
        public UIAtlasEngineWindow()
        {
            #region Search Criteria
            this.SearchProperties[WpfWindow.PropertyNames.Name] = "Atlas Engine";
            this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Atlas Engine");
            #endregion
        }
        
        #region Properties
        public WpfCheckBox UIChkAutoResizeCheckBox
        {
            get
            {
                if ((this.mUIChkAutoResizeCheckBox == null))
                {
                    this.mUIChkAutoResizeCheckBox = new WpfCheckBox(this);
                    #region Search Criteria
                    this.mUIChkAutoResizeCheckBox.SearchProperties[WpfCheckBox.PropertyNames.AutomationId] = "chkAutoResize";
                    this.mUIChkAutoResizeCheckBox.WindowTitles.Add("Atlas Engine");
                    #endregion
                }
                return this.mUIChkAutoResizeCheckBox;
            }
        }
        
        public UIMenuMenu UIMenuMenu
        {
            get
            {
                if ((this.mUIMenuMenu == null))
                {
                    this.mUIMenuMenu = new UIMenuMenu(this);
                }
                return this.mUIMenuMenu;
            }
        }
        #endregion
        
        #region Fields
        private WpfCheckBox mUIChkAutoResizeCheckBox;
        
        private UIMenuMenu mUIMenuMenu;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class UIMenuMenu : WpfMenu
    {
        
        public UIMenuMenu(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WpfMenu.PropertyNames.AutomationId] = "menu";
            this.WindowTitles.Add("Atlas Engine");
            #endregion
        }
        
        #region Properties
        public UIFileMenuItem UIFileMenuItem
        {
            get
            {
                if ((this.mUIFileMenuItem == null))
                {
                    this.mUIFileMenuItem = new UIFileMenuItem(this);
                }
                return this.mUIFileMenuItem;
            }
        }
        #endregion
        
        #region Fields
        private UIFileMenuItem mUIFileMenuItem;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class UIFileMenuItem : WpfMenuItem
    {
        
        public UIFileMenuItem(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WpfMenuItem.PropertyNames.AutomationId] = "menuItemFile";
            this.WindowTitles.Add("Atlas Engine");
            #endregion
        }
        
        #region Properties
        public WpfMenuItem UIExitProgramMenuItem
        {
            get
            {
                if ((this.mUIExitProgramMenuItem == null))
                {
                    this.mUIExitProgramMenuItem = new WpfMenuItem(this);
                    #region Search Criteria
                    this.mUIExitProgramMenuItem.SearchProperties[WpfMenuItem.PropertyNames.Name] = "Exit Program";
                    this.mUIExitProgramMenuItem.SearchConfigurations.Add(SearchConfiguration.ExpandWhileSearching);
                    this.mUIExitProgramMenuItem.WindowTitles.Add("Atlas Engine");
                    #endregion
                }
                return this.mUIExitProgramMenuItem;
            }
        }
        #endregion
        
        #region Fields
        private WpfMenuItem mUIExitProgramMenuItem;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class UIAreYouSureWindow : WinWindow
    {
        
        public UIAreYouSureWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Are You Sure?";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "#32770";
            this.WindowTitles.Add("Are You Sure?");
            #endregion
        }
        
        #region Properties
        public UIYesWindow UIYesWindow
        {
            get
            {
                if ((this.mUIYesWindow == null))
                {
                    this.mUIYesWindow = new UIYesWindow(this);
                }
                return this.mUIYesWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIYesWindow mUIYesWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class UIYesWindow : WinWindow
    {
        
        public UIYesWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.ControlId] = "6";
            this.WindowTitles.Add("Are You Sure?");
            #endregion
        }
        
        #region Properties
        public WinButton UIYesButton
        {
            get
            {
                if ((this.mUIYesButton == null))
                {
                    this.mUIYesButton = new WinButton(this);
                    #region Search Criteria
                    this.mUIYesButton.SearchProperties[WinButton.PropertyNames.Name] = "Yes";
                    this.mUIYesButton.WindowTitles.Add("Are You Sure?");
                    #endregion
                }
                return this.mUIYesButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIYesButton;
        #endregion
    }
}
