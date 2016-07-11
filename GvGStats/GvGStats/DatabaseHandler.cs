using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                dataConnection.Close();
            }
        }

        #endregion

    }
}
