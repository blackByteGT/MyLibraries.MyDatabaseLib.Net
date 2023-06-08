using System;
using System.Collections.Generic;
using MyLibraries.MySystemLib.Classes;

namespace MyLibraries.MyDatabaseLib.Classes
{
    /// <summary>
    /// Клас для роботи з вибіркою із бази даних
    /// </summary>
    public class MyTableDB
    {
        #region Items
        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Name;
        /// <summary>
        /// Стовпці
        /// </summary>
        private List<string> Columns;
        /// <summary>
        /// Список полів
        /// </summary>
        private List<List<string>> Fields;
        /// <summary>
        /// Запит для оновлення
        /// </summary>
        protected string UpdateRequest;
        #endregion Items

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public MyTableDB() => FillFieldsClass("", new List<string>(), new List<List<string>>());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Назва таблиці</param>
        public MyTableDB(string name) => FillFieldsClass(name, new List<string>(), new List<List<string>>());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns">Список назв полів</param>
        public MyTableDB(List<string> columns) => FillFieldsClass("", columns, new List<List<string>>());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns">Список назв полів</param>
        /// <param name="fields">Список значень полів</param>
        public MyTableDB(List<string> columns, List<List<string>> fields) => FillFieldsClass("", columns, fields);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Назва таблиці</param>
        /// <param name="columns">Список назв полів</param>
        /// <param name="fields">Список значень полів</param>
        public MyTableDB(string name, List<string> columns, List<List<string>> fields) => FillFieldsClass(name, columns, fields);
        #endregion Constructor

