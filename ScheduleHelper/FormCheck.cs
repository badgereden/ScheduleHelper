using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace ScheduleHelper
{
    public partial class FormCheck : Form
    {
        public List<Item> query;
        public ListViewItem currentSelectedItem;
        public FormCheck()
        {
            InitializeComponent();
            query = new List<Item>();
            ReadXMLConfig();
            LoadQuery(query);
        }

        

        public int ReadXMLConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("query.xml");
            
            query.Clear();
            int count = 0;
            foreach (XmlElement e in doc.GetElementsByTagName("item"))
            {
                query.Add(new Item(e.GetElementsByTagName("name")[0].InnerText, e.GetElementsByTagName("description")[0].InnerText, e.GetElementsByTagName("sql")[0].InnerText));
                count++;
            }


            return count;
        }

        private void LoadQuery(List<Item> _query)
        {
            foreach (Item item in _query)
            {
                //ListViewItem it = new ListViewItem(new string[] {item.Name,item.Description });
                MyListViewItem it = new MyListViewItem(new string[] { item.Name, item.Description }, item.SQL);
                lvQuery.Items.Add(it);
                //it.
            }
        }

        private void lvQuery_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (currentSelectedItem != null)
            {
                string sql = (currentSelectedItem as MyListViewItem).SQL;
                DataTable dt = StaticSQLiteHelper.ExecuteQuery(sql);
                dataGridView1.DataSource = dt;
                //MessageBox.Show(sql);
            }
        }

        private void lvQuery_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void lvQuery_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                currentSelectedItem = e.Item;
            }
            else
            {
                currentSelectedItem = null;
            }
        }
    }





    public struct Item
    {
       public string Name { get; set; }
        public string Description { get; set; }
        public string SQL { get; set; }
        public Item(string name,string des,string sql):this()
        {
            Name = name;
            Description = des;
            SQL = sql;
        }
    }
    public class MyListViewItem : ListViewItem
    {
        public string SQL { get; set; }
        public MyListViewItem(string[] items, string sql):base(items)
        {
            this.SQL = sql;
        }

    }

}
