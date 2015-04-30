namespace UI_Test
{
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using System;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    using System.Drawing;
    using System.Windows.Input;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;


    public partial class UIMap
    {

        /// <summary>
        /// SimpleAppTest - Use 'SimpleAppTestParams' to pass parameters into this method.
        /// </summary>
        public void ModifiedSimpleAppTest()
        {
            #region Variable Declarations
            WpfCheckBox uIChkAutoResizeCheckBox = this.UIAtlasEngineWindow.UIChkAutoResizeCheckBox;
            WpfMenuItem uIExitProgramMenuItem = this.UIAtlasEngineWindow.UIMenuMenu.UIFileMenuItem.UIExitProgramMenuItem;
            WinButton uIYesButton = this.UIAreYouSureWindow.UIYesWindow.UIYesButton;
            #endregion

            // Launch '%USERPROFILE%\Documents\GitHub\AtlasEngine\AtlasEngine\bin\Debug\AtlasEngine.exe'
            ApplicationUnderTest uIAtlasEngineWindow = ApplicationUnderTest.Launch(this.SimpleAppTestParams.UIAtlasEngineWindowExePath, this.SimpleAppTestParams.UIAtlasEngineWindowAlternateExePath);

            uIChkAutoResizeCheckBox.WaitForControlEnabled();

            // Select 'chkAutoResize' check box
            uIChkAutoResizeCheckBox.Checked = this.SimpleAppTestParams.UIChkAutoResizeCheckBoxChecked;

            // Click 'File' -> 'Exit Program' menu item
            Mouse.Click(uIExitProgramMenuItem, new Point(67, 15));

            // Click '&Yes' button
            Mouse.Click(uIYesButton, new Point(70, 12));
        }

        public virtual SimpleAppTestParams SimpleAppTestParams
        {
            get
            {
                if ((this.mSimpleAppTestParams == null))
                {
                    this.mSimpleAppTestParams = new SimpleAppTestParams();
                }
                return this.mSimpleAppTestParams;
            }
        }

        private SimpleAppTestParams mSimpleAppTestParams;
    }
    /// <summary>
    /// Parameters to be passed into 'SimpleAppTest'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "12.0.21005.1")]
    public class SimpleAppTestParams
    {

        #region Fields
        /// <summary>
        /// Launch '%USERPROFILE%\Documents\GitHub\AtlasEngine\AtlasEngine\bin\Debug\AtlasEngine.exe'
        /// </summary>
        public string UIAtlasEngineWindowExePath = "C:\\Users\\jeffrey.johnson\\Documents\\GitHub\\AtlasEngine\\AtlasEngine\\bin\\Debug\\Atlas" +
            "Engine.exe";

        /// <summary>
        /// Launch '%USERPROFILE%\Documents\GitHub\AtlasEngine\AtlasEngine\bin\Debug\AtlasEngine.exe'
        /// </summary>
        public string UIAtlasEngineWindowAlternateExePath = "%USERPROFILE%\\Documents\\GitHub\\AtlasEngine\\AtlasEngine\\bin\\Debug\\AtlasEngine.exe";

        /// <summary>
        /// Select 'chkAutoResize' check box
        /// </summary>
        public bool UIChkAutoResizeCheckBoxChecked = true;
        #endregion
}
}