        #region Functions
        #region Gets
        /// <summary>
        /// Отримати назву таблиці
        /// </summary>
        /// <param name="name">Поточна назва таблиці</param>
        public void GetName(ref string name) => name = this.Name;
        /// <summary>
        /// Отримати список заголовків таблиці
        /// </summary>
        /// <param name="columns">Поточний список заголовків</param>
        public void GetColumns(ref List<string> columns) => columns = this.Columns;
        /// <summary>
        /// Отримати значення поля
        /// </summary>
        /// <param name="field">Поточне значення поля</param>
        /// <param name="nameColumn">Назва колонки</param>
        /// <param name="indexLine">Індекс рядка</param>
        public void GetField(ref string field, string nameColumn, int indexLine = 0)
        {
            bool checkIndexLine = !(indexLine < 0) && indexLine < Fields.Count;
            if (!checkIndexLine || nameColumn.Length == 0) return;

            int indexColumn = -1; MyList.GetIndexValue(ref indexColumn, Columns, nameColumn);
            if (indexColumn == -1) return;

            GetField(ref field, indexColumn, indexLine);
        }
        /// <summary>
        /// Отримати значення поля
        /// </summary>
        /// <param name="field">Поточне значення поля</param>
        /// <param name="indexColumn">Індекс стовпця</param>
        /// <param name="indexLine">Індекс рядка</param>
        public void GetField(ref string field, int indexColumn, int indexLine = 0)
        {
            bool checkIndexColumn = !(indexColumn < 0) && indexColumn < Columns.Count;
            bool checkIndexLine = !(indexLine < 0) && indexLine < Fields.Count;
            if (!checkIndexColumn || !checkIndexLine) return;

            field = Fields[indexLine][indexColumn];
        }
        /// <summary>
        /// Отримати значення поля. 
        /// select nameColumnGetValue from table where nameColumnCondition {symbolEquality} valueFieldCondition
        /// </summary>
        /// <param name="field">Поточне значення полів</param>
        /// <param name="nameColumnCondition">Назва стовпця для пошуку</param>
        /// <param name="nameColumnGetValue">Значення стовпця для пошуку</param>
        /// <param name="valueFieldCondition">Назва стовпця для отримання потрібного значення</param>
        /// <param name="symbolEquality">Дія</param>
        public void GetField(ref string field, string nameColumnGetValue, string nameColumnCondition, string valueFieldCondition, string symbolEquality = "==")
        {
            int
                countColumns = Columns.Count,
                countFields = Fields.Count;

            if (countColumns == 0 || countFields == 0) return;

            int
                lengthNameColumnCondition = nameColumnCondition.Length,
                lengthNameColumnGetValue = nameColumnGetValue.Length,
                lengthValueColumnCondition = valueFieldCondition.Length;

            if (lengthNameColumnCondition == 0 || lengthNameColumnGetValue == 0 || lengthValueColumnCondition == 0) return;

            string newField = "";
            int indexColumnCondition = -1; MyList.GetIndexValue(ref indexColumnCondition, Columns, nameColumnCondition);
            if (indexColumnCondition == -1) return;

            string currentValueField = "";

            for (int i = 0; i < countFields; i++)
            {
                GetField(ref currentValueField, nameColumnCondition, i);

                if (MyMath.CheckForEquality(currentValueField, valueFieldCondition, symbolEquality))
                {
                    GetField(ref newField, nameColumnGetValue, i);
                    field = newField;

                    break;
                }
            }
        }
        /// <summary>
        /// Отримати матрицю значень таблиці
        /// </summary>
        /// <param name="fields">Поточна матриця значень</param>
        public void GetFields(ref List<List<string>> fields) => fields = Fields;
        /// <summary>
        /// В розробці. 
        /// Необхідно виконати: відбір вибірки за умови 256р.
        /// Отримати таблицю значень таблиці
        /// </summary>
        /// <param name="currentTable">Нова таблиця</param>
        /// <param name="requst">Запит до таблиці. Наприклад: select column1, coulumn2 where column1 = 1</param>
        public void GetFields(ref MyTableDB currentTable, string requst)
        {
            int lengthRequst = requst.Length;
            if (lengthRequst == 0) return;



            string endStr = "\0";
            requst += requst[lengthRequst - 1].ToString() == endStr ? "" : endStr;

            const string select = "select", where = "where", charFormat = "X",
                formatRequest1 = select + charFormat,
                formatRequest2 = formatRequest1 + where;

            bool
                checkFormat1 = !MyString.CheckForFormatInString(requst, formatRequest1, charFormat),
                checkFormat2 = !MyString.CheckForFormatInString(requst, formatRequest2, charFormat);

            if (checkFormat1 && checkFormat2) return;



            string columnsFromRequest = ""; MyString.GetSubstring(ref columnsFromRequest, requst, select, where);
            if (columnsFromRequest.Length == 0) return;
            MyString.Remove(ref columnsFromRequest, " ");

            MyTableDB newTable = new MyTableDB();
            List<string> columnsNewTable = new List<string>(); MyString.GetSubstrings(ref columnsNewTable, columnsFromRequest, new List<string>() { ",", endStr });
            int countColumnsNewTable = Columns.Count;

            if (columnsNewTable[0] == "*") columnsNewTable = Columns;
            else
            {
                countColumnsNewTable = columnsNewTable.Count;
                if (countColumnsNewTable == 0 || MyList.CheckForIllegalValueInList(columnsNewTable, Columns)) return;
            }

            newTable.Columns = columnsNewTable;

            int countFields = Fields.Count;
            string currentStr = "";
            List<List<string>> fieldsNewTable = new List<List<string>>();

            if (!checkFormat1)
            {
                for (int i = 0; i < countFields; i++)
                {
                    fieldsNewTable.Add(new List<string>());

                    for (int j = 0; j < countColumnsNewTable; j++)
                    {
                        GetField(ref currentStr, columnsNewTable[j], i);
                        fieldsNewTable[i].Add(currentStr);
                    }
                }

                newTable.Fields = fieldsNewTable;
            }
            else
            {
                // вибірка з умовою
            }

            currentTable = newTable;

        }
        /// <summary>
        /// Отримати кількість стовпців таблиці
        /// </summary>
        /// <param name="currentCount">Поточне значення</param>
        public void GetNumberColumns(ref int currentCount) => currentCount = Columns.Count;
        /// <summary>
        /// Отримати кількість рядків таблиці
        /// </summary>
        /// <param name="currentCount">Поточне значення</param>
        public void GetNumberRows(ref int currentCount) => currentCount = Fields.Count;
        /// <summary>
        /// Отримати значення ствопця
        /// </summary>
        /// <param name="listValue">Поточний список значень</param>
        /// <param name="indexColumn">Індекс стовпця</param>
        public void GetValueColumn(ref List<string> listValue, int indexColumn)
        {
            List<string> newListValue = new List<string>();
            int countFields = Fields.Count;

            for (int i = 0; i < countFields; i++) newListValue.Add(Fields[i][indexColumn]);

            listValue = newListValue;
        }
        /// <summary>
        /// Отримати значення ствопця
        /// </summary>
        /// <param name="listValue">Поточний список значень</param>
        /// <param name="nameColumn">Назва стовпця</param>
        public void GetValueColumn(ref List<string> listValue, string nameColumn)
        {
            if (nameColumn.Length == 0) return;

            int indexColumn = -1; MyList.GetIndexValue(ref indexColumn, Columns, nameColumn);
            if (indexColumn == -1) return;

            GetValueColumn(ref listValue, indexColumn);
        }
        #endregion Gets

