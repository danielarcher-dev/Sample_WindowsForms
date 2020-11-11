using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drag_and_Drop_Process_Queue
{
    public partial class ProcessQueue : Form
    {
        public ProcessQueue()
        {
            InitializeComponent();

            listBox_left.AllowDrop = true;
            listBox_right.AllowDrop = true;

        }



        private void generic_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private string[] FilePicker_DroppedFile(DragEventArgs e)
        {
            return (string[])e.Data.GetData(DataFormats.FileDrop);
        }

        private void AddArray_To_ListBox(string[] list, ListBox listBox)
        {
            foreach (string item in list)
            {
                listBox.Items.Add(item);
            }
        }
        private void AddString_To_ListBox(string item, ListBox listBox)
        {
            listBox.Items.Add(item);
        }

        private void generic_DragDrop(object sender, DragEventArgs e)
        {
            // add a file that was dropped over
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                AddArray_To_ListBox(FilePicker_DroppedFile(e), (ListBox)sender);
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                AddString_To_ListBox((String)e.Data.GetData(DataFormats.Text), (ListBox)sender);
            }
            // but move a list item that was clicked on
            else
            {
                generic_MoveItem(sender, e);
            }
        }
        private void generic_DragLeave(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                // TODO: get underlying data and add to list
                //AddString_To_ListBox(e.Data.ToString(), (ListBox)sender);

                // TODO: don't forget to remove from source list
                ListBox listBox = (ListBox)sender;
                Point point = listBox.PointToClient(new Point(e.X, e.Y));
                int index = listBox.IndexFromPoint(point);
                if (index < 0) index = listBox.Items.Count - 1;
                object data = e.Data.GetData(typeof(String));
                listBox.Items.Remove(data);
                //listBox.Items.Insert(index, data);
            }
        }
        private void generic_MoveItem(object sender, DragEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Point point = listBox.PointToClient(new Point(e.X, e.Y));
            int index = listBox.IndexFromPoint(point);
            if (index < 0) index = listBox.Items.Count - 1;
            object data = e.Data.GetData(typeof(String));
            listBox.Items.Remove(data);
            listBox.Items.Insert(index, data);
        }

        private void generic_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.Move;
        }
        private void generic_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedItem == null) return;
            listBox.DoDragDrop(listBox.SelectedItem, DragDropEffects.Move);
        }

        private void generic_DragLeave(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            listBox.Items.Remove(listBox.SelectedItem);
        }
    }
}
