using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;

namespace Unit_Tests
{
    [TestClass]
    public class UI_Testing
    {
        private int DEFAULT_WIDTH = 256;
        private int DEFAULT_HEIGHT = 256;
        private string PROJECT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GitHub\AtlasEngine\AtlasEngine\";
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
        /// Helper function for finding elements in window
        /// </summary>
        /// <param name="automationIdProperty"></param>
        /// <returns></returns>
        private AutomationElement GetElement(string automationIdProperty)
        {
            return mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationIdProperty));
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
            fileMenuItem = mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "menuItemFile"));
            if (null == fileMenuItem)
            {
                throw new Exception("Could not find file menu control.");
            }
            else
            {
                Console.WriteLine("File menu control found.");
            }
            Console.WriteLine("Clicking file on menu");
            ExpandCollapsePattern clickFile = (ExpandCollapsePattern)fileMenuItem.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            clickFile.Expand();
        }

        private void ClickFile_Exit()
        {
            ClickFileMenuItem();
            Console.WriteLine("Clicking File-Exit");
            AutomationElement fileExit = null;
            fileExit = mainWindow.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Exit Program"));
            if (null == fileExit)
            {
                throw new Exception("File-Exit not found.");
            }
            else
            {
                Console.WriteLine("File-Exit control found.");
            }

            InvokePattern clickFileExit = (InvokePattern)fileExit.GetCurrentPattern(InvokePattern.Pattern);
            clickFileExit.Invoke();
        }
    }
}