        #region Sets
        /// <summary>
        /// Встановити запит для оновлення
        /// </summary>
        /// <param name="updateRequest">Запит для оновлення</param>
        public void SetUpdateRequest(string updateRequest) => UpdateRequest = updateRequest;
        /// <summary>
        /// Встановити значення поля в таблицю
        /// </summary>
        /// <param name="value">Значення поля</param>
        /// <param name="nameColumn">Назва стовпця</param>
        /// <param name="indexLine">Індекс рядка</param>
        public void SetFieldValueToTable(string value, string nameColumn, int indexLine)
        {
            bool checkIndexLine = !(indexLine < 0) && indexLine < Fields.Count;
            if (!checkIndexLine || nameColumn.Length == 0) return;

            int indexColumn = -1; MyList.GetIndexValue(ref indexColumn, Columns, nameColumn);
            if (indexColumn == -1) return;

            SetFieldValueToTable(value, indexColumn, indexLine);
        }
        /// <summary>
        /// Встановити значення поля в таблицю
        /// </summary>
        /// <param name="value">Значення поля</param>
        /// <param name="indexColumn">Індекс стовпця</param>
        /// <param name="indexLine">Індекс рядка</param>
        public void SetFieldValueToTable(string value, int indexColumn, int indexLine)
        {
            bool checkIndexColumn = !(indexColumn < 0) && indexColumn < Columns.Count;
            bool checkIndexLine = !(indexLine < 0) && indexLine < Fields.Count;
            if (!checkIndexColumn || !checkIndexLine) return;

            Fields[indexLine][indexColumn] = value;
        }
        #endregion Sets

        #region Add
        /// <summary>
        /// Добавити стовпець в таблицю по індексу поточного стовпця
        /// </summary>
        /// <param name="nameColumn">Назва нового стовпця</param>
        /// <param name="indexColumn">Індекс стовпця. 
        /// Якщо такого стовпця не знайдено дія відміняється</param>
        /// <param name="addAfterCurrentColumn">Добавити після поточного стовпця. Якщо false - додає перед поточним стовпцем</param>
        public void AddColumnToTable(string nameColumn, int indexColumn = 0, bool addAfterCurrentColumn = true)
        {
            if (nameColumn.Length == 0) return;

            int countColumns = Columns.Count;
            if (!(indexColumn < countColumns)) indexColumn = 0;

            indexColumn =
                addAfterCurrentColumn ?
                indexColumn == 0 ? countColumns : indexColumn + 1 :
                !(indexColumn > 0) ? 0 : indexColumn - 1;

            MyList.Add(ref Columns, nameColumn, indexColumn);

            List<List<string>> currentFields = new List<List<string>>();
            List<string> currentLineFields = new List<string>();
            int countColumnFields = Fields.Count;

            for (int i = 0; i < countColumnFields; i++)
            {
                currentLineFields = Fields[i];

                MyList.Add(ref currentLineFields, nameColumn, indexColumn);

                currentFields.Add(currentLineFields);
            }

            Fields = currentFields;
        }
        /// <summary>
        /// Добавити стовпець в таблицю по назві поточного стовпця
        /// </summary>
        /// <param name="nameColumn">Назва нового стовпця</param>
        /// <param name="currentColumnName">Поточна назва стовпця. Якщо такого стовпця не знайдено дія відміняється</param>
        /// <param name="addAfterCurrentColumn">Добавити після поточного стовпця. Якщо false - додає перед поточним стовпцем</param>
        public void AddColumnToTable(string nameColumn, string currentColumnName, bool addAfterCurrentColumn = true)
        {
            if (currentColumnName.Length == 0) return;

            int index = -1;
            int countColumns = Columns.Count;

            for (int i = 0; i < countColumns; i++)
                if (Columns[i] == currentColumnName)
                {
                    index = i;
                    break;
                }

            if (index == -1) return;

            AddColumnToTable(nameColumn, index, addAfterCurrentColumn);
        }
        /// <summary>
        /// Добавити рядок в таблицю
        /// </summary>
        /// <param name="line">Поточний рядок</param>
        /// <param name="indexLine">Індекс поточного рядка</param>
        /// <param name="addAfterCurrentLine">Добавлення після поточного рядка. False - перед</param>
        public void AddLineToTable(List<string> line = null, int indexLine = 0, bool addAfterCurrentLine = true)
        {
            int countColumn = Columns.Count;

            if (line == null)
            {
                List<string> nullLine = new List<string>();

                for (int i = 0; i < countColumn; i++) nullLine.Add("");

                line = nullLine;
            }

            int countLine = Fields.Count;

            indexLine =
                addAfterCurrentLine ?
                indexLine == 0 || !(indexLine < countLine) ? countLine : indexLine + 1 :
                !(indexLine > 0) ? 0 : indexLine - 1;

            List<List<string>> newFields = Fields;

            MyList.Add(ref newFields, line, indexLine);

            Fields = newFields;
        }
        #endregion Add

