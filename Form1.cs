using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lr4._1
{
    public partial class Form1 : Form
    {
        MyStorage storage;
        Bitmap bmp = new Bitmap(1000, 1000);

        public Form1()
        {
            InitializeComponent();
            storage = new MyStorage();
        }

        private void paintBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            storage.DrawAll(paintBox, g, bmp);
        }

        private void paintBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (storage.isCheckedStorage(e) == false)//нажатие на пустое место
            {
                storage.AllNotChecked();
                storage.AddObject(new CCircle(e.X, e.Y, 50));//добавление нового круга в хранилище
            }
            else//если нажата ctrl, то можно выделить несколько кругов
                if (Control.ModifierKeys == Keys.Control)
                storage.MakeCheckedObjectStorage(e);
            else//если не нажата, выделяется только один круг
            {
                storage.AllNotChecked();
                storage.MakeCheckedObjectStorage(e);
            }
            this.Refresh();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Graphics g = Graphics.FromImage(bmp);
                storage.RemoveCheckedObject();
                g.Clear(Color.White);
            }

        }
    }
    class CCircle
    {
        private int radius;
        private int x;
        private int y;
        private bool isClick;
        public CCircle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
        }
        public void Draw(PictureBox pb, Graphics g, Bitmap bmp)//метод, рисующий круг 
        {
            Pen pen;
            if (isClick == true)
                pen = new Pen(Color.Blue);
            else
                pen = new Pen(Color.Black);
            g.DrawEllipse(pen, (x - (radius / 2)), (y - (radius / 2)), radius, radius);
            pb.Image = bmp;
        }
        public bool isClicked(MouseEventArgs e)//попали ли в область круга
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= radius * radius)
                return true;
            else
                return false;
        }
        public void MakeClickTrue()
        {
            isClick = true;
        }
        public void MakeClickFalse()
        {
            isClick = false;
        }
        public bool IsClick()
        {
            return isClick;
        }
    }

    class MyStorage//хранилище
    {
        int size;
        CCircle[] storage;
        public MyStorage()
        {
            size = 0;
        }
        public void SetObject(int i, CCircle obj)
        {
            storage[i] = obj;
        }
        public void AddObject(CCircle obj)
        {
            Array.Resize(ref storage, size + 1);
            storage[size] = obj;
            size = size + 1;
        }
        public bool isCheckedStorage(MouseEventArgs e)//проверка, нажато ли на какой-либо круг
        {
            for (int i = 0; i < size; i++)
                if (storage[i].isClicked(e) == true)
                    return true;
            return false;
        }
        public void MakeCheckedObjectStorage(MouseEventArgs e)//делает объект выделенным при нажатии на него
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].isClicked(e) == true)
                {
                    storage[i].MakeClickTrue();
                    i = size;
                }
            }
        }
        public void RemoveObject(int i)//удаление объекта
        {
            if (size > 1 && i < size)
            {
                CCircle[] storage2 = new CCircle[size - 1];
                for (int j = 0; j < i; j++)
                    storage2[j] = storage[j];
                storage[i] = null;
                for (int j = i; j < size - 1; j++)
                    storage2[j] = storage[j + 1];
                size = size - 1;
                storage = storage2;
            }
            else
            {
                size = 0;
                storage[size] = null;
            }
        }
        public void RemoveCheckedObject()//удаление выделенных объектов
        {
            for (int i = 0; i < size; i++)
            {
                if (storage[i].IsClick() == true)
                {
                    RemoveObject(i);
                    i = i - 1;
                }
            }
        }
        public void AllNotChecked()
        {
            for (int i = 0; i < size; i++)
                storage[i].MakeClickFalse();
        }
        public void DrawAll(PictureBox pb, Graphics g, Bitmap bmp)
        {
            for (int i = 0; i < size; i++)
                storage[i].Draw(pb, g, bmp);
        }
        public int getSize()
        {
            return size;
        }
    }

}
