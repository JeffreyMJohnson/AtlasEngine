using AtlasEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace Unit_Tests
{
    /// <summary>
    /// Summary description for AtlasDocumentUnitTest
    /// </summary>
    [TestClass]
    public class AtlasDocumentUnitTest
    {
        PrivateObject mAtlasDocClass;
        AtlasDocument mAtlasDoc;

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void TestInit()
        {
            mAtlasDoc = new AtlasDocument();
            
        }

        [TestMethod]
        public void ConstructorTest()
        {
           //verify document and root are not null
            XmlDocument doc = mAtlasDoc.XMLDoc;
            Assert.IsNotNull(doc);
            Assert.IsNotNull(mAtlasDoc.RootNode);

            //verify doc has rootnode
            Assert.AreEqual(1, doc.ChildNodes.Count);
        }

        [TestMethod]
        public void ConstructorTest_Overload()
        {
            mAtlasDoc = new AtlasDocument("500","500");
            //verify document and root are not null
            XmlDocument doc = mAtlasDoc.XMLDoc;
            Assert.IsNotNull(doc);
            Assert.IsNotNull(mAtlasDoc.RootNode);

            //verify width and height attributes
            Assert.AreEqual("500", mAtlasDoc.SheetWidth);
            Assert.AreEqual("500", mAtlasDoc.SheetHeight);
        }

        [TestMethod]
        public void WidthPropTest_NotSet()
        {
            string result = mAtlasDoc.SheetWidth;
            Assert.AreEqual("", result);

            mAtlasDoc.SheetWidth = "100";
            result = mAtlasDoc.SheetWidth;
            Assert.AreEqual("100", result);

            //verify xml
            result = GetRootNodeAttribute(mAtlasDoc.XMLDoc, "width");
            Assert.IsNotNull(result);
            Assert.AreEqual("100", result);

        }

        [TestMethod]
        public void WidthPropTest_Set()
        {
            //verify the attribute is set when already there
            mAtlasDoc.SheetWidth = "100";
            mAtlasDoc.SheetWidth = "200";

            //check variable
            Assert.AreEqual("200", mAtlasDoc.SheetWidth);

            //verify xml
            string result = GetRootNodeAttribute(mAtlasDoc.XMLDoc, "width");
            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }



        [TestMethod]
        public void HeightPropTest_NotSet()
        {
            string result = mAtlasDoc.SheetHeight;
            Assert.AreEqual("", result);

            mAtlasDoc.SheetHeight = "100";
            result = mAtlasDoc.SheetHeight;
            Assert.AreEqual("100", result);

            //verify xml
            result = GetRootNodeAttribute(mAtlasDoc.XMLDoc, "height");
            Assert.IsNotNull(result);
            Assert.AreEqual("100", result);

        }

        [TestMethod]
        public void HeightPropTest_Set()
        {
            //verify the attribute is set when already there
            mAtlasDoc.SheetHeight = "100";
            mAtlasDoc.SheetHeight = "200";

            //check variable
            Assert.AreEqual("200", mAtlasDoc.SheetHeight);

            //verify xml
            string result = GetRootNodeAttribute(mAtlasDoc.XMLDoc, "height");
            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }

        [TestMethod]
        public void AddSpriteTest_Empty()
        {
            string id = "10";
            string x = "0";
            string y = "0";
            string width = "100";
            string height = "100";

            mAtlasDoc.AddSprite(id, x, y, width, height);

            XmlNode spriteNode = mAtlasDoc.GetSpriteNodeById(id);
            Assert.IsNotNull(spriteNode);

            Assert.AreEqual(id, spriteNode.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual(x, spriteNode.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual(y, spriteNode.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual(width, spriteNode.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual(height, spriteNode.Attributes.GetNamedItem("height").Value);
        }

        [TestMethod]
        public void AddSpriteTest_NotEmpty()
        {
            string id = "10";
            string x = "0";
            string y = "0";
            string width = "100";
            string height = "100";
            mAtlasDoc.AddSprite(id, x, y, width, height);

            id = "20";
            x = "100";
            y = "100";
            width = "200";
            height = "200";
            mAtlasDoc.AddSprite(id, x, y, width, height);

            id = "30";
            x = "300";
            y = "300";
            width = "100";
            height = "100";
            mAtlasDoc.AddSprite(id, x, y, width, height);

            //verify second
            XmlNode spriteNode = mAtlasDoc.GetSpriteNodeById("20");
            Assert.IsNotNull(spriteNode);

            Assert.AreEqual("20", spriteNode.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual("100", spriteNode.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual("100", spriteNode.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual("200", spriteNode.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual("200", spriteNode.Attributes.GetNamedItem("height").Value);

            spriteNode = mAtlasDoc.GetSpriteNodeById(id);
            Assert.IsNotNull(spriteNode);

            Assert.AreEqual(id, spriteNode.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual(x, spriteNode.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual(y, spriteNode.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual(width, spriteNode.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual(height, spriteNode.Attributes.GetNamedItem("height").Value);


        }

        [TestMethod]
        public void SaveDocTest()
        {
            mAtlasDoc.SheetWidth = "500";
            mAtlasDoc.SheetHeight = "200";
            mAtlasDoc.AddSprite("0", "0", "0", "100", "100");
            mAtlasDoc.AddSprite("1", "100", "100", "100", "100");
            mAtlasDoc.AddSprite("2", "0", "100", "100", "100");

            string filePath = Environment.CurrentDirectory + @"\SaveDocTest.xml";

            mAtlasDoc.SheetPath = "SavedDocTest.png";

            mAtlasDoc.Save(filePath);

            XmlDocument savedFile = new XmlDocument();
            savedFile.Load(filePath);


            //verify root attributes
            Assert.AreEqual("500", GetRootNodeAttribute(savedFile, "width"));
            Assert.AreEqual("200", GetRootNodeAttribute(savedFile, "height"));
            Assert.AreEqual("SavedDocTest.png", GetRootNodeAttribute(savedFile, "filePath"));

            //verify 3 sprite nodes
            Assert.AreEqual(3, savedFile.FirstChild.FirstChild.ChildNodes.Count);

            //verify sprite attributes
            XmlElement sprite = savedFile.FirstChild.FirstChild.ChildNodes.Item(0) as XmlElement;
            Assert.AreEqual("0", sprite.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual("0", sprite.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual("0", sprite.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("height").Value);

            sprite = savedFile.FirstChild.FirstChild.ChildNodes.Item(1) as XmlElement;
            Assert.AreEqual("1", sprite.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("height").Value);

            sprite = savedFile.FirstChild.FirstChild.ChildNodes.Item(2) as XmlElement;
            Assert.AreEqual("2", sprite.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual("0", sprite.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual("100", sprite.Attributes.GetNamedItem("height").Value);

        }

        [TestMethod]
        public void ClearAtlasTest()
        {
            mAtlasDoc = new AtlasDocument("500", "500");
            //add some sprite elems
            mAtlasDoc.AddSprite("0", "0", "0", "100", "100");
            mAtlasDoc.AddSprite("1", "0", "100", "100", "100");
            mAtlasDoc.AddSprite("2", "0", "200", "100", "100");

            //call clear
            mAtlasDoc.Clear();
            //verify group has no children
            XmlNode groupNode = mAtlasDoc.RootNode.FirstChild;
            Assert.AreEqual(0, groupNode.ChildNodes.Count);

        }

        private string GetRootNodeAttribute(XmlDocument doc, string attributeName)
        {
            XmlNode rootNode = doc.FirstChild;
            XmlNode att = rootNode.Attributes.GetNamedItem(attributeName);
            if (att == null)
                return null;
            else
                return att.Value;
        }
    }
}
