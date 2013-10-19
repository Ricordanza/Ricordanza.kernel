using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Forms.Design;

using Ricordanza.Core.Utilities;
using Ricordanza.WinFormsUI.Utilities;

namespace Ricordanza.WinFormsUI.Forms
{
    #region RoundButton

    /// <summary>
    /// 円形のボタンです。
    /// </summary>
    public class RoundButton : CoreButton
    {
        #region Public Properties
        public override string Text
        {
            get
            {
                return buttonText;
            }
            set
            {
                buttonText = value;
                this.Invalidate();
            }
        }

        private int recessDepth;
        [Bindable(true), Category("Ricordanza"),
        DefaultValue(2),
        Description("ボタンの押下時の深さを取得または設定します。"),
        Editor(typeof(RecessEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public int RecessDepth
        {
            get
            {
                return recessDepth;
            }
            set
            {
                if (value < 0)
                    recessDepth = 0;
                else if (value > 15)
                    recessDepth = 15;
                else
                    recessDepth = value;
                this.Invalidate();
            }
        }

        private int bevelHeight;
        [Bindable(true), Category("Ricordanza"),
        DefaultValue(0),
        Description("ボタンの傾斜の高さを取得または設定します。")]
        public int BevelHeight
        {
            get
            {
                return bevelHeight;
            }
            set
            {
                if (value < 0)
                    bevelHeight = 0;
                else
                    bevelHeight = value;
                this.Invalidate();
            }
        }

        private int bevelDepth;
        [Bindable(true), Category("Ricordanza"),
        DefaultValue(0),
        Description("ボタンの傾斜の深さを取得または設定します。")]
        public int BevelDepth
        {
            get
            {
                return bevelDepth;
            }
            set
            {
                if (value < 0)
                    bevelDepth = 0;
                else
                    bevelDepth = value;
                this.Invalidate();
            }
        }

        private bool dome;
        [Bindable(true), Category("Ricordanza"),
        DefaultValue(false),
        Description("ボタンには半球形の先端があるかどうかを取得または設定します。")]
        public bool Dome
        {
            get
            {
                return dome;
            }
            set
            {
                dome = value;
                this.Invalidate();
            }
        }
        #endregion Public Properties

        /// <summary>
        /// Windows form control derived from Button, to create round and elliptical buttons
        /// </summary>
        public RoundButton()
        {
            this.BackColor = System.Drawing.Color.Red;
            this.BevelDepth = 3;
            this.BevelHeight = 2;
            this.Dome = true;
            this.RecessDepth = 3;
            this.Size = new System.Drawing.Size(70, 40);
            this.UseVisualStyleBackColor = false;
            this.Name = "RoundButton";
            this.DoubleBuffered = true;

            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);

            this.Enter += new System.EventHandler(this.weGotFocus);
            this.Leave += new System.EventHandler(this.weLostFocus);

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
        }

        #region Private properties

        private Color buttonColor;
        private String buttonText;

        private LinearGradientBrush edgeBrush;                          // For painting button edges
        private Blend edgeBlend;                                        // Blend colours for realistic edges
        private Color edgeColor1;                                       // Change if button pressed
        private Color edgeColor2;                                       //
        private int edgeWidth;                                          // Width of button edge    
        private int buttonPressOffset;                                     // Offset when button is pressed
        private float lightAngle = Angle.Up;                            // Alter light angle for "Pressed" effect
        private Color cColor = Color.White;                             // Centre colour for Path Gradient brushed

        private bool gotFocus = false;                                  // Determine if this button has focus
        private Font labelFont;                                         // Font for label text
        private VerticalString vs;                                      // Create vertical text for this buttons
        private SolidBrush labelBrush;									// Brush to paint the text
        private StringFormat labelStrFmt;			 					// Used to align text on the control

        private GraphicsPath bpath;
        private GraphicsPath gpath;

        #endregion

        #region Base draw functions

        protected override void OnPaint(PaintEventArgs e)
        {
            buttonColor = this.BackColor;
            edgeColor1 = ControlPaint.Light(buttonColor);
            edgeColor2 = ControlPaint.Dark(buttonColor);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle buttonRect = this.ClientRectangle;
            edgeWidth = GetEdgeWidth(buttonRect);

            FillBackground(g, buttonRect);

            if (RecessDepth > 0)
            {
                DrawRecess(ref g, ref buttonRect);
            }

            DrawEdges(g, ref buttonRect);

            ShrinkShape(ref g, ref buttonRect, edgeWidth);

            DrawButton(g, buttonRect);

            DrawText(g, buttonRect);

            SetClickableRegion();

        }

        /// <summary>
        /// Fill in the control background
        /// </summary>
        /// <param name="g">Graphics object</param>
        /// <param name="rect">Rectangle defining the button background</param>
        protected void FillBackground(Graphics g, Rectangle rect)
        {
            // Fill in the control with the background colour, to overwrite anything
            // that may have been drawn already.
            Rectangle bgRect = rect;
            bgRect.Inflate(1, 1);
            SolidBrush bgBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
            bgBrush.Color = Parent.BackColor;
            g.FillRectangle(bgBrush, bgRect);
            bgBrush.Dispose();

        }

        /// <summary>
        /// Create the button recess
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="recessRect">Rectangle defining the button recess</param>
        protected virtual void DrawRecess(ref Graphics g, ref Rectangle recessRect)
        {

            LinearGradientBrush recessBrush = new LinearGradientBrush(recessRect,
                                                ControlPaint.Dark(Parent.BackColor),
                                                ControlPaint.LightLight(Parent.BackColor),
                                                GetLightAngle(Angle.Up));
            // Blend colours for realism
            Blend recessBlend = new Blend();
            recessBlend.Positions = new float[] { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
            recessBlend.Factors = new float[] { .2f, .2f, .4f, .4f, 1f, 1f };
            recessBrush.Blend = recessBlend;

            // Using this second smaller rectangle smooths the edges - don't know why...?
            Rectangle rect2 = recessRect;
            ShrinkShape(ref g, ref rect2, 1);
            FillShape(g, recessBrush, rect2);

            ShrinkShape(ref g, ref recessRect, recessDepth);

        }

        /// <summary>
        /// Fill in the button edges
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="edgeRect">Rectangle defining the button edge</param>
        protected virtual void DrawEdges(Graphics g, ref Rectangle edgeRect)
        {
            ShrinkShape(ref g, ref edgeRect, 1);

            Rectangle lgbRect = edgeRect;
            lgbRect.Inflate(1, 1);
            edgeBrush = new LinearGradientBrush(lgbRect,
                                                edgeColor1,
                                                edgeColor2,
                                                GetLightAngle(lightAngle));
            // Blend colours for realism
            edgeBlend = new Blend();
            edgeBlend.Positions = new float[] { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
            edgeBlend.Factors = new float[] { .0f, .0f, .2f, .4f, 1f, 1f };
            edgeBrush.Blend = edgeBlend;
            FillShape(g, edgeBrush, edgeRect);
        }

        /// <summary>
        /// Fill in the main button colour
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="buttonRect">Rectangle defining the button top</param>
        protected virtual void DrawButton(Graphics g, Rectangle buttonRect)
        {
            BuildGraphicsPath(buttonRect);

            PathGradientBrush pgb = new PathGradientBrush(bpath);
            pgb.SurroundColors = new Color[] { buttonColor };

            buttonRect.Offset(buttonPressOffset, buttonPressOffset);

            if (bevelHeight > 0)
            {
                Rectangle lgbRect = buttonRect;
                lgbRect.Inflate(1, 1);

                pgb.CenterPoint = new PointF(buttonRect.X + buttonRect.Width / 8 + buttonPressOffset,
                                             buttonRect.Y + buttonRect.Height / 8 + buttonPressOffset);

                pgb.CenterColor = cColor;
                FillShape(g, pgb, buttonRect);
                ShrinkShape(ref g, ref buttonRect, bevelHeight);
            }

            if (bevelDepth > 0)
            {
                DrawInnerBevel(g, buttonRect, bevelDepth, buttonColor);
                ShrinkShape(ref g, ref buttonRect, bevelDepth);
            }

            pgb.CenterColor = buttonColor;

            if (dome)
            {
                pgb.CenterColor = cColor;
                pgb.CenterPoint = new PointF(buttonRect.X + buttonRect.Width / 8 + buttonPressOffset,
                                             buttonRect.Y + buttonRect.Height / 8 + buttonPressOffset);
            }

            FillShape(g, pgb, buttonRect);

            // If we have focus, draw line around control to indicate this.
            if (gotFocus)
            {
                DrawFocus(g, buttonRect);
            }
        }

        /// <summary>
        /// Write the text on the button
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="textRect">Rectangle defining the button text area</param>
        protected void DrawText(Graphics g, Rectangle textRect)
        {
            labelStrFmt = new StringFormat();
            labelBrush = new SolidBrush(this.ForeColor);
            labelFont = this.Font;			// Get the caller-specified font

            vs = new VerticalString();
            vs.TextSpread = .75;

            // Check for tall button, and write text vertically if necessary
            bool verticalText = false;
            if (textRect.Height > textRect.Width * 2)
            {
                verticalText = true;
            }

            // Convert the text alignment from ContentAlignment to StringAlignment
            labelStrFmt.Alignment = ConvertToHorAlign(this.TextAlign);
            labelStrFmt.LineAlignment = ConvertToVertAlign(this.TextAlign);

            // If horizontal text is not horizontally centred,
            // or vertical text is not vertically centred,
            // shrink the rectangle so that the text doesn't stray outside the ellipse
            if ((!verticalText & (labelStrFmt.LineAlignment != StringAlignment.Center)) |
                (verticalText & (labelStrFmt.Alignment != StringAlignment.Center)))
            {
                textRect.Inflate(-(int)(textRect.Width / 7.5), -(int)(textRect.Height / 7.5));
            }

            textRect.Offset(buttonPressOffset, buttonPressOffset);	// Apply the offset if we've been clicked

            // If button is not enabled, "grey out" the text.
            if (!this.Enabled)
            {
                //Write the white "embossing effect" text at an offset
                textRect.Offset(1, 1);
                labelBrush.Color = ControlPaint.LightLight(buttonColor);
                WriteString(verticalText, g, textRect);

                //Restore original text pos, and set text colour to grey.
                textRect.Offset(-1, -1);
                labelBrush.Color = Color.Gray;
            }

            //Write the text
            WriteString(verticalText, g, textRect);
        }

        #endregion Base draw functions

        #region Overrideable shape-specific methods

        protected virtual void BuildGraphicsPath(Rectangle buttonRect)
        {
            bpath = new GraphicsPath();
            // Adding this second smaller rectangle to the graphics path smooths the edges - don't know why...?
            Rectangle rect2 = new Rectangle(buttonRect.X - 1, buttonRect.Y - 1, buttonRect.Width + 2, buttonRect.Height + 2);
            AddShape(bpath, rect2);
            AddShape(bpath, buttonRect);
        }

        /// <summary>
        /// Create region the same shape as the button. This will respond to the mouse click
        /// </summary>
        protected virtual void SetClickableRegion()
        {
            gpath = new GraphicsPath();
            gpath.AddEllipse(this.ClientRectangle);
            this.Region = new Region(gpath);			// Click only activates on elipse
        }

        /// <summary>
        /// Method to fill the specified rectangle with either LinearGradient or PathGradient
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="brush">Either a PathGradient- or LinearGradientBrush</param>
        /// <param name="rect">The rectangle bounding the ellipse to be filled</param>
        protected virtual void FillShape(Graphics g, Object brush, Rectangle rect)
        {
            if (brush.GetType().ToString() == "System.Drawing.Drawing2D.LinearGradientBrush")
            {
                g.FillEllipse((LinearGradientBrush)brush, rect);
            }
            else if (brush.GetType().ToString() == "System.Drawing.Drawing2D.PathGradientBrush")
            {
                g.FillEllipse((PathGradientBrush)brush, rect);
            }
        }

        /// <summary>
        /// Add an ellipse to the Graphics Path
        /// </summary>
        /// <param name="gpath">The Graphics Path</param>
        /// <param name="rect">The rectangle defining the ellipse to be added</param>
        protected virtual void AddShape(GraphicsPath gpath, Rectangle rect)
        {
            gpath.AddEllipse(rect);
        }

        /// <summary>
        /// Draw the specified shape
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="pen">Pen</param>
        /// <param name="rect">The rectangle bounding the ellipse to be drawn</param>
        protected virtual void DrawShape(Graphics g, Pen pen, Rectangle rect)
        {
            g.DrawEllipse(pen, rect);
        }

        /// <summary>
        /// Shrink the shape 
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="rect">The rectangle defining the shape to be shrunk</param>
        /// <param name="amount">The amount to shrink it by</param>
        protected virtual void ShrinkShape(ref Graphics g, ref Rectangle rect, int amount)
        {
            rect.Inflate(-amount, -amount);
        }

        /// <summary>
        /// Indicate that this button has focus
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="rect">The rectangle defining the button face</param>
        protected virtual void DrawFocus(Graphics g, Rectangle rect)
        {
            rect.Inflate(-2, -2);
            Pen fPen = new Pen(Color.Black);
            fPen.DashStyle = DashStyle.Dot;
            DrawShape(g, fPen, rect);
        }

        protected virtual void DrawInnerBevel(Graphics g, Rectangle rect, int depth, Color buttonColor)
        {

            Color lightColor = ControlPaint.LightLight(buttonColor);
            Color darkColor = ControlPaint.Dark(buttonColor);
            Blend bevelBlend = new Blend();
            bevelBlend.Positions = new float[] { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
            bevelBlend.Factors = new float[] { .2f, .4f, .6f, .6f, 1f, 1f };
            Rectangle lgbRect = rect;
            lgbRect.Inflate(1, 1);
            LinearGradientBrush innerBevelBrush = new LinearGradientBrush(lgbRect,
                                                darkColor,
                                                lightColor,
                                                GetLightAngle(Angle.Up));

            innerBevelBrush.Blend = bevelBlend;
            FillShape(g, innerBevelBrush, rect);

        }
        #endregion Overrideable shape-specific methods

        #region Help functions

        /// <summary>
        /// Calculate light angle depending on proportions of button
        /// </summary>
        /// <param name="angle">Angle.Up or Angle.Down enum</param>
        /// <returns>The angle in degrees</returns>
        protected float GetLightAngle(float angle)
        {
            float f = 1 - (float)this.Width / this.Height;       	// Calculate proportions of button
            float a = angle - (15 * f);							// Move virtual light source accordingly
            return a;
        }

        /// <summary>
        /// Calculate button edge width depending on button size
        /// </summary>
        /// <param name="rect">Rectangle defining the button</param>
        /// <returns>The width of the edge</returns>
        protected int GetEdgeWidth(Rectangle rect)
        {
            if (rect.Width < 50 | rect.Height < 50) return 1;
            else return 2;
        }

        /// <summary>
        /// Write the string using Graphics.Draw or VerticalString.Draw
        /// </summary>
        /// <param name="vertical">Is the button taller than it is wide</param>
        /// <param name="g">Graphics Object</param>
        /// <param name="textRect">Rectangle defining the text area for the button</param>
        protected void WriteString(bool vertical, Graphics g, Rectangle textRect)
        {
            if (vertical)
            {
                vs.Draw(g, buttonText, labelFont, labelBrush, textRect, labelStrFmt);
            }
            else
            {
                g.DrawString(buttonText, labelFont, labelBrush, textRect, labelStrFmt);
            }
        }

        /// <summary>
        /// Convert horizontal alignment from ContentAlignment to StringAlignment
        /// </summary>
        protected StringAlignment ConvertToHorAlign(ContentAlignment ca)
        {
            if ((ca == ContentAlignment.TopLeft) | (ca == ContentAlignment.MiddleLeft) | (ca == ContentAlignment.BottomLeft))
            {
                return StringAlignment.Near;
            }
            else if ((ca == ContentAlignment.TopRight) | (ca == ContentAlignment.MiddleRight) | (ca == ContentAlignment.BottomRight))
            {
                return StringAlignment.Far;
            }
            else
            {
                return StringAlignment.Center;
            }
        }

        /// <summary>
        /// Convert vertical alignment from ContentAlignment to StringAlignment
        /// </summary>
        protected StringAlignment ConvertToVertAlign(ContentAlignment ca)
        {
            if ((ca == ContentAlignment.TopLeft) | (ca == ContentAlignment.TopCenter) | (ca == ContentAlignment.TopRight))
            {
                return StringAlignment.Near;
            }
            else if ((ca == ContentAlignment.BottomLeft) | (ca == ContentAlignment.BottomCenter) | (ca == ContentAlignment.BottomRight))
            {
                return StringAlignment.Far;
            }
            else
            {
                return StringAlignment.Center;
            }
        }

        #endregion Private Help functions

        #region Event Handlers

        protected void mouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            buttonDown();
        }

        protected void mouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            buttonUp();
        }

        protected void buttonDown()
        {
            lightAngle = Angle.Down;
            buttonPressOffset = 1;
            this.Invalidate();
        }

        protected void buttonUp()
        {
            lightAngle = Angle.Up;
            buttonPressOffset = 0;
            this.Invalidate();
        }

        protected void keyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Space")
            {
                buttonDown();
            }
        }

        protected void keyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Space")
            {
                buttonUp();
            }
        }

        protected void weGotFocus(object sender, System.EventArgs e)
        {
            gotFocus = true;
            this.Invalidate();
        }

        protected void weLostFocus(object sender, System.EventArgs e)
        {
            gotFocus = false;
            buttonUp();
            this.Invalidate();
        }

        #endregion Event Handlers
    }

