using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtlasEngine;
using System.Xml;

namespace Unit_Tests
{
    [TestClass]
    public class SpriteSheetTest
    {
        PrivateObject sheet;

        [TestInitialize]
        public void TestInit()
        {
            sheet = new PrivateObject(typeof(SpriteSheet));
        }

        [TestMethod]
        public void FileExtensionConvertTest()
        {
            string result = sheet.Invoke("XmlToPngFile", "ABC.XYZ") as string;
            Assert.AreEqual("ABC.png", result);

            result = sheet.Invoke("XmlToPngFile", @"Foo\ABC.XYZ") as string;
            Assert.AreEqual(@"Foo\ABC.png", result);

        }

        [TestMethod]
        public void PathParseTest()
        {
            string filePath =  @"ABC\DEF\GHI.XYZ";
           Object[] args = new Object[] {filePath, "", ""};
            sheet.Invoke("ParseFilePath", args);
            Assert.AreEqual(@"ABC\DEF\", args[1]);
            Assert.AreEqual("GHI.XYZ", args[2]);
        }

    }
}
