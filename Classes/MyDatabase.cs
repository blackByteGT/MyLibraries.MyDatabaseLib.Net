using System.Data.SqlClient;

namespace MyLibraries.MyDatabaseLib.Classes
{
    /// <summary>
    /// Клас для роботи з базою даних
    /// </summary>
    public class MyDatabase
    {
        #region Items
        /// <summary>
        /// Рядок підключення до БД
        /// </summary>
        private string StringConnectionToDB;
        #endregion Items

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringConnection">Рядок підключення до БД</param>
        public MyDatabase(string stringConnection) => StringConnectionToDB = stringConnection;
        #endregion Constructors

        #region Functions
        #region Query
        /// <summary>
        /// Запит з вибіркою
        /// </summary>
        /// <param name="selectionTable">Вибірка у таблиці</param>
        /// <param name="bodyRequest">Текст запиту</param>
        public void QueryWithSampling(ref MyTableDB selectionTable, string bodyRequest)
        {
            if (bodyRequest.Length == 0) return;

            MyTableDB currentTable = new MyTableDB(selectionTable.Name);

            using (SqlConnection connection = new SqlConnection(StringConnectionToDB))
            {
                connection.Open();

                #region Items
                SqlCommand command = new SqlCommand(bodyRequest, connection);
                SqlDataReader reader = command.ExecuteReader();
                int countReader = reader.FieldCount;
                int currentIndexLine = 0;
                #endregion Items

                for (int i = 0; i < countReader; i++) currentTable.AddColumnToTable(reader.GetName(i));

                while (reader.Read())
                {
                    currentTable.AddLineToTable();

                    for (int i = 0; i < countReader; i++)
                    {
                        currentTable.SetFieldValueToTable(reader[i].ToString(), i, currentIndexLine);
                    }

                    currentIndexLine++;
                }

                selectionTable = currentTable; selectionTable.SetUpdateRequest(bodyRequest);

                connection.Close();
            }
        }
        /// <summary>
        /// Запит без вибірки
        /// </summary>
        /// <param name="request">Текст запиту</param>
        public void QueryWithoutSampling(string request)
        {
            if (request.Length == 0) return;

            using (SqlConnection connection = new SqlConnection(StringConnectionToDB))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(request, connection); command.ExecuteReader();

                connection.Close();
            }
        }
        #endregion Query
        #endregion Functions
    }
}
