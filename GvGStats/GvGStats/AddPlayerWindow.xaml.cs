using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GvGStats
{
    /// <summary>
    /// Interaction logic for AddPlayerWindow.xaml
    /// </summary>
    public partial class AddPlayerWindow : Window
    {
        public AddPlayerWindow()
        {
            InitializeComponent();

            // Load combo box
            comboBox_RoleSelector.Items.Add("Melee");
            comboBox_RoleSelector.Items.Add("MeleeCC");
            comboBox_RoleSelector.Items.Add("Mage");
            comboBox_RoleSelector.Items.Add("Runner");
            comboBox_RoleSelector.Items.Add("Healer");
            comboBox_RoleSelector.SelectedIndex = 0; // Set to first option
        }


        #region Button Clicks

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler data = new DatabaseHandler();

            data.AddPlayerToDatabase(textBox_NameEntry.Text, comboBox_RoleSelector.SelectedItem.ToString());
        }

        #endregion
    }
}