        #region Console
        /// <summary>
        /// Вивести на консоль таблиць зі значеннями
        /// </summary>
        /// <param name="name">Показувати назву таблиці</param>
        /// <param name="columns">Показувати стовпці таблиці</param>
        /// <param name="fields">Показувати значення таблиці</param>
        public void OutputToConsole(bool name = true, bool columns = true, bool fields = true)
        {
            string currentStr = "";
            int maxLengthValue = 0; GetMaxLengthFieldsFromTable(ref maxLengthValue);

            if (name) try
                {
                    Console.WriteLine("\n\tName: " + this.Name + "\n\n");
                }
                catch
                {
                    Console.WriteLine("Не вдалося вивести назву таблиці!");
                }

            if (columns) try
                {
                    int countColumns = this.Columns.Count;
                    Console.Write("\t");

                    for (int i = 0; i < countColumns; i++)
                    {
                        currentStr = this.Columns[i];
                        MyString.GetWithFormat(ref currentStr, maxLengthValue);

                        Console.Write("\t" + currentStr);
                    }

                    Console.WriteLine("\n\n");
                }
                catch
                {
                    Console.Write("Не вдалося вивести назви стовпців!");
                }

            if (fields) try
                {
                    List<string> currentFields = new List<string>();
                    int countColumns = this.Fields.Count;
                    int countFields;

                    for (int i = 0; i < countColumns; i++)
                    {
                        currentFields = this.Fields[i];
                        countFields = currentFields.Count;

                        Console.Write("\t");

                        for (int j = 0; j < countFields; j++)
                        {
                            currentStr = currentFields[j];
                            MyString.GetWithFormat(ref currentStr, maxLengthValue);

                            Console.Write("\t" + currentStr);
                        }

                        Console.WriteLine("\n");
                    }
                }
                catch
                {
                    Console.WriteLine("Не вдалося вивести значення полів таблиці!");
                }

            Console.WriteLine("Натисніть Enter ...");
            Console.ReadLine();
        }
        /// <summary>
        /// Отримати максимальну довжину поля таблиці
        /// </summary>
        /// <param name="length">Поточна найбільша довжина</param>
        public void GetMaxLengthFieldsFromTable(ref int length)
        {
            int currentMaxLength = 0;

            int countColumnColumns = Columns.Count;

            for (int i = 0; i < countColumnColumns; i++) MyMath.Max(ref currentMaxLength, currentMaxLength, Columns[i].Length);

            int countColumnFields = Fields.Count;
            List<string> line = new List<string>();
            int countLineFields = 0;

            for (int i = 0; i < countColumnFields; i++)
            {
                line = Fields[i];
                countLineFields = line.Count;

                for (int j = 0; j < countLineFields; j++) MyMath.Max(ref currentMaxLength, currentMaxLength, line[j].Length);
            }

            length = currentMaxLength;
        }
        #endregion Console

        #region Other
        /// <summary>
        /// Заповнити поля класу
        /// <param name="name">Назва таблиці</param>
        /// <param name="columns">Список назв полів</param>
        /// <param name="fields">Список значень полів</param>
        /// </summary>
        private void FillFieldsClass(string name, List<string> columns, List<List<string>> fields)
        {
            Name = name;
            Columns = columns;
            Fields = fields;
        }
        /// <summary>
        /// Оновити таблицю
        /// </summary>
        /// <param name="database">База даних</param>
        public void Update(MyDatabase database)
        {
            if (UpdateRequest == default) return;

            MyTableDB newTable = new MyTableDB(Name);

            database.QueryWithSampling(ref newTable, UpdateRequest);

            Fields = newTable.Fields;
        }
        #endregion Other
        #endregion Functions
    }
}
