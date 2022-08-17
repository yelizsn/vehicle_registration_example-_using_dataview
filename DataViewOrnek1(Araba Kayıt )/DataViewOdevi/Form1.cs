using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataViewOdevi
{
    public partial class Form1 : Form
    {

        DataView DtView;
        DataSet DtSet;
        OleDbDataAdapter DataAdapter;
        OleDbCommand Komut;
        string ConnectionString = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=araclar.accdb"; 

        public Form1()
        {
            InitializeComponent();
            VeriGoruntule(); 
        }
        private void VeriGoruntule() 
        {
            using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
            {
                DtView = new DataView();
                DtSet = new DataSet();
                Komut = new OleDbCommand("SELECT * FROM Araclar_Tablosu", baglanti); 
                DataAdapter = new OleDbDataAdapter();  
                DataAdapter.SelectCommand = Komut;
                DataAdapter.Fill(DtSet);
                DtView = new DataView(DtSet.Tables[0]); 
                DtView.Sort = "Id"; 
                dataGridView1.DataSource = DtView; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView newRow = DtView.AddNew(); 
                newRow["Marka"] = txt_marka.Text;
                newRow["Model"] = txt_model.Text;
                newRow["Beygir_Gucu"] = Convert.ToUInt16(txt_beygirGucu.Text);
                newRow.EndEdit();
                DtView.Sort = "Id";
                using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
                { 
                    OleDbCommand com = new OleDbCommand("INSERT INTO Araclar_Tablosu (Marka,Model,Beygir_Gucu) values (@Marka,@Model,@Beygir_Gucu)");
                    com.Connection = baglanti;
                    baglanti.Open();
                    if (baglanti.State == ConnectionState.Open)
                    {
                        
                        com.Parameters.Add("@Marka", OleDbType.VarChar).Value = DtView[0][1];           
                        com.Parameters.Add("@Model", OleDbType.VarChar).Value = DtView[0][2];           
                        com.Parameters.Add("@Beygir_Gucu", OleDbType.VarChar).Value = DtView[0][3];     

                        try
                        {
                            com.ExecuteNonQuery(); 
                            MessageBox.Show("Veri Eklendi");
                            baglanti.Close();
                            VeriGoruntule();
                        }
                        catch (OleDbException ex)
                        {
                            MessageBox.Show(ex.Source);
                            baglanti.Close(); 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bağlantı Hatası");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
