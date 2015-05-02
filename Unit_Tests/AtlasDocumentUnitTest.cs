using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtlasEngine;
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

        public AtlasDocumentUnitTest()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
            mAtlasDocClass = new PrivateObject(typeof(AtlasDocument));
        }

        [TestMethod]
        public void ConstructorTest()
        {
           //verify document and root are not null
            XmlDocument doc = mAtlasDocClass.GetField("mDocument") as XmlDocument;
            Assert.IsNotNull(doc);
            Assert.IsNotNull(mAtlasDocClass.GetProperty("RootNode"));

            //verify doc has rootnode
            Assert.AreEqual(1, doc.ChildNodes.Count);
        }

        [TestMethod]
        public void WidthPropTest_NotSet()
        {
            string result = mAtlasDocClass.GetProperty("SheetWidth") as string;
            Assert.AreEqual("", result);

            mAtlasDocClass.SetProperty("SheetWidth", "100");
            result = mAtlasDocClass.GetProperty("SheetWidth") as string;
            Assert.AreEqual("100", result);

            //verify xml
            result = GetRootNodeAttribute("width");
            Assert.IsNotNull(result);
            Assert.AreEqual("100", result);

        }

        [TestMethod]
        public void WidthPropTest_Set()
        {
            //verify the attribute is set when already there
            mAtlasDocClass.SetProperty("SheetWidth", "100");
            mAtlasDocClass.SetProperty("SheetWidth", "200");

            //check variable
            Assert.AreEqual("200", mAtlasDocClass.GetFieldOrProperty("SheetWidth") as string);

            //verify xml
            string result = GetRootNodeAttribute("width");
            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }



        [TestMethod]
        public void HeightPropTest_NotSet()
        {
            string result = mAtlasDocClass.GetProperty("SheetHeight") as string;
            Assert.AreEqual("", result);

            mAtlasDocClass.SetProperty("SheetHeight", "100");
            result = mAtlasDocClass.GetProperty("SheetHeight") as string;
            Assert.AreEqual("100", result);

            //verify xml
            result = GetRootNodeAttribute("height");
            Assert.IsNotNull(result);
            Assert.AreEqual("100", result);

        }

        [TestMethod]
        public void HeightPropTest_Set()
        {
            //verify the attribute is set when already there
            mAtlasDocClass.SetProperty("SheetHeight", "100");
            mAtlasDocClass.SetProperty("SheetHeight", "200");

            //check variable
            Assert.AreEqual("200", mAtlasDocClass.GetFieldOrProperty("SheetHeight") as string);

            //verify xml
            string result = GetRootNodeAttribute("height");
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

            mAtlasDocClass.Invoke("AddSprite", id, x, y, width, height);

            XmlNode spriteNode = mAtlasDocClass.Invoke("GetSpriteNodeById", id) as XmlNode;
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
            mAtlasDocClass.Invoke("AddSprite", id, x, y, width, height);

            id = "20";
            x = "100";
            y = "100";
            width = "200";
            height = "200";
            mAtlasDocClass.Invoke("AddSprite", id, x, y, width, height);

            id = "30";
            x = "300";
            y = "300";
            width = "100";
            height = "100";
            mAtlasDocClass.Invoke("AddSprite", id, x, y, width, height);

            //verify second
            XmlNode spriteNode = mAtlasDocClass.Invoke("GetSpriteNodeById", "20") as XmlNode;
            Assert.IsNotNull(spriteNode);

            Assert.AreEqual("20", spriteNode.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual("100", spriteNode.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual("100", spriteNode.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual("200", spriteNode.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual("200", spriteNode.Attributes.GetNamedItem("height").Value);

            spriteNode = mAtlasDocClass.Invoke("GetSpriteNodeById", id) as XmlNode;
            Assert.IsNotNull(spriteNode);

            Assert.AreEqual(id, spriteNode.Attributes.GetNamedItem("id").Value);
            Assert.AreEqual(x, spriteNode.Attributes.GetNamedItem("x").Value);
            Assert.AreEqual(y, spriteNode.Attributes.GetNamedItem("y").Value);
            Assert.AreEqual(width, spriteNode.Attributes.GetNamedItem("width").Value);
            Assert.AreEqual(height, spriteNode.Attributes.GetNamedItem("height").Value);


        }

        private string GetRootNodeAttribute(string attributeName)
        {
            XmlNode rootNode = mAtlasDocClass.GetProperty("RootNode") as XmlNode;
            XmlNode att = rootNode.Attributes.GetNamedItem(attributeName);
            if (att == null)
                return null;
            else
                return att.Value;
        }
    }
}
