using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Jurassic.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace UnitTests;

[TestClass]
public class EntityQueryDataObjectTests : TestBase
{
    [TestMethod]
    public void Test()
    {
        string mySelectQuery = "SELECT * FROM `activityconfiguration__questions` LIMIT 10";
        MySqlConnection myConnection = new MySqlConnection("Server=localhost;port=3306;database=vnext.local;uid=root;pwd=RQL*Dev-2000;Max Pool Size=150;Connection Lifetime=180;"); 
        MySqlCommand myCommand = new MySqlCommand(mySelectQuery,myConnection);
        myConnection.Open();
        MySqlDataReader myReader = myCommand.ExecuteReader();
// Always call Read before accessing data.

        var entityData = new EntityQueryDataObject(ScriptEngine);
        var headersRead = false;
        while (myReader.Read()) {
                if (!headersRead)
                {
                        var columnNames = new List<string>();
                        for (var index = 0; index < myReader.FieldCount; index++)
                        { 
                                columnNames.Add(myReader.GetName(index));  
                        }
                        headersRead = true;
                        entityData.AddColumnNames(columnNames); 
                }
                
                entityData.StartNewRow();

                for (var index = 0; index < myReader.FieldCount; index++)
                {
                        entityData.AddColumnData(myReader.GetValue(index));
                }
        }
// always call Close when done reading.
        myReader.Close();
// Close the connection when done with it.
        myConnection.Close();

        var jurassicObject = entityData.ToObjectInstance();
        
        ScriptEngine.SetGlobalValue("resultData",jurassicObject);
        ScriptEngine.SetGlobalValue("resultDataJson","");
        var objectJson = ScriptEngine.Evaluate("resultDataJson = JSON.stringify(resultData)");

        var blah = ScriptEngine.GetGlobalValue("resultDataJson");
        Console.WriteLine(blah.ToString());
    }
}