    #endregion

    #region Angle

    /// <summary>
    /// 「光」が輝いている角度を測定します。
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// 上向きの輝角
        /// </summary>
        public const float Up = 50F;

        /// <summary>
        /// 下向きの輝角
        /// </summary>
        public const float Down = Angle.Up + 180F;
    }

    #endregion

    #region RecessEditor

    public class RecessEditor : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            if ((context != null) && (provider != null))
            {
                // Access the property browser痴 UI display service, IWindowsFormsEditorService
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    // Create an instance of the UI editor control, passing a reference to the editor service
                    RecessEditorControl dropDownEditor = new RecessEditorControl(editorService);

                    // Pass the UI editor control the current property value
                    dropDownEditor.Recess = (int)value;

                    // Display the UI editor control
                    editorService.DropDownControl(dropDownEditor);

                    // Return the new property value from the UI editor control
                    return dropDownEditor.Recess;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }

    #endregion

    #region VerticalString

    internal class VerticalString
    {
        private enum esaType
        {
            Pre,
            Post,
            Either
        }

        private double textSpread;

        public double TextSpread
        {
            get
            {
                return textSpread;
            }

            set
            {
                textSpread = value;
            }
        }

        public VerticalString()
        {
            textSpread = .75F;
        }

        public void Draw(Graphics g, string text, Font font, Brush brush, Rectangle stringRect)
        {
            this.Draw(g, text, font, brush,
                      stringRect.X,
                      stringRect.Y);
        }

