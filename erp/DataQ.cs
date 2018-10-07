using System.Data.SqlClient;
using System.Diagnostics;

namespace erp
{
    //!!! not
    public class DataQ // all Function in our data connection 
    {
        protected readonly string ConnStr;
        public readonly SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();

        public DataQ()
        {
            ConnStr = "WIN-NALRE9SA668";
            Builder.DataSource = ConnStr;
            Builder.UserID = "resto";
            Builder.Password = "RfrPft,fkjFqrj.11";//178.136.14.234:1433
            Builder.InitialCatalog = "WIN-NALRE9SA668\\SQLEXPRESS";
        }
        public static void DisplaySqlErrors(SqlException exception)
        {
            for (var i = 0; i < exception.Errors.Count; i++)
            {
                Debug.WriteLine("Index #" + i + "\n" +
                                  "Source: " + exception.Errors[i].Source + "\n" +
                                  "Number: " + exception.Errors[i].Number.ToString() + "\n" +
                                  "State: " + exception.Errors[i].State.ToString() + "\n" +
                                  "Class: " + exception.Errors[i].Class.ToString() + "\n" +
                                  "Server: " + exception.Errors[i].Server + "\n" +
                                  "Message: " + exception.Errors[i].Message + "\n" +
                                  "Procedure: " + exception.Errors[i].Procedure + "\n" +
                                  "LineNumber: " + exception.Errors[i].LineNumber.ToString());
            }
        }

        internal class InfLogin
        {
            public int tabNumPerson;
            public int loginStr;
            public int pass;

            InfLogin()
            {
                DataQ data = new DataQ();
            }
        }

        //Get request
        //Add eleement

    }
}
