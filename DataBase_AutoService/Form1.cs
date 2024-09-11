using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBase_AutoService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (FSWork.IsFileExist("AutoServise.db"))            
                MakeStore();                  
            FillMechanicsNames();
        }
        private void MakeStore() // Метод для создания "хранилища"
        {
            if (DBWork.MakeDB())
                MessageBox.Show($"База данных существует");
        }
        private void FillMechanicsNames() // Метод заполнения имён мастеров
        {
            foreach (var name in DBWork.GetMechanics())
            {
                cmbMechanic.Items.Add(name);
            }            
        }
    }
}
