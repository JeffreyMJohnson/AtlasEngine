using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtlasEngine;
using System.Xml;
using System.Windows.Media;
using System.Windows.Controls;

namespace Unit_Tests
{
    [TestClass]
    public class SpriteSheetTest
    {
        PrivateObject mSheetClass;
        SpriteSheet mSheet;


        [TestInitialize]
        public void TestInit()
        {
            mSheetClass = new PrivateObject(typeof(SpriteSheet));
            mSheet = new SpriteSheet();
        }

        [TestMethod]
        public void HasChangedProperty()
        {
            //check default setting is false
            Assert.AreEqual(false, mSheet.HasChanged);

            mSheet.HasChanged = true;
            Assert.AreEqual(true, mSheet.HasChanged);

            mSheet.HasChanged = false;
            Assert.AreEqual(false, mSheet.HasChanged);
        }

        [TestMethod]
        public void WidthProperty()
        {
            int defaultWidth = (int)mSheetClass.GetField("DEFAULT_IMAGE_WIDTH");

            //verfiy default width
            Assert.AreEqual(defaultWidth, mSheet.Width);
            mSheet.Width = 512;

            Assert.AreEqual(512, mSheet.Width);
        }

        [TestMethod]
        public void HeightProperty()
        {
            int defaultWidth = (int)mSheetClass.GetField("DEFAULT_IMAGE_HEIGHT");

            //verfiy default width
            Assert.AreEqual(defaultWidth, mSheet.Height);
            mSheet.Height = 512;

            Assert.AreEqual(512, mSheet.Height);
        }

        [TestMethod]
        public void AutoResizeProperty()
        {
            //verify set get
            Assert.AreEqual(false, mSheet.AutoResize);

            
            //setting causes coupled WPF stuff so can't unit test
            //mSheet.AutoResize = true;
            //Assert.AreEqual(true, mSheet.AutoResize);

            //mSheet.AutoResize = false;
            //Assert.AreEqual(false, mSheet.AutoResize);
        }

        [TestMethod]
        public void FileExtensionConvertTest()
        {
            string result = mSheetClass.Invoke("XmlToPngFile", "ABC.XYZ") as string;
            Assert.AreEqual("ABC.png", result);

            result = mSheetClass.Invoke("XmlToPngFile", @"Foo\ABC.XYZ") as string;
            Assert.AreEqual(@"Foo\ABC.png", result);

        }

        [TestMethod]
        public void PathParseTest()
        {
            string filePath = @"ABC\DEF\GHI.XYZ";
            Object[] args = new Object[] { filePath, "", "" };
            mSheetClass.Invoke("ParseFilePath", args);
            Assert.AreEqual(@"ABC\DEF\", args[1]);
            Assert.AreEqual("GHI.XYZ", args[2]);
        }
    }
}
