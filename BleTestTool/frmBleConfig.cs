﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BleTestTool
{
    public partial class frmBleConfig : Form
    {
        /// <summary>
        /// 定义BleConfig类
        /// </summary>
        private BleConfig bleConfig;

        #region 加载页面

        public frmBleConfig(BleConfig bc)
        {
            InitializeComponent();
            bleConfig = bc;
        }

        private void frmBleConfig_Load(object sender, EventArgs e)
        {
            Console.WriteLine("\r\nDicBleBlackListConfig");
            foreach (KeyValuePair<string,string> item in bleConfig.DicBleBlackListConfig)
            {
                Console.WriteLine(item.Key + " : " + item.Value);
            }

            Console.WriteLine("\r\nDicBleNameReplaceConfig");
            foreach (KeyValuePair<string, string> item in bleConfig.DicBleNameReplaceConfig)
            {
                Console.WriteLine(item.Key + " : " + item.Value);
            }

            //初始化列表
            InitBleNameReplaceList(listViewBleNameReplace);
            InitBleBlackList();
        }

        private void frmBleConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = false;
        }

        private void InitBleNameReplaceList(ListView listView)
        {
            //基本属性设置
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView.View = View.Details;

            //创建列表头
            listView.Columns.Add("蓝牙名称", 170, HorizontalAlignment.Left);
            listView.Columns.Add("替换内容", 170, HorizontalAlignment.Left);

            //添加数据
            listView.BeginUpdate();
            foreach (KeyValuePair<string,string> item in bleConfig.DicBleNameReplaceConfig)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = item.Key;
                listViewItem.SubItems.Add(item.Value);
                listView.Items.Add(listViewItem);
            }
            listView.EndUpdate();
        }

        private void InitBleBlackList()
        {

        }

        #endregion

        #region 表格操作
        /// <summary>
        /// 添加List数据
        /// </summary>
        /// <param name="listView">表格</param>
        /// <param name="key">数据</param>
        /// <param name="value">数据</param>
        private void AddListData(ListView listView, string key, string value)
        {
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = key;
            listViewItem.SubItems.Add(value);
            listView.Items.Add(listViewItem);
        }

        /// <summary>
        /// 编辑List数据
        /// </summary>
        /// <param name="listView">表格</param>
        /// <param name="key">数据(唯一标识)</param>
        /// <param name="value">数据</param>
        private void EditListData(ListView listView, string key, string value)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Text == key)
                {
                    item.SubItems[1].Text = value;
                    return;
                }
            }
        }

        /// <summary>
        /// 删除List数据
        /// </summary>
        /// <param name="listView">表格</param>
        /// <param name="index">标号</param>
        private void DeleteListData(ListView listView, int index)
        {
            if (listView.Items.Count > index)
            {
                listView.Items[index].Remove();
            }
        }
        #endregion

        #region 蓝牙名称替换控件
        private void btnBleNameReplace_Click(object sender, EventArgs e)
        {
            string key = txtBleName.Text.Trim();
            string value = txtBleReplace.Text.Trim();
            if (key != "" && value != "")
            {
                if (!bleConfig.DicBleNameReplaceConfig.ContainsKey(key))
                {
                    //添加数据
                    AddListData(listViewBleNameReplace, key, value);
                    bleConfig.DicBleNameReplaceConfig.Add(key, value);
                    bleConfig.SaveBleNameReplaceConfig();
                }
                else
                {
                    //编辑数据
                    EditListData(listViewBleNameReplace, key, value);
                    bleConfig.DicBleNameReplaceConfig[key] = value;
                    bleConfig.SaveBleNameReplaceConfig();
                }
            }
            else
            {
                MessageBox.Show("内容不可为空！", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewBleNameReplace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count > 0)
            {
                txtBleName.Text = ((ListView)sender).SelectedItems[0].Text;
                txtBleReplace.Text = ((ListView)sender).SelectedItems[0].SubItems[1].Text;
            }
            else
            {
                txtBleName.Text = "";
                txtBleReplace.Text = "";
            }
        }

        private void listViewBleNameReplace_DoubleClick(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count > 0)
            {
                bleConfig.DicBleNameReplaceConfig.Remove(listView.SelectedItems[0].Text);
                bleConfig.SaveBleNameReplaceConfig();
                DeleteListData(listView, listView.SelectedItems[0].Index);
                //listView.SelectedItems[0].Remove();
            }
        }

        #endregion


    }
}
