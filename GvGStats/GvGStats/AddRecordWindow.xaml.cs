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
    /// Interaction logic for AddRecordWindow.xaml
    /// </summary>
    public partial class AddRecordWindow : Window
    {
        DatabaseHandler data = new DatabaseHandler();
        List<string> playerNameList = new List<string>();


        public AddRecordWindow()
        {
            InitializeComponent();

            playerNameList = data.GetListOfPlayerNames();

            // Populate Player name combo boxes
            comboBox_Player1.ItemsSource = playerNameList;
            comboBox_Player2.ItemsSource = playerNameList;
            comboBox_Player3.ItemsSource = playerNameList;
            comboBox_Player4.ItemsSource = playerNameList;
            comboBox_Player5.ItemsSource = playerNameList;

            // Populate Win/Loss combobox
            comboBox_WinOrLose.Items.Add("Win");
            comboBox_WinOrLose.Items.Add("Loss");
        }


        #region Button Clicks

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_AddRecord_Click(object sender, RoutedEventArgs e)
        {
            data.AddMatchRecordToDatabase(
                comboBox_WinOrLose.SelectedItem.ToString(),
                comboBox_Player1.SelectedItem.ToString(),
                comboBox_Player2.SelectedItem.ToString(),
                comboBox_Player3.SelectedItem.ToString(),
                comboBox_Player4.SelectedItem.ToString(),
                comboBox_Player5.SelectedItem.ToString()
                );
        }

        #endregion

    }
}
