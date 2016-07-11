using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace GvGStats
{
    class DatabaseHandler
    {
        #region Variables

        public SQLiteConnection dataConnection;
        public SQLiteDataReader dataReader;

        private string connectionString = "Data Source = gvgdata.db; Version=3;"; // Connects to database

        #endregion

        public DatabaseHandler()
        {
            // Update player wins and losses
            UpdateAllWinsAndLosses();
        }

        #region Methods

        public void OpenConnection()
        {
            dataConnection = new SQLiteConnection(connectionString);
            dataConnection.Open();
        }

        public void CloseConnection()
        {
            dataConnection.Close();
        }
        

        /// <summary>
        /// Takes in and sends a SQL query then sets the dataReader to the information retrieved
        /// </summary>
        /// <param name="query">Query to the database</param>
        public void SendQuery(string query)
        {
            SQLiteCommand createCommand = new SQLiteCommand(query, dataConnection);
            dataReader = createCommand.ExecuteReader();
        }


        /// <summary>
        /// Adds a player to the Players table in database
        /// </summary>
        /// <param name="inputName">Player name</param>
        /// <param name="inputRole">Role of player</param>
        public void AddPlayerToDatabase(string inputName, string inputRole)
        {
            using (dataConnection = new SQLiteConnection(connectionString))
            {
                dataConnection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(dataConnection))
                {
                    cmd.CommandText = 
                        "INSERT INTO Players (ID, Name, Role, Wins, Losses) VALUES(@id, @name, @role, @wins, @losses)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@id", null); // Auto-increments
                    cmd.Parameters.AddWithValue("@name", inputName);
                    cmd.Parameters.AddWithValue("@role", inputRole);
                    cmd.Parameters.AddWithValue("@wins", 0);
                    cmd.Parameters.AddWithValue("@losses", 0);

                    cmd.ExecuteNonQuery();
                }

                // Confirmation Window
                System.Windows.MessageBox.Show("Player Added");

                dataConnection.Close();
            }
        }


        /// <summary>
        /// Adds a match record to the MatchStats database
        /// </summary>
        /// <param name="winOrLoss"></param>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <param name="player3"></param>
        /// <param name="player4"></param>
        /// <param name="player5"></param>
        public void AddMatchRecordToDatabase(string winOrLoss, string player1, string player2, 
            string player3, string player4, string player5)
        {
            using (dataConnection = new SQLiteConnection(connectionString))
            {
                dataConnection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(dataConnection))
                {
                    cmd.CommandText =
                        "INSERT INTO MatchStats (GameID, WinOrLoss, Player1, Player2, Player3, Player4, Player5) " +
                        "VALUES(@id, @winorloss, @p1, @p2, @p3, @p4, @p5)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@id", null); // Auto-increments
                    cmd.Parameters.AddWithValue("@winorloss", winOrLoss);
                    cmd.Parameters.AddWithValue("@p1", player1);
                    cmd.Parameters.AddWithValue("@p2", player2);
                    cmd.Parameters.AddWithValue("@p3", player3);
                    cmd.Parameters.AddWithValue("@p4", player4);
                    cmd.Parameters.AddWithValue("@p5", player5);

                    cmd.ExecuteNonQuery();
                }

                // Confirmation Window
                System.Windows.MessageBox.Show("Match Record Added");

                dataConnection.Close();
            }
        }


        /// <summary>
        /// Populates a list with current player names from database
        /// </summary>
        /// <returns>a List of player names</returns>
        public List<string> GetListOfPlayerNames()
        {
            List<string> playerNames = new List<string>();

            OpenConnection();

            SendQuery("SELECT Name FROM Players ORDER BY Name");

            while (dataReader.Read())
            {
                playerNames.Add(dataReader.GetString(0));
            }

            CloseConnection();

            return playerNames;
        }


        /// <summary>
        /// Returns a DataTable consisting of the entire Players table
        /// </summary>
        /// <returns></returns>
        public DataTable DisplayPlayerData()
        {
            OpenConnection();

            string cmdString = "SELECT * FROM Players";
            SQLiteCommand cmd = new SQLiteCommand(cmdString, dataConnection);

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(cmd);

            DataTable dt = new DataTable("PlayersList");

            dataAdapter.Fill(dt);

            return dt;
        }


        /// <summary>
        /// Updates values in Players table for Wins and Losses based on MatchStats table
        /// </summary>
        /// <param name="playerName">Player name to check</param>
        public void UpdatePlayerWinsAndLosses(string playerName)
        {
            OpenConnection();

            int wins = 0;
            int losses = 0;

            SendQuery(
                "SELECT WinOrLoss FROM MatchStats WHERE " +
                "Player1='" + playerName + "' OR " +
                "Player2='" + playerName + "' OR " +
                "Player3='" + playerName + "' OR " +
                "Player4='" + playerName + "' OR " +
                "Player5='" + playerName + "'"
                );

            while (dataReader.Read())
            {
                if (dataReader.GetString(0) == "Win")
                {
                    wins++;
                }
                else if (dataReader.GetString(0) == "Loss")
                {
                    losses++;
                }
            }

            CloseConnection();


            // Update Player Wins and Losses
            using (dataConnection = new SQLiteConnection(connectionString))
            {
                dataConnection.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(dataConnection))
                {
                    cmd.CommandText =
                        "UPDATE Players SET Wins=" + wins + ", Losses=" + losses +
                        " WHERE Name='" + playerName + "'";            
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }


                dataConnection.Close();
            }
        }


        /// <summary>
        /// Updates All Player Win/Loss data
        /// </summary>
        public void UpdateAllWinsAndLosses()
        {
            List<string> playerList = new List<string>();

            playerList = GetListOfPlayerNames();

            foreach (string name in playerList)
            {
                UpdatePlayerWinsAndLosses(name);
            }
        }

        #endregion

    }
}
