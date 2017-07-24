﻿namespace VisualPlus.Toolkit.Controls
{
    #region Namespace

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Windows.Forms;

    using VisualPlus.Enumerators;
    using VisualPlus.Structure;
    using VisualPlus.Toolkit.ActionList;
    using VisualPlus.Toolkit.VisualBase;

    #endregion

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ListView))]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    [Description("The Visual ListView")]
    [Designer(typeof(VisualListViewTasks))]
    public class VisualListView : ContainedControlBase, IContainedControl
    {
        #region Variables

        private ListView _listView;
        private Border columnBorder;
        private Color columnHeaderBackground;
        private Size columnSize;
        private bool drawStandardHeader;
        private Font headerFont;
        private Color headerText;
        private Color itemBackground;
        private Color itemHover;
        private int itemPadding = 12;
        private Color itemSelected;

        #endregion

        #region Constructors

        public VisualListView()
        {
            // Contains another control
            SetStyle(ControlStyles.ContainerControl, true);

            // Cannot select this control, only the child ListView and does not generate a click event
            SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick, false);

            headerFont = StyleManager.Font;

            Background = StyleManager.ControlStyle.Background(3);

            _listView = new ListView
                {
                    BackColor = Background,
                    Size = GetInternalControlSize(Size, Border),
                    BorderStyle = BorderStyle.None,
                    View = View.Details,
                    MultiSelect = false,
                    LabelEdit = false,
                    AllowColumnReorder = false,
                    CheckBoxes = false,
                    FullRowSelect = true,
                    GridLines = true,
                    HeaderStyle = ColumnHeaderStyle.Nonclickable,
                    OwnerDraw = true,
                    Location = GetInternalControlLocation(Border)
                };

            AutoSize = true;
            BackColor = Color.Transparent;
            Size = new Size(250, 150);

            // AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            // AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            columnHeaderBackground = StyleManager.ControlStyle.FlatButtonDisabled;
            headerText = StyleManager.FontStyle.ForeColor;
            itemBackground = StyleManager.ControlStyle.ItemEnabled;
            itemHover = StyleManager.ControlStyle.ItemHover;
            itemSelected = StyleManager.BorderStyle.Color;

            columnBorder = new Border
                {
                    Type = ShapeType.Rectangle,
                    HoverVisible = false
                };

            _listView.DrawColumnHeader += ListView_DrawColumnHeader;
            _listView.DrawItem += ListView_DrawItem;
            _listView.DrawSubItem += ListView_DrawSubItem;

            Controls.Add(_listView);
        }

        #endregion

        #region Properties

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public Color ColumnBackground
        {
            get
            {
                return columnHeaderBackground;
            }

            set
            {
                columnHeaderBackground = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof(BorderConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category(Localize.PropertiesCategory.Appearance)]
        public Border ColumnBorder
        {
            get
            {
                return columnBorder;
            }

            set
            {
                columnBorder = value;
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ColumnHeaderCollection Columns
        {
            get
            {
                return _listView.Columns;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(false)]
        [Description("Gets access to the contained control.")]
        public Control ContainedControl
        {
            get
            {
                return _listView;
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListViewGroupCollection Groups
        {
            get
            {
                return _listView.Groups;
            }
        }

        [Category(Localize.PropertiesCategory.Layout)]
        [Description(Localize.Description.Strings.Font)]
        public Font HeaderFont
        {
            get
            {
                return headerFont;
            }

            set
            {
                headerFont = value;
                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public Color HeaderText
        {
            get
            {
                return headerText;
            }

            set
            {
                headerText = value;
                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public Color ItemBackground
        {
            get
            {
                return itemBackground;
            }

            set
            {
                itemBackground = value;
                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public Color ItemHover
        {
            get
            {
                return itemHover;
            }

            set
            {
                itemHover = value;
                Invalidate();
            }
        }

        [Category(Localize.PropertiesCategory.Appearance)]
        public int ItemPadding
        {
            get
            {
                return itemPadding;
            }

            set
            {
                itemPadding = value;
                Invalidate();
            }
        }

        [Category("Data")]
        [Description("The items in the VisualListView.")]
        [Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Localizable(true)]
        public virtual ListView.ListViewItemCollection Items
        {
            get
            {
                return _listView.Items;
            }
        }

        [Category(Localize.PropertiesCategory.Appearance)]
        [Description(Localize.Description.Common.Color)]
        public Color ItemSelected
        {
            get
            {
                return itemSelected;
            }

            set
            {
                itemSelected = value;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category(Localize.PropertiesCategory.Behavior)]
        public bool StandardHeader
        {
            get
            {
                return drawStandardHeader;
            }

            set
            {
                drawStandardHeader = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Selects one of five different views that can be shown in.")]
        [DefaultValue(false)]
        public virtual View View
        {
            get
            {
                return _listView.View;
            }

            set
            {
                _listView.View = value;
            }
        }

        #endregion

        #region Events

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            LastPosition = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            ControlGraphicsPath = Border.GetBorderShape(ClientRectangle, ControlBorder);

            graphics.SetClip(ControlGraphicsPath);

            if (Background != _listView.BackColor)
            {
                _listView.BackColor = Background;
            }

            graphics.FillRectangle(new SolidBrush(Background), ClientRectangle);

            graphics.ResetClip();

            Border.DrawBorderStyle(graphics, ControlBorder, MouseState, ControlGraphicsPath);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _listView.Location = GetInternalControlLocation(ControlBorder);
            _listView.Size = GetInternalControlSize(Size, ControlBorder);
        }

        private static StringFormat GetStringFormat()
        {
            return new StringFormat
                {
                    FormatFlags = StringFormatFlags.LineLimit,
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint;

            columnSize = new Size(Width, e.Bounds.Height);

            Rectangle columnHeaderRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, columnSize.Width, columnSize.Height);
            GraphicsPath columnHeaderPath = new GraphicsPath();
            columnHeaderPath.AddRectangle(columnHeaderRectangle);
            columnHeaderPath.CloseAllFigures();

            if (drawStandardHeader)
            {
                // Draw the standard header background.
                e.DrawBackground();
            }
            else
            {
                // Draw column header background
                e.Graphics.FillRectangle(new SolidBrush(columnHeaderBackground), columnHeaderRectangle);
            }

            Border.DrawBorderStyle(graphics, columnBorder, MouseState, columnHeaderPath);

            StringFormat stringFormat = new StringFormat
                {
                    // Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

            // Draw the header text.
            e.Graphics.DrawString(e.Header.Text, headerFont, new SolidBrush(headerText), new Rectangle(e.Bounds.X + itemPadding, e.Bounds.Y + itemPadding, e.Bounds.Width - (itemPadding * 2), e.Bounds.Height - (itemPadding * 2)), stringFormat);
            graphics.Dispose();
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // We draw the current line of items (= item with sub items) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
            Bitmap bitmap = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            // always draw default background
            graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

            if (e.State.HasFlag(ListViewItemStates.Selected))
            {
                // selected background
                graphics.FillRectangle(new SolidBrush(itemSelected), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }
            else if (e.Bounds.Contains(LastPosition) && (MouseState == MouseStates.Hover))
            {
                // hover background
                graphics.FillRectangle(new SolidBrush(itemHover), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }

            // Draw separator
            graphics.DrawLine(new Pen(StyleManager.BorderStyle.Color), e.Bounds.Left, 0, e.Bounds.Right, 0);

            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                // Draw text
                graphics.DrawString(subItem.Text, Font, new SolidBrush(Color.Black), new Rectangle(subItem.Bounds.X + itemPadding, itemPadding, subItem.Bounds.Width - (2 * itemPadding), subItem.Bounds.Height - (2 * itemPadding)), GetStringFormat());
            }

            if ((e.State & ListViewItemStates.Selected) != 0)
            {
                // Selected item background
                e.Graphics.FillRectangle(new SolidBrush(itemSelected), e.Bounds);
            }
            else
            {
                // Unselected item background
                e.Graphics.FillRectangle(new SolidBrush(itemBackground), e.Bounds);
            }

            // Draw the item text for views other than the Details view
            if (_listView.View != View.Details)
            {
                e.DrawText();
            }

            e.Graphics.DrawImage((Image)bitmap.Clone(), new Point(0, e.Item.Bounds.Location.Y));
            graphics.Dispose();
            bitmap.Dispose();
        }

        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Left;

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        flags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        flags = TextFormatFlags.Right;
                        break;
                }

                // Draw the text and background for a subitem with a 
                // negative value. 
                double subItemValue;
                if ((e.ColumnIndex > 0) && double.TryParse(e.SubItem.Text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out subItemValue) && (subItemValue < 0))
                {
                    // Unless the item is selected, draw the standard 
                    // background to make it stand out from the gradient.
                    if ((e.ItemState & ListViewItemStates.Selected) == 0)
                    {
                        e.DrawBackground();
                    }

                    // Draw the subitem text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text, Font, Brushes.Red, e.Bounds, sf);

                    return;
                }

                // Draw normal text for a subitem with a nonnegative 
                // or nonnumerical value.
                e.DrawText(flags);
            }
        }

        #endregion
    }
}