using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ricordanza.Core;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region CoreForm

    /// <summary>
    /// �S�Ă�Form�̋K��N���X�ł��B
    /// </summary>
    public partial class CoreForm : Form
    {
        #region const

        #endregion

        #region private variable

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// �V�����C���X�^���X���\�z���܂��B
        /// </summary>
        public CoreForm()
            : base()
        {
            // ������
            KeyMap = new Dictionary<Keys, IBindKey>();
            IEditables = new List<IEditable>();
            ErrorProvider = new ErrorProvider()
            {
                BlinkRate = 500,
                BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink,
                ContainerControl = this,
                Icon = global::Ricordanza.WinFormsUI.Properties.Resources.Error
            };
        }

        #endregion

        #region property

        /// <summary>
        /// �L�[��<see cref="Ricordanza.WinFormsUI.Forms.IBindKey"/>���֘A�t����A�z�z����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("�L�[��IBindKey���֘A�t����A�z�z����擾�܂��͐ݒ肵�܂��B")]
        protected internal Dictionary<Keys, IBindKey> KeyMap { private set; get; }

        /// <summary>
        ///  <see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>�R���g���[���z����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("IValidatable�R���g���[���z����擾�܂��͐ݒ肵�܂��B")]
        protected internal List<IEditable> IEditables { private set; get; }

        /// <summary>
        /// ���̓`�F�b�N�s�����̃v���o�C�_���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Ricordanza")]
        [Description("���̓`�F�b�N�s�����̃v���o�C�_���擾�܂��͐ݒ肵�܂��B")]
        protected internal ErrorProvider ErrorProvider { private set; get; }

        #endregion

        #region event

        /// <summary>
        /// �E�B���h�E�T�C�Y�ύX�C�x���g�B
        /// </summary>
        [Category("Ricordanza")]
        [Description("�E�B���h�E�T�C�Y�̒l���R���g���[���ŕύX����钼�O�ɔ������܂��B")]
        public event EventHandler<WindowStateChangingEventArgs> WindowStateChanging;

        /// <summary>
        /// �E�B���h�E�T�C�Y�ύX��C�x���g�B
        /// </summary>
        [Category("Ricordanza")]
        [Description("�E�B���h�E�T�C�Y�̒l���R���g���[���ŕύX����钼��ɔ������܂��B")]
        public event EventHandler<WindowStateChangedEventArgs> WindowStateChanged;

        #endregion

        #region event method

        /// <summary>
        /// Shown�C�x���g�𔭐������܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���i�[���Ă���<see cref="System.EventArgs"/>�B</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Load�ł͋N�����x���Ȃ�(Form�̕\�����x���Ȃ��)Form�\����ɏ��������s���B
            InitForm();
        }

        /// <summary>
        /// KeyDown�C�x���g�𔭐������܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���i�[���Ă���<see cref="System.Windows.Forms.KeyEventArgs"/>�B</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // �������ꂽ�L�[�̍\�z
            Keys key = e.Modifiers | e.KeyCode;

            // �������ꂽ�L�[�ɑΉ�����IBindKey�����݂���ꍇ�͏������s
            if (KeyMap.ContainsKey(key))
            {
                IBindKey ib = KeyMap[key] as IBindKey;

                // IBindKey���L���ȏꍇ�͎��s
                if (ib.Effective)
                {
                    ib.KeyHook();
                    e.Handled = true;
                }
            }

            // ���n���h���̏ꍇ�͏�ʂɃC�x���g��ʒm
            if (!e.Handled)
                base.OnKeyDown(e);
        }

        /// <summary>
        /// WindowStateChanging�C�x���g�𔭐������܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���i�[���Ă���<see cref="Ricordanza.WinFormsUI.Forms.WindowStateChangingEventArgs"/>�B</param>
        /// <returns>�E�B���h�E�̕ύX�����e����ꍇ��true�B����ȊO�̏ꍇ��false�B</returns>
        protected virtual bool OnWindowStateChanging(WindowStateChangingEventArgs e)
        {
            if (WindowStateChanging != null)
                WindowStateChanging(this, e);

            return e.Cancel;
        }

        /// <summary>
        /// WindowStateChanged�C�x���g�𔭐������܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���i�[���Ă���<see cref="Ricordanza.WinFormsUI.Forms.WindowStateChangedEventArgs"/>�B</param>
        protected virtual void OnWindowStateChanged(WindowStateChangedEventArgs e)
        {
            if (WindowStateChanged != null)
                WindowStateChanged(this, e);
        }

        #endregion

        #region public method

        #endregion

        #region protected method

        /// <summary>
        /// Windows���b�Z�[�W���������܂��B
        /// </summary>
        /// <param name="m">�����Ώۂ�<see cref="System.Windows.Forms.Message"/></param>
        [System.Security.Permissions.SecurityPermission(
        System.Security.Permissions.SecurityAction.LinkDemand,
        Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WindowMessages.WM_SYSCOMMAND)
            {
                int wparam = m.WParam.ToInt32() & 0xfff0;
                switch (wparam)
                {
                    case WindowMessages.SC_MINIMIZE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Minimized)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Minimized));
                        return;

                    case WindowMessages.SC_MAXIMIZE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Maximized)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Maximized));
                        return;

                    case WindowMessages.SC_RESTORE:
                        if (this.OnWindowStateChanging(new WindowStateChangingEventArgs(FormWindowState.Normal)))
                            return;
                        base.WndProc(ref m);
                        this.OnWindowStateChanged(new WindowStateChangedEventArgs(FormWindowState.Normal));
                        return;
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// �t�H�[���̋��ʏ������������s���܂��B
        /// </summary>
        protected virtual void InitForm()
        {
            // �L�[�t�b�N�p�̃}�b�v�쐬
            KeyMap.Clear();
            FindIBindKey(this).ToList().ForEach(ib =>
            {
                // �L�[�����Ȃ��̏ꍇ
                if (ib.BindKey == Keys.None)
                    return;

                // �L�[�̊��蓖�Ă̗D�揇�ʂ͏o�����Ƃ���
                if (!KeyMap.ContainsKey(ib.BindKey))
                    KeyMap[ib.BindKey] = ib;
            });

            // �L�[�t�b�N�̒�`���f�ނ���ꍇ�̓L�[���t�b�N�ł���悤�ɂ���B
            if ( KeyMap.Count > 0)
                this.KeyPreview = true;

            // ���̓C���^�[�t�F�[�X��ErrorProvider��ݒ�
            IEditables.Clear();
            IEditables.AddRange(FindIEditable(this));
            IEditables.ForEach(ie => ie.ErrorProvider = ErrorProvider);
        }

        /// <summary>
        /// �S�Ă�<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>���ۗL����G���[�v���o�C�_�[�̏�Ԃ������l�ɖ߂��܂��B
        /// </summary>
        protected virtual void ClearError()
        {
            IEditables.ForEach(ie => ie.ClearError());
        }

        /// <summary>
        /// �S�Ă�<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>�ɑ΂���<see cref="Ricordanza.WinFormsUI.Forms.IEditable.Clear"/>�����s���܂��B
        /// </summary>
        protected virtual void ClearAll()
        {
            IEditables.ForEach(ie => ie.Clear());
        }

        /// <summary>
        /// �S�Ă�<see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>�ɑ΂��ē��̓`�F�b�N�����s���܂��B
        /// </summary>
        /// <returns>����NG�̍��ڂ����݂���ꍇ��true�A����ȊO�̏ꍇ��false�B</returns>
        protected virtual bool ValidateAll()
        {
            bool returnVal = false;
            IEditables.ForEach(
                ie =>
                {
                    bool ret = ie.Validate();
                    if (!returnVal)
                        returnVal = ret;
                });
            return returnVal;
        }

        #endregion

        #region private method

        /// <summary>
        /// <see cref="Ricordanza.WinFormsUI.Forms.IEditable"/>�R���g���[�������o���܂��B
        /// </summary>
        /// <param name="control">�e�ɂ�����R���g���[���B</param>
        /// <returns>�擾�ΏۃR���g���[���z��B</returns>
        private IEditable[] FindIEditable(Control control)
        {
            List<IEditable> buf = new List<IEditable>();
            foreach (Control c in control.Controls)
            {
                IEditable iv = c as IEditable;
                if (iv != null)
                    buf.Add(iv);

                buf.AddRange(FindIEditable(c));
            }
            return buf.ToArray();
        }

        /// <summary>
        /// <see cref="Ricordanza.WinFormsUI.Forms.IBindKey"/>�R���g���[�������o���܂��B
        /// </summary>
        /// <param name="control">�e�ɂ�����R���g���[���B</param>
        /// <returns>�擾�ΏۃR���g���[���z��B</returns>
        private IBindKey[] FindIBindKey(Control control)
        {
            List<IBindKey> buf = new List<IBindKey>();
            foreach (Control c in control.Controls)
            {
                IBindKey iv = c as IBindKey;
                if (iv != null)
                    buf.Add(iv);

                buf.AddRange(FindIBindKey(c));
            }
            return buf.ToArray();
        }

        /// <summary>
        /// �R���|�[�l���g�����������܂��B
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CoreForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "CoreForm";
            this.ResumeLayout(false);
        }

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region WindowStateChangingEventArgs

    /// <summary>
    /// WindowStateChanging �C�x���g�̃f�[�^��񋟂��܂��B
    /// </summary>
    public class WindowStateChangingEventArgs : CancelEventArgs
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// �V�����E�B���h�E��Ԃ��擾���܂��B
        /// </summary>
        public FormWindowState WindowState { private set; get; }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// ���̃N���X�̐V�����C���X�^���X���擾���܂��B
        /// </summary>
        /// <param name="windowState">�t�H�[���E�B���h�E�̕\���T�C�Y</param>
        public WindowStateChangingEventArgs(FormWindowState windowState)
        {
            WindowState = windowState;
        }

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion

    #region WindowStateChangedEventArgs

    /// <summary>
    /// WindowStateChanged �C�x���g�̃f�[�^��񋟂��܂��B
    /// </summary>
    public class WindowStateChangedEventArgs : EventArgs
    {
        #region const

        #endregion

        #region protected variable

        #endregion

        #region property

        /// <summary>
        /// �V�����E�B���h�E��Ԃ��擾���܂��B
        /// </summary>
        public FormWindowState WindowState { private set; get; }

        #endregion

        #region static constructor

        #endregion

        #region constructor

        /// <summary>
        /// ���̃N���X�̐V�����C���X�^���X���擾���܂��B
        /// </summary>
        /// <param name="windowState">�t�H�[���E�B���h�E�̕\���T�C�Y</param>
        public WindowStateChangedEventArgs(FormWindowState windowState)
        {
            WindowState = windowState;
        }

        #endregion

        #region event method

        #endregion

        #region public method

        #endregion

        #region protected method

        #endregion

        #region private method

        #endregion

        #region delegate

        #endregion
    }

    #endregion
}