        public void Draw(Graphics g, string text, Font font, Brush brush, Rectangle stringRect, StringFormat stringStrFmt)
        {
            int horOffset;
            int vertOffset;

            // Set horizontal offset
            switch (stringStrFmt.Alignment)
            {
                case StringAlignment.Center:
                    horOffset = (stringRect.Width / 2) - (int)(font.Size / 2) - 2;
                    break;
                case StringAlignment.Far:
                    horOffset = (stringRect.Width - (int)font.Size - 2);
                    break;
                default:
                    horOffset = 0;
                    break;
            }

            // Set vertical offset

            double textSize = this.Length(text, font);

            switch (stringStrFmt.LineAlignment)
            {
                case StringAlignment.Center:
                    vertOffset = (stringRect.Height / 2) - (int)(textSize / 2);
                    break;
                case StringAlignment.Far:
                    vertOffset = stringRect.Height - (int)textSize - 2;
                    break;
                default:
                    vertOffset = 0;
                    break;
            }

            // Draw the string using the offsets
            this.Draw(g, text, font, brush,
                      stringRect.X + horOffset,
                      stringRect.Y + vertOffset);
        }

        [Description("Draw Method 3 - draw string at coordinates x, y")]
        public void Draw(Graphics g, string text, Font font, Brush brush, int x, int y)
        {
            char[] textChars = text.ToCharArray();				// Put the string into array of chars
            StringFormat charStrFmt = new StringFormat();		// Used to align each char centrally
            charStrFmt.Alignment = StringAlignment.Center;
            Rectangle charRect = new Rectangle(x, y, (int)(font.Size * 1.5), font.Height);

            for (int i = 0; i < text.Length; i++)
            {
                charRect.Offset(0, ExtraSpaceAllowance(esaType.Pre, textChars[i], font));	// Height allowance
                g.DrawString(textChars[i].ToString(), font, brush, charRect, charStrFmt);		// Write the character
                charRect.Offset(0, (int)(font.Height * textSpread));						// Standard height
                charRect.Offset(0, ExtraSpaceAllowance(esaType.Post, textChars[i], font));	// Height Allowance
            }
        }

