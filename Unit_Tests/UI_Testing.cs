using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;

namespace Unit_Tests
{
    [TestClass]
    public class UI_Testing
    {
        private int DEFAULT_WIDTH = 256;
        private int DEFAULT_HEIGHT = 256;
        private string PROJECT_PATH = @"..\..\..\AtlasEngine\";
        private Process programProcess = null;
        private AutomationElement desktop = null;
        private AutomationElement mainWindow = null;
        private AutomationElement fileMenuItem = null;

        [TestInitialize]
        public void TestInit()
        {
            Console.WriteLine("Initializing test");
            StartProgram();
            FindDesktopElement();
            FindMainWindow();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("Cleaning up after test.");
            ClickFile_Exit();
            programProcess = null;
            desktop = null;
            mainWindow = null;
            fileMenuItem = null;

        }

        /// <summary>
        /// Verify the auto resize chk box is off by default, and the width and height boxes are correct
        /// </summary>
        [TestMethod]
        public void AutoResize_Default()
        {
            try
            {
                Console.WriteLine("starting test....");
                AutomationElement autoResize = GetElement("chkAutoResize");
                AutomationElement txtWidthReadOnly = GetElement("txtWidthReadOnly");
                AutomationElement txtWidthWrite = GetElement("txtWidthWrite");
                AutomationElement txtHeightReadOnly = GetElement("txtHeightReadOnly");
                AutomationElement txtHeightWrite = GetElement("txtHeightWrite");

                TogglePattern toggle = (TogglePattern)autoResize.GetCurrentPattern(TogglePattern.Pattern);

                //verify is off at startup
                Assert.AreEqual(ToggleState.Off, toggle.Current.ToggleState);

                //verify widthRO is not visible and widthWrite is
                Assert.IsFalse((bool)txtWidthWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsTrue((bool)txtWidthReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //verify heightRO is not visible and heightWrite is
                Assert.IsFalse((bool)txtHeightWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsTrue((bool)txtHeightReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //can't check canvas size now need to refactor

                //verify the value
                ValuePattern txtWidthValue = (ValuePattern)txtWidthWrite.GetCurrentPattern(ValuePattern.Pattern);
                ValuePattern txtHeightValue = (ValuePattern)txtHeightWrite.GetCurrentPattern(ValuePattern.Pattern);
                int width = Int32.Parse(txtWidthValue.Current.Value);
                int height = Int32.Parse(txtHeightValue.Current.Value);

                Assert.AreEqual(DEFAULT_WIDTH, width);
                Assert.AreEqual(DEFAULT_HEIGHT, height);

                toggle.Toggle();

                //verify widthRO is visible and widthWrite is not
                Assert.IsTrue((bool)txtWidthWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsFalse((bool)txtWidthReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //verify heightRO is visible and heightWrite is not
                Assert.IsTrue((bool)txtHeightWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsFalse((bool)txtHeightReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //verify the value
                width = Int32.Parse(txtWidthReadOnly.Current.Name);
                height = Int32.Parse(txtHeightReadOnly.Current.Name);
                Assert.AreEqual(DEFAULT_WIDTH, width);
                Assert.AreEqual(DEFAULT_HEIGHT, height);


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Caught:");
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AutoResize_Toggle()
        {
            try
            {
                Console.WriteLine("starting test....");
                AutomationElement autoResize = GetElement("chkAutoResize");
                AutomationElement txtWidthReadOnly = GetElement("txtWidthReadOnly");
                AutomationElement txtWidthWrite = GetElement("txtWidthWrite");
                AutomationElement txtHeightReadOnly = GetElement("txtHeightReadOnly");
                AutomationElement txtHeightWrite = GetElement("txtHeightWrite");

                TogglePattern toggle = (TogglePattern)autoResize.GetCurrentPattern(TogglePattern.Pattern);

                toggle.Toggle();

                //verify widthRO is visible and widthWrite is not
                Assert.IsTrue((bool)txtWidthWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsFalse((bool)txtWidthReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //verify heightRO is visible and heightWrite is not
                Assert.IsTrue((bool)txtHeightWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
                Assert.IsFalse((bool)txtHeightReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

                //verify the value
                int width = Int32.Parse(txtWidthReadOnly.Current.Name);
                int height = Int32.Parse(txtHeightReadOnly.Current.Name);
                Assert.AreEqual(DEFAULT_WIDTH, width);
                Assert.AreEqual(DEFAULT_HEIGHT, height);


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Caught:");
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
        }

        /// <summary>
        /// verify the sheet clears, scollviewer background is green and no alert message pops
        /// </summary>
        [TestMethod]
        public void NewSheetWithEmpty()
        {
            ClickFileMenuItem();
            AutomationElement fileNewMenuItem = GetElement("menuItemNewSheet");

            //verify no popup 
            //check for pop up window
            AutomationElement popUp = mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Are You Sure?"));
            Assert.IsNull(popUp);

            //verify
            VerifyDefaultSettings();
            

        }

        [TestMethod]
        public void NewSheetNotEmpty()
        {
            //ClickFileMenuItem();
            for (int i = 0; i < 5; i++)
            {
                LoadImage(PROJECT_PATH + @"resources\test_images\small\", "green_square_small.png");
            }
            //click file-new
            ClickFileMenuItem();
            AutomationElement fileNew = GetElement("menuItemNewSheet");
            InvokePattern fileNewPat = (InvokePattern)fileNew.GetCurrentPattern(InvokePattern.Pattern);
            fileNewPat.Invoke();

            //verify main window still there
            //mainWindow = desktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Atlas Engine"));
            //Assert.IsNotNull(mainWindow);

            //check for pop up window\
            AutomationElement popUp = null;
            int cnt = 0;
            do
            {
                Console.WriteLine("Looking for popup window...");
                popUp = mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Are You Sure?"));
                cnt++;
                Thread.Sleep(100);
            } while (null == popUp && cnt < 50);
            
            Assert.IsNotNull(popUp);

            //click yes button
            AutomationElement btnYes = popUp.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Yes"));
            Assert.IsNotNull(btnYes);

            InvokePattern btnPattern = (InvokePattern)btnYes.GetCurrentPattern(InvokePattern.Pattern);
            btnPattern.Invoke();

            //verify default settings
            VerifyDefaultSettings();


        }

        private void VerifyDefaultSettings()
        {
            AutomationElement txtWidthReadOnly = GetElement("txtWidthReadOnly");
            AutomationElement txtWidthWrite = GetElement("txtWidthWrite");
            AutomationElement txtHeightReadOnly = GetElement("txtHeightReadOnly");
            AutomationElement txtHeightWrite = GetElement("txtHeightWrite");

            //verify widthRO is not visible and widthWrite is
            Assert.IsFalse((bool)txtWidthWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
            Assert.IsTrue((bool)txtWidthReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

            //verify heightRO is not visible and heightWrite is
            Assert.IsFalse((bool)txtHeightWrite.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));
            Assert.IsTrue((bool)txtHeightReadOnly.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty));

            //can't check canvas size now need to refactor

            //verify the value
            ValuePattern txtWidthValue = (ValuePattern)txtWidthWrite.GetCurrentPattern(ValuePattern.Pattern);
            ValuePattern txtHeightValue = (ValuePattern)txtHeightWrite.GetCurrentPattern(ValuePattern.Pattern);
            int width = Int32.Parse(txtWidthValue.Current.Value);
            int height = Int32.Parse(txtHeightValue.Current.Value);

            Assert.AreEqual(DEFAULT_WIDTH, width);
            Assert.AreEqual(DEFAULT_HEIGHT, height);
        }

        private void LoadImage(string path, string imageFile)
        {
            //click file
            ClickFileMenuItem();
            //click load image
            AutomationElement loadImageItem = GetElement("menuItemLoadImage");
            InvokePattern loadImgPattern = (InvokePattern)loadImageItem.GetCurrentPattern(InvokePattern.Pattern);
            loadImgPattern.Invoke();
            //get file select window - located as child of app window but no automationId, so have to use name
            AutomationElement fileOpenWindow = null;

            //might take a bit so need to loop
            int numWaits = 0;
            do
            {
                Console.WriteLine("Looking for file open dialog...");
                fileOpenWindow = mainWindow.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Open"));
                ++numWaits;
                Thread.Sleep(200);
            } while (null == fileOpenWindow);
            if (null == fileOpenWindow)
            {
                throw new Exception("Could not find file open window");
            }
            else
            {
                Console.WriteLine("Found file open window");
            }

            //get textbox for file
            AutomationElement txtFileSelect = null;

            AutomationElementCollection elems = fileOpenWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "File name:"));
            foreach (AutomationElement elem in elems)
            {
                //check class name
                if (elem.Current.ClassName == "Edit")
                {
                    txtFileSelect = elem;
                }
            }

            Assert.IsNotNull(txtFileSelect);

            //get pattern
            ValuePattern txtValue = (ValuePattern)txtFileSelect.GetCurrentPattern(ValuePattern.Pattern);
            //set it for path
            Console.WriteLine("Loading file: " + path + imageFile);
            txtValue.SetValue(path + imageFile);

            //get OK button 
            AutomationElement openButton = null;
            openButton = fileOpenWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "1"));
            Assert.IsNotNull(openButton);
            //click it
            InvokePattern openButtonPattern = (InvokePattern)openButton.GetCurrentPattern(InvokePattern.Pattern);
            openButtonPattern.Invoke();
        }


        /// <summary>
        /// Helper function for finding elements in window
        /// </summary>
        /// <param name="automationIdProperty"></param>
        /// <returns></returns>
        private AutomationElement GetElement(string automationIdProperty)
        {
            AutomationElement result = null;
            result = mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationIdProperty));
            if (null == result)
            {
                throw new Exception("Could not find element: " + automationIdProperty);
            }
            else
            {
                Console.WriteLine("Found element: " + automationIdProperty);
            }
            return result;
        }

        private void StartProgram()
        {
            Console.WriteLine("Starting app");
            programProcess = Process.Start(PROJECT_PATH + @"bin\Debug\AtlasEngine.exe");

            if (null == programProcess)
            {
                throw new Exception("Failed to find AtlasEngine.exe");
            }
            else
            {
                Console.WriteLine("Found AtlasEngine.exe");
            }

            VerifyProcessRunning();
        }

        private void VerifyProcessRunning()
        {
            int ct = 0;
            do
            {
                Console.WriteLine("Looking for AtlasEngine process...");
                ++ct;
                Thread.Sleep(100);
            } while (null == programProcess && ct < 50);
        }

        private void FindDesktopElement()
        {
            Console.WriteLine("Getting desktop element.");
            desktop = AutomationElement.RootElement;
            if (null == desktop)
            {
                throw new Exception("Unable to get desktop.");
            }
            else
            {
                Console.WriteLine("Found desktop");
            }
        }

        private void FindMainWindow()
        {
            int numWaits = 0;
            do
            {
                Console.WriteLine("Looking for AtlasEngine window...");
                mainWindow = desktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Atlas Engine"));
                ++numWaits;
                Thread.Sleep(200);
            } while (null == mainWindow && numWaits < 50);

            if (null == mainWindow)
            {
                throw new Exception("Failed to find Atlas Engine main window.");
            }
            else
            {
                Console.WriteLine("Found main window.");
            }
        }

        private void ClickFileMenuItem()
        {
            fileMenuItem = GetElement("menuItemFile");
            Console.WriteLine("Clicking file on menu");
            ExpandCollapsePattern clickFile = (ExpandCollapsePattern)fileMenuItem.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            clickFile.Expand();
        }

        private void ClickFile_Exit()
        {
            ClickFileMenuItem();
            Console.WriteLine("Clicking File-Exit");
            AutomationElement fileExit = GetElement("menuItemExitProgram");
            InvokePattern clickFileExit = (InvokePattern)fileExit.GetCurrentPattern(InvokePattern.Pattern);
            clickFileExit.Invoke();
        }
    }
}
