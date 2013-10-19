using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Ricordanza.Security
{
    /// <summary>
    /// 公開鍵クラスです。
    /// </summary>
    public class PublicKey
    {
        #region const

        #endregion

        #region private variable

        /// <summary>
        /// エンコード
        /// </summary>
        private readonly Encoding _encode;

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public PublicKey()
            : base()
        {
            _encode = Encoding.UTF8;
            CreateDate = DateTime.Today;
            RSAProvider = new RSACryptoServiceProvider();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input">公開鍵ファイルの<see cref="System.IO.Stream"/></param>
        public PublicKey(Stream input)
            : this()
        {
            Load(input);
        }

        #endregion

        #region property

        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateDate { protected set; get; }

        /// <summary>
        /// プロダクト名
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        /// 読みとり専用
        /// </summary>
        internal bool ReadOnly { private get; set; }

        /// <summary>
        /// プロバイダー
        /// </summary>
        internal RSACryptoServiceProvider RSAProvider { set; get; }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 暗号化を行います。
        /// </summary>
        /// <param name="data">暗号化を行いたい文字列</param>
        /// <returns>暗号化文字列</returns>
        public virtual string Encrypt(string data)
        {
            return Convert.ToBase64String(RSAProvider.Encrypt(_encode.GetBytes(data), false));
        }

        /// <summary>
        /// 公開鍵を保存します。
        /// </summary>
        /// <param name="output">公開鍵ファイルを保存する<see cref="System.IO.Stream"/></param>
        public virtual void Save(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output is null.");

            string s = RSAProvider.ToXmlString(false);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = _encode,
                Indent = true,
                IndentChars = "  "
            };

            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("publicKey");
                writer.WriteStartElement("productName");
                writer.WriteValue(ProductName);
                writer.WriteEndElement();
                writer.WriteStartElement("createDate");
                writer.WriteValue(CreateDate.ToString("yyyy-MM-dd"));
                writer.WriteEndElement();
                writer.WriteStartElement("keyValue");

                using (StringReader reader = new StringReader(s))
                {
                    using (XmlReader reader2 = XmlReader.Create(reader))
                    {
                        while (reader2.Read())
                        {
                            switch (reader2.NodeType)
                            {
                                case XmlNodeType.Element:
                                    writer.WriteStartElement(reader2.Name);
                                    if (reader2.IsEmptyElement)
                                        writer.WriteEndElement();
                                    continue;
                                case XmlNodeType.Attribute:
                                    continue;
                                case XmlNodeType.Text:
                                    writer.WriteValue(reader2.Value);
                                    continue;
                                case XmlNodeType.CDATA:
                                    writer.WriteCData(reader2.Value);
                                    continue;
                                case XmlNodeType.EndElement:
                                    break;
                                default:
                                    continue;
                            }
                            writer.WriteEndElement();
                        }
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            ReadOnly = true;
        }

        #endregion

        #region protected method

        /// <summary>
        /// 公開鍵ファイルを読み込みます。
        /// </summary>
        /// <param name="input">公開鍵ファイルの<see cref="System.IO.Stream"/></param>
        protected virtual void Load(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input is null.");

            string xmlString = null;
            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            using (XmlReader reader = XmlReader.Create(input, settings))
            {
                if (!reader.IsStartElement("publicKey"))
                    throw new XmlException("format is not supported.");

                if (reader.ReadToFollowing("productName"))
                    ProductName = reader.ReadString();

                if (reader.ReadToFollowing("createDate"))
                    CreateDate = DateTime.ParseExact(reader.ReadString(), "yyyy-MM-dd", null);

                if (reader.ReadToFollowing("keyValue"))
                    xmlString = reader.ReadInnerXml();
            }

            if ((xmlString == null) || (this.ProductName == null))
                throw new XmlException("format is not supported.");

            RSAProvider.FromXmlString(xmlString);
        }

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