        [Description("Length Method - returns vertical length of string")]
        public int Length(string text, Font font)
        {
            char[] textChars = text.ToCharArray();		// Put the string into array of chars
            int len = new int();

            for (int i = 0; i < text.Length; i++)
            {
                len += (int)(font.Height * textSpread);		// Add height of font, times spread factor.
                len += ExtraSpaceAllowance(esaType.Either, textChars[i], font); // Add allowance
            }

            return len;
        }


        private int ExtraSpaceAllowance(esaType type, char ch, Font font)
        {

            if (textSpread >= 1) return 0;				// No action if textSpread 1 or more

            int offset = 0;

            // Do we need to pad BEFORE the next char?
            if (type == esaType.Pre | type == esaType.Either)
            {
                // Does our character appear in the "pre" list? (ie taller than average)
                if (" bdfhijkltABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IndexOf(ch) > 0)
                {
                    // TODO Allow for textSpread here.
                    //offset += (int)(font.Height * .2 * (1 - textSpread));
                    offset += (int)(font.Height * .2);
                }
            }

            // Do we need to pad AFTER the next char?
            if (type == esaType.Post | type == esaType.Either)
            {
                // Does our character appear in the "post" list? (is dangles over the bottom of the line)
                if (" gjpqyQ".IndexOf(ch) > 0)
                {
                    // TODO Allow for textSpread here.
                    //offset += (int)(font.Height * .2 * (1 - textSpread));
                    offset += (int)(font.Height * .2);
                }
            }

            return offset;
        }
    }

    #endregion
}
