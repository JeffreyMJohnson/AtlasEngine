using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AtlasEngine
{
    public class AtlasDocument
    {
        XmlDocument mDocument;
        XmlNode mRootNode;

        /*constructor*/
        public AtlasDocument()
        {
            mDocument = new XmlDocument();
            mRootNode = mDocument.CreateElement("SpriteSheet");
            mDocument.AppendChild(mRootNode);
        }

        public AtlasDocument(string width, string height) : this()
        {
            SheetWidth = width;
            SheetHeight = height;

        }

        /*properties*/
        public XmlNode RootNode
        {
            get { return mRootNode; }
        }

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
            }
        }

        /*public methods*/
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

        public void Save(string path)
        {
            mDocument.Save(path);
        }
    }
}
