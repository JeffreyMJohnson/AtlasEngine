using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AtlasEngine
{
    /// <summary>
    /// Class representing atlas map file in XML format.
    /// </summary>
    public class AtlasDocument
    {
        XmlDocument mDocument;
        XmlNode mRootNode;

        /*constructor*/
        /// <summary>
        /// Default connstructor
        /// </summary>
        public AtlasDocument()
        {
            mDocument = new XmlDocument();
            mRootNode = mDocument.CreateElement("SpriteSheet");
            mDocument.AppendChild(mRootNode);
        }


        /// <summary>
        /// Constructor that sets atlas doc width and height from given values.
        /// </summary>
        /// <param name="width">Width or atlas map document.</param>
        /// <param name="height">Height of atlas map document.</param>
        public AtlasDocument(string width, string height) : this()
        {
            SheetWidth = width;
            SheetHeight = height;

        }

        /*properties*/
        /// <summary>
        /// Returns the XMLDocument object of the atlas file.\n
        /// *NOTE: This property is read-only.*
        /// </summary>
        public XmlDocument XMLDoc
        {
            get { return mDocument; }
        }

        /// <summary>
        /// Returns the root XMLNode of the atlas file.\n
        /// *NOTE: This property is read-only.*
        /// </summary>
        public XmlNode RootNode
        {
            get { return mRootNode; }
        }

        /// <summary>
        /// Sets and returns the width of the atlas map document.
        /// </summary>
        public string SheetWidth
        {
            get
            {
                if (mRootNode != null)
                {
                    XmlNode widthAttribute = mRootNode.Attributes.GetNamedItem("width");
                    if (widthAttribute != null)
                    {
                        return widthAttribute.Value;
                    }
                }
                return "";
            }
            set
            {
                if (mRootNode != null)
                {
                    //check if already exists
                    XmlNode widthAttribute = mRootNode.Attributes.GetNamedItem("width");
                    if (widthAttribute == null)
                    {
                        //create it
                        XmlAttribute newAtt = mDocument.CreateAttribute("width");
                        newAtt.Value = value;
                        mRootNode.Attributes.Append(newAtt);
                    }
                    else
                    {
                        widthAttribute.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Sets and returns the height of atlas map document.
        /// </summary>
        public string SheetHeight
        {
            get
            {
                if (mRootNode != null)
                {
                    XmlNode heightAttribute = mRootNode.Attributes.GetNamedItem("height");
                    if (heightAttribute != null)
                    {
                        return heightAttribute.Value;
                    }
                }
                return "";
            }
            set
            {
                if (mRootNode != null)
                {
                    //check if already exists
                    XmlNode heightAttribute = mRootNode.Attributes.GetNamedItem("height");
                    if (heightAttribute == null)
                    {
                        //create it
                        XmlAttribute newAtt = mDocument.CreateAttribute("height");
                        newAtt.Value = value;
                        mRootNode.Attributes.Append(newAtt);
                    }
                    else
                    {
                        heightAttribute.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Path and filename to the produced atlas map document file.
        /// </summary>
        public string SheetPath
        {
            get
            {
                XmlAttribute pathAtt =  mRootNode.Attributes.GetNamedItem("filePath") as XmlAttribute;
                if(pathAtt != null)
                {
                    return pathAtt.Value;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                XmlAttribute pathAtt = mRootNode.Attributes.GetNamedItem("filePath") as XmlAttribute;
                if(pathAtt == null)
                {
                    pathAtt = mDocument.CreateAttribute("filePath");
                }
                pathAtt.Value = value;
                mRootNode.Attributes.Append(pathAtt);
            }
        }

        /*public methods*/
        /// <summary>
        /// Add a new sprite to the atlas map document.
        /// </summary>
        /// <param name="id">ID of the sprite.</param>
        /// <param name="x">The mimimum x value (Left) of the sprite in the produced atlas map document.</param>
        /// <param name="y">The mimimum y value (Top) of the sprite in the produced atlas map document.</param>
        /// <param name="width">Width of the sprite.</param>
        /// <param name="height">Height of the sprite.</param>
        public void AddSprite(string id, string x, string y, string width, string height)
        {
            //check if group parent is there yet
            XmlElement groupNode = mRootNode.FirstChild as XmlElement;
            if(groupNode == null)
            {
                //make it
                groupNode = mDocument.CreateElement("group");
                mRootNode.AppendChild(groupNode);
            }

            XmlElement newNode = mDocument.CreateElement("sprite");
            XmlAttribute idAtt = mDocument.CreateAttribute("id");
            idAtt.Value = id;
            newNode.Attributes.Append(idAtt);

            XmlAttribute xAtt = mDocument.CreateAttribute("x");
            xAtt.Value = x;
            newNode.Attributes.Append(xAtt);

            XmlAttribute yAtt = mDocument.CreateAttribute("y");
            yAtt.Value = y;
            newNode.Attributes.Append(yAtt);

            XmlAttribute widthAtt = mDocument.CreateAttribute("width");
            widthAtt.Value = width;
            newNode.Attributes.Append(widthAtt);

            XmlAttribute heightAtt = mDocument.CreateAttribute("height");
            heightAtt.Value = height;
            newNode.Attributes.Append(heightAtt);

            groupNode.AppendChild(newNode);

        }

        /// <summary>
        /// Returns sprite XMLNode element that matches the given id or null if no ID that matches is found.
        /// </summary>
        /// <param name="id">Id of the sprite to return.</param>
        /// <returns>Sprite XMLNode element that matches givven ID, else null if no match found.</returns>
        public XmlNode GetSpriteNodeById(string id)
        {
            XmlNode groupNode = mRootNode.FirstChild;
            XmlNode result = null;
            foreach(XmlNode node in groupNode.ChildNodes)
            {
                if(node.Attributes.GetNamedItem("id").Value == id)
                {
                    result = node;
                }
            }
            return result;
        }

        /// <summary>
        /// Save the atlas map document with the given path and filename.
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            mDocument.Save(path);
        }

        /// <summary>
        /// Clear the current atlas map document and set back to default.
        /// </summary>
        public void Clear()
        {
            XmlNode groupNode = RootNode.FirstChild;
            if (null != groupNode)
            {
                groupNode.RemoveAll();
            }
            
        }
    }
}
