using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hetwork
{
    public partial class CheckListPro : UserControl
    {
        public List<CheckedItemPro> Items = new List<CheckedItemPro>();
        public Timer timer = new Timer();
        bool needRepaint = true;

        private Color elementColor = Color.Gray;
        private Color hoverColor = Color.LightGray;
        private Color hoverCheckColor = Color.LightGray;

        private int yOffset = 0;

        private int minimumScroll = 0;

        private int elementDistance = 3;

        //private int elementHeight = 10;

        private bool useBorder = true;
        private Color borderColor = Color.Black;
        private float borderWidth = 0.5f;
        private Color checkColor = Color.White;
        private int horizontalPadding = 3;
        private Font textFont = new Font("Arial", 8);
        private Color textColor = Color.Black;
        private Color isCheckColor = Color.Gray;
        private Color selectedColor = Color.DarkGray;
        private bool useCheckBox = true;



        private int fontHeight
        {
            get
            {
                return (int)textFont.Height;
            }
        }


        private int maximumScroll
        {
            get
            {
                return Items.Count * (fontHeight + elementDistance) - Height;
            }
        }

        private int scrollSensitivity = 5;
        private Form parentForm;

        public CheckListPro(Form _form)
        {
            InitializeComponent();
            parentForm = _form;
            DoubleBuffered = true;
            timer.Interval = 30;
            timer.Tick += TimerOnTick;
            timer.Start();


            MouseWheel += Control_MouseWheel;
            PopulateNewMenu();
        }

        public void PopulateNewMenu()
        {
            ContextMenu cm = new ContextMenu();
            MenuItem mi1 = new MenuItem();
            mi1.Text = "Add Item";
            mi1.Click += new System.EventHandler(this.AddItem);
            cm.MenuItems.Add(mi1);

            ContextMenu = cm;
        }

        public void AddItem(object sender, System.EventArgs e)
        {
            Items.Add(new CheckedItemPro(false, "New Item", "", Program.selectedProject.GetTaskId()));
            NodeSelected_Event(this, e);
            needRepaint = true;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (needRepaint)
            {
                Invalidate();
            }
        }


        public void Paint_Object(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //this.Region = GetRoundedRegion(this.Width, this.Height);

            for (int i = 0; i < Items.Count; i++)
            {

                //  DRAW BACKGROUND
                if (selectetedItem == i)
                {
                    g.FillRectangle(new SolidBrush(selectedColor), new Rectangle(new Point(horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset), new Size(Width - horizontalPadding * 2, fontHeight)));
                }
                else if (hoverId != i)
                {
                    g.FillRectangle(new SolidBrush(elementColor), new Rectangle(new Point(horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset), new Size(Width - horizontalPadding * 2, fontHeight)));
                }
                else
                {
                    g.FillRectangle(new SolidBrush(hoverColor), new Rectangle(new Point(horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset), new Size(Width - horizontalPadding * 2, fontHeight)));
                }
                //  DRAW BORDER
                if (useBorder)
                    g.DrawRectangle(new Pen(borderColor, borderWidth), new Rectangle(new Point(horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset), new Size(Width - 1 - horizontalPadding * 2, fontHeight)));

                //  DRAW CHECK BACKGROUND
                if (useCheckBox)
                {
                    if (i != hoverCheckId)
                    {
                        g.FillRectangle(new SolidBrush(checkColor), new Rectangle(new Point(1 + horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset + 1), new Size(fontHeight - 2, fontHeight - 2)));
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(hoverCheckColor), new Rectangle(new Point(1 + horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset + 1), new Size(fontHeight - 2, fontHeight - 2)));
                    }

                    if (Items[i].check)
                    {
                        g.FillEllipse(new SolidBrush(isCheckColor), new Rectangle(new Point(1 + horizontalPadding + 2, ((int)fontHeight + elementDistance) * i - yOffset + 1 + 2), new Size(fontHeight - 2 - 4, fontHeight - 2 - 4)));
                    }

                    //  DRAW CHECK BORDER

                    g.DrawRectangle(new Pen(borderColor, 0.5f), new Rectangle(new Point(1 + horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset + 1), new Size(fontHeight - 2, fontHeight - 2)));
                }

                //  DRAW TEXT
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                SizeF textSize = g.MeasureString(Items[i].name, textFont);
                int textWidth = (int)textSize.Width;
                string newText = Items[i].name;
                if (horizontalPadding + 14 + textWidth + 4 > Width - horizontalPadding * 2)
                {
                    int charIndex = 0;
                    newText = "";
                    while (horizontalPadding + 14 + g.MeasureString(newText, textFont).Width < Width - horizontalPadding * 2 - 10 + 5 && charIndex < Items[i].name.Length)
                    {
                        newText += Items[i].name[charIndex];
                        charIndex++;
                    }
                    newText += "..";
                }
                g.DrawString(newText, textFont, new SolidBrush(textColor), new Point(horizontalPadding + 14 + 5, (fontHeight + elementDistance) * i - yOffset));
            }


            needRepaint = false;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        //function call, it will return the rounded region of the given region
        public static System.Drawing.Region GetRoundedRegion(int controlWidth, int controlHeight)
        {
            return System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, controlWidth, controlHeight, 30, 30));
        }
        //the following Line of Code will round the edges of C# form
        

        void Control_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                yOffset -= scrollSensitivity;
            }
            else if (e.Delta < 0)
            {
                yOffset += scrollSensitivity;
            }

            if (yOffset > maximumScroll)
            {
                yOffset = maximumScroll;
            }
            if (yOffset < minimumScroll)
            {
                yOffset = minimumScroll;
            }
            needRepaint = true;


        }

        private void CheckListPro_Resize(object sender, EventArgs e)
        {
            needRepaint = true;
            Invalidate();
        }

        private bool leftMouseDown = false;
        private bool dragging = false;


        private void CheckListPro_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                    
                leftMouseDown = true;

                if (GetCheckByRect(e.Location) != -1 && hoverCheckId != -1 && Items.Count > hoverCheckId)
                {
                    Items[hoverCheckId].check = !Items[hoverCheckId].check;
                }

                if (GetItemByRect(e.Location) != -1 && hoverId != -1 && Items.Count > hoverId)
                {
                    selectetedItem = hoverId;
                    dragging = true;
                    Cursor.Current = Cursors.SizeAll;
                }
                else
                {
                    selectetedItem = -1;
                }
            }

            needRepaint = true;
        }

        

        private void CheckListPro_MouseMove(object sender, MouseEventArgs e)
        {
            hoverId = -1;
            hoverCheckId = -1;

            hoverId = GetItemByRect(e.Location);
            if(hoverId != -1)
            {
                int scounter = 0;
                int ccounter = 0;
                string text = "";
                for(int i = 0; i < Items[hoverId].details.Length; i++)
                {
                    if(Items[hoverId].details[i] == ' ')
                    {
                        scounter++;
                    }

                    text += Items[hoverId].details[i];

                    if (scounter >= 4)
                    {
                        text += "\n";
                        scounter = 0;
                        ccounter = i;
                    }
                    else if(i == ccounter + 30)
                    {
                        ccounter = i;
                        text += "\n";
                    }
                }
                    
                    
                toolTip1.SetToolTip(this, text);
            }

            if (hoverId == -1 && useCheckBox)
            {
                hoverCheckId = GetCheckByRect(e.Location);
            }


            if(dragging)
            {
                Cursor.Current = Cursors.SizeAll;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }

            

            needRepaint = true;
        }

        private int hoverId = -1;
        private int hoverCheckId = -1;

        public int selectetedItem = -1;

        int GetItemByRect(Point p)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (new Rectangle(new Point(horizontalPadding + 1 + fontHeight - 2, ((int)fontHeight + elementDistance) * i - yOffset), new Size(Width - horizontalPadding * 2 - (1 + fontHeight - 2), fontHeight)).Contains(p))
                {
                    return i;
                }
            }
            return -1;
        }

        int GetCheckByRect(Point p)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (new Rectangle(new Point(1 + horizontalPadding, ((int)fontHeight + elementDistance) * i - yOffset + 1), new Size(fontHeight - 2, fontHeight - 2)).Contains(p))
                {
                    return i;
                }
            }

            return -1;
        }


        private void CheckListPro_MouseUp(object sender, MouseEventArgs e)
        {
            leftMouseDown = false;

            if (e.Button == MouseButtons.Left)
            {
                if (hoverId != -1 && selectetedItem != hoverId)
                {
                    Items.Rearrange(selectetedItem, hoverId);
                    selectetedItem = hoverId;
                }
                dragging = false;
            }
        }


        private void CheckListPro_MouseLeave(object sender, EventArgs e)
        {
            hoverId = -1;
        }



        [Browsable(true)]
        [Category("Check List Pro Action")]
        [Description("Invoked when items change")]
        public event EventHandler ItemsChanged;
        public void NodeSelected_Event(object sender, EventArgs e)
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, e);
            }
        }



        #region Properties


        [Browsable(true), Category("Check List Pro"), Description("Item background color")]
        public Color ElementColor
        {
            get
            {
                return elementColor;
            }
            set
            {
                elementColor = value;
                needRepaint = true;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Distance between each item")]
        public int ElementDistance
        {
            get
            {
                return elementDistance;
            }
            set
            {
                elementDistance = value;
                needRepaint = true;
            }
        }

        //[Browsable(true), Category("Check List Pro"), Description("Height of item element")]
        //public int ElementHeight
        //{
        //    get
        //    {
        //        return elementHeight;
        //    }
        //    set
        //    {
        //        elementHeight = value;
        //        needRepaint = true;
        //    }
        //}

        [Browsable(true), Category("Check List Pro"), Description("Amount the list offsets when scrolling")]
        public int ScrollSensitivity
        {
            get
            {
                return scrollSensitivity;
            }
            set
            {
                scrollSensitivity = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Choose whether to display element borders")]
        public bool UseItemBorders
        {
            get
            {
                return useBorder;
            }
            set
            {
                useBorder = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Element border rendering color")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Element border rendering width")]
        public float BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Check background color")]
        public Color CheckColor
        {
            get
            {
                return checkColor;
            }
            set
            {
                checkColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Padding to left and right of element")]
        public int HorizontalPadding
        {
            get
            {
                return horizontalPadding;
            }
            set
            {
                horizontalPadding = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Item font")]
        public Font TextFont
        {
            get
            {
                return textFont;
            }
            set
            {
                textFont = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Item font color")]
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Item hover color")]
        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }
            set
            {
                hoverColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Checkbox hover color")]
        public Color HoverCheckColor
        {
            get
            {
                return hoverCheckColor;
            }
            set
            {
                hoverCheckColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Color of check inside checkbox")]
        public Color IsCheckedColor
        {
            get
            {
                return isCheckColor;
            }
            set
            {
                isCheckColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Item selected color")]
        public Color SelectedColor
        {
            get
            {
                return selectedColor;
            }
            set
            {
                selectedColor = value;
            }
        }

        [Browsable(true), Category("Check List Pro"), Description("Use Checkbox")]
        public bool UseCheckbox
        {
            get
            {
                return useCheckBox;
            }
            set
            {
                useCheckBox = value;
            }
        }



        #endregion

        private void CheckListPro_KeyDown(object sender, KeyEventArgs e)
        {
            if(selectetedItem != -1)
            {
                if(e.KeyCode == Keys.Delete)
                {
                    Items.RemoveAt(selectetedItem);
                    needRepaint = true;
                }
            }
        }
    }


    public class CheckedItemPro
    {
        public bool check = false;
        public string name = "";
        public string details = "";
        public int id;
        public CheckedItemPro(bool checkStatus, string nameDetail, string detailString, int ID)
        {
            check = checkStatus;
            name = nameDetail;
            details = detailString;
            id = ID;
        }
    }


    public static class ListExtensions
    {
        
        public static void Rearrange<T>(this List<T> list, int index, int targetIndex)
        {
            try
            {
                T v = list[index];
                list.RemoveAt(index);
                list.Insert(targetIndex, v);
            }
            catch
            {

            }
        }

        public static void SendToTop<T>(this List<T> list, int index, int targetIndex)
        {
            T v = list[index];
            list.RemoveAt(index);
            list.Insert(0, v);
        }

        public static void SendToBottom<T>(this List<T> list, int index, int targetIndex)
        {
            T v = list[index];
            list.RemoveAt(index);
            list.Insert(list.Count, v);
        }
    }
}
