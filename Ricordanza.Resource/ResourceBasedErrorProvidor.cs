using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ricordanza.Resource
{
    /// <summary>
    /// リソースベースのErrorProviderです。
    /// </summary>
    public partial class ResourceBasedErrorProvider
        : ErrorProvider
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public ResourceBasedErrorProvider(IContainer container)
            : base(container)
        {
            Keys = new HashSet<Control>();
        }

        #endregion

        #region property

        /// <summary>
        /// 例外通知中のコントロールセット
        /// </summary>
        protected HashSet<Control> Keys { set; get; }

        /// <summary>
        /// 例外を保持しているか
        /// </summary>
        public bool HasError { get { return Keys.Count > 0; } }

        #endregion

        #region event

        #endregion

        #region event handler

        #endregion

        #region event method

        #endregion

        #region public method

        /// <summary>
        /// 例外を通知します。
        /// </summary>
        /// <param name="con">例外通知をしたいコントロール</param>
        /// <param name="resourceKey">対象のリソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        public virtual void RaiseError(Control con, string resourceKey, params string[] paramters)
        {
            RaiseError(con, ErrorIconAlignment.MiddleRight, resourceKey, paramters);
        }

        /// <summary>
        /// 例外を通知します。
        /// </summary>
        /// <param name="con">例外通知をしたいコントロール</param>
        /// <param name="resourceKey">対象のリソースキー</param>
        /// <param name="paramters">プレースフォルダを置換するパラメータ</param>
        public virtual void RaiseError(Control con, ErrorIconAlignment alignment, string resourceKey, params string[] paramters)
        {
            this.SetIconAlignment(con, alignment);
            this.SetError(con, ResourceManager.Get(resourceKey, paramters));
            if (!Keys.Contains(con))
                Keys.Add(con);
        }

        /// <summary>
        /// 例外の通知を解除します。
        /// </summary>
        /// <param name="con">例外通知を解除したいコントロール</param>
        public virtual void ClearError(Control con)
        {
            this.SetError(con, string.Empty);
            if (Keys.Contains(con))
                Keys.Remove(con);
        }

        /// <summary>
        /// 全ての例外の通知を解除します。
        /// </summary>
        public virtual void ClearErrorAll()
        {
            Keys.ToList().ForEach(c => this.SetError(c, string.Empty));
            Keys.Clear();
        }

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }
}
