using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            string queryString = "exec procedurename";

            using (SqlConnection connection = new SqlConnection("connectionstring"))
            {
                List<Dictionary<string, string>> hashList = new List<Dictionary<string, string>>();
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();
                command.CommandTimeout = 1000000;


                SqlDataReader reader = command.ExecuteReader();
                List<string> lista = new List<string>();

                while (reader.Read())
                {
                    string text = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i == 0)
                            text = reader[i].ToString().Replace(";", "");
                        else
                            text = text + ";" + reader[i].ToString().Replace(";","");
                    }

                    lista.Add(text);
                    if (lista.Count > 1000)
                    {

                        using (FileStream fs = new FileStream("C:/base.csv", FileMode.Append, FileAccess.Write))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            for (int i = 0; i < lista.Count; i++)
                            {
                               sw.WriteLine( lista[i]);
                            }
                            lista = null;
                            lista = new List<string>();
                        }
                    }
                }
                if (lista.Count > 0)
                {
                    using (FileStream fs = new FileStream("C:/base.csv", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        for (int i = 0; i < lista.Count; i++)
                        {
                            sw.WriteLine(lista[i]);
                        }



                        lista = null;
                        lista = new List<string>();
                    }
                }

                // Call Close when done reading.
                reader.Close();
            }

        }
    }
}
