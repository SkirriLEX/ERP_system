using System.Data.SqlClient;
using System.Diagnostics;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace erp
{
    //!!! not
    public class DataQ // all Function in our data connection 
    {
        protected readonly string ConnStr;
        public readonly SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();

        public DataQ()
        {
            ConnStr = "tcp.178.136.14.234,1433";
            Builder.DataSource = ConnStr;
            Builder.UserID = "resto1";
            Builder.Password = "RfrPft,fkjFqrj.11"; //WIN-NALRE9SA668
            Builder.InitialCatalog = "ERP_system";
            //Builder.InitialCatalog = "WIN-NALRE9SA668\\SQLEXPRESS";
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

        class InfLogin
        {
            public int tabNumPerson;
            public int loginStr;
            public int pass;

            InfLogin()
            {
                DataQ data = new DataQ();
            }
        }
    }
}