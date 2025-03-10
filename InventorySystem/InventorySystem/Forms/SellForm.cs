﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventorySystem.Forms
{
    public partial class SellForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
        public SellForm()
        {
            InitializeComponent();
        }

        public void fill_dg()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from stockTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            metroGrid2.DataSource = dt;
        }

        public void disp_unit()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from unitstable order by Unit_List";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            metroGrid2.DataSource = dt;
        }

        public void fill_product_name()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from productTable order by Product_Name";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["Product_Name"].ToString());
            }
        }

        public void fill_product_units()
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from unitsTable order by Unit_List";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox2.Items.Add(dr["Unit_List"].ToString());
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            int i;
            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "Select * from stockTable where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
            cmd1.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            i = Convert.ToInt32(dt1.Rows.Count.ToString());

            if (i == 0)
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd.ExecuteNonQuery();

                SqlCommand cmd3 = con.CreateCommand();
                cmd3.CommandType = CommandType.Text;
                cmd3.CommandText = "insert into stockTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd3.ExecuteNonQuery();

                fill_dg();
                metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
            else
            {
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "insert into quantityTable values('" + comboBox1.SelectedItem.ToString() + "','" + Convert.ToInt32(textBox1.Text) + "','" + comboBox2.SelectedItem.ToString() + "')";
                cmd2.ExecuteNonQuery();

                SqlCommand cmd4 = con.CreateCommand();
                cmd4.CommandType = CommandType.Text;
                cmd4.CommandText = "update stockTable set Product_Qty=Product_Qty - " + textBox1.Text + " where Product_Name='" + comboBox1.Text + "' and Product_Unit='" + comboBox2.Text + "'";
                cmd4.ExecuteNonQuery();

                fill_dg();
                metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                MessageBox.Show("Item '" + comboBox1.SelectedItem.ToString() + "' '" + comboBox2.SelectedItem.ToString() + "' Successfully Updated!");
            }
        }

        private void SellForm_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            fill_product_name();
            fill_product_units();
            fill_dg();
            metroGrid2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
    }
}
