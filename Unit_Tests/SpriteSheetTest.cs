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
        public void InitAtlasTest()
        {
           
            sheet.SetProperty("Width", 100);
            sheet.SetProperty("Height", 100);
            sheet.Invoke("InitAtlasDoc");


            XmlDocument xmlDoc = sheet.GetProperty("AtlasDoc") as XmlDocument;

            //verify rootnode
            XmlNode root = xmlDoc.FirstChild;
            Assert.AreEqual("SpriteSheet", root.Name);


            //verify attributes
            XmlAttributeCollection attributes = root.Attributes;
            Assert.AreEqual(4, attributes.Count);

            //width
            XmlAttribute att = attributes.GetNamedItem("width") as XmlAttribute;
            Assert.IsNotNull(att);
            Assert.AreEqual("100", att.Value);

            //height
            att = attributes.GetNamedItem("height") as XmlAttribute;
            Assert.IsNotNull(att);
            Assert.AreEqual("100", att.Value);

            //verify single group child of root

        }

        [TestMethod]
        public void FileConvertTest()
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
