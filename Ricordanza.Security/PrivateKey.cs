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
    /// 秘密鍵クラスです。
    /// </summary>
    public class PrivateKey
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
        public PrivateKey()
            : base()
        {
            _encode = Encoding.UTF8;
            CreateDate = DateTime.Today;
            RSAProvider = new RSACryptoServiceProvider();
        }

        /// <summary>
        /// 秘密鍵クラスのインスタンスを構築します。
        /// </summary>
        /// <param name="publicKey">公開鍵クラス</param>
        public PrivateKey(PublicKey publicKey)
            : this()
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey is null.");

            RSAProvider = publicKey.RSAProvider;
            CreateDate = publicKey.CreateDate;
            ProductName = publicKey.ProductName;
            publicKey.ReadOnly = true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="input">秘密鍵ファイルの<see cref="System.IO.Stream"/></param>
        public PrivateKey(Stream input)
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
        public string ProductName { protected set; get; }

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
        /// 復号化を行います。
        /// </summary>
        /// <param name="data">復号化を行いたい文字列</param>
        /// <returns>復号化文字列</returns>
        public virtual string Decrypt(string data)
        {
            return _encode.GetString(RSAProvider.Decrypt(Convert.FromBase64String(data), false));
        }

        /// <summary>
        /// 秘密鍵を保存します。
        /// </summary>
        /// <param name="output">秘密鍵ファイルを保存する<see cref="System.IO.Stream"/></param>
        public virtual void Save(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output is null.");

            string s = RSAProvider.ToXmlString(true);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = _encode,
                Indent = true,
                IndentChars = "  "
            };

            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("privateKey");
                writer.WriteStartElement("productName");
                writer.WriteValue(ProductName);
                writer.WriteEndElement();
                writer.WriteStartElement("creationDate");
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
        }

        #endregion

        #region protected method

        /// <summary>
        /// 秘密鍵ファイルを読み込みます。
        /// </summary>
        /// <param name="input">秘密鍵ファイルの<see cref="System.IO.Stream"/></param>
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
                if (!reader.IsStartElement("privateKey"))
                    throw new XmlException("format is not supported.");

                if (reader.ReadToFollowing("productName"))
                    ProductName = reader.ReadString();

                if (reader.ReadToFollowing("creationDate"))
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
