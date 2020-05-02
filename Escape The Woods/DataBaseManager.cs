using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.IO;
namespace Escape_The_Woods
{
    static class DataBaseManager
    {
        private static string ConnectionString = @"Data Source=desktop-lcumeg0;Initial Catalog=EscapeTheForest; Integrated Security=True";
        private static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            return connection;
        }
        public static void SetupDataBase()
        {
            DropAllTables();
            CreateDBTables();
        }
        public static void DropAllTables()
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(@"DROP TABLE dbo.WoodRecords;
                                                                DROP TABLE dbo.MonkeyRecords;
                                                                DROP TABLE dbo.Logs;
                                                                ", con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public static void CreateDBTables()
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE [dbo].[WoodRecords]
                                                                (
	                                                                [recordid] INT IDENTITY(1,1) PRIMARY KEY, 
                                                                    [woodID] INT NOT NULL, 
                                                                    [treeID] INT NOT NULL, 
                                                                    [x] INT NOT NULL, 
                                                                    [y] INT NOT NULL
                                                                )
                                                                CREATE TABLE [dbo].[MonkeyRecords]
                                                                (
	                                                                [recordID] INT IDENTITY(1,1) PRIMARY KEY, 
                                                                    [monkeyID] INT NOT NULL, 
                                                                    [monkeyName] NCHAR(25) NOT NULL, 
                                                                    [woodID] INT NOT NULL, 
                                                                    [seqnr] INT NOT NULL, 
                                                                    [treeID] INT NOT NULL, 
                                                                    [x] INT NOT NULL, 
                                                                    [y] INT NOT NULL
                                                                )
                                                                CREATE TABLE [dbo].[Logs]
                                                                (
	                                                                [Id] INT IDENTITY(1,1) PRIMARY KEY, 
                                                                    [woodID] INT NULL, 
                                                                    [monkeyID] INT NULL, 
                                                                    [message] NCHAR(255) NULL
                                                                )
                                                                ", con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public static async Task WriteForestToDataBase(Forest forest)
        {
            Console.WriteLine($"Write forest {forest.ID} to database - start");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO dbo.WoodRecords (woodID, treeID, x, y) VALUES (@woodID, @treeID, @x, @y)";
            foreach(Tree tree in forest.Trees)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        await Task.Run(() => connection.Open());
                        command.Parameters.Add(new SqlParameter("@woodID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@treeID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@x", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@y", SqlDbType.Int));
                        command.CommandText = query;
                        command.Parameters["@woodID"].Value = forest.ID;
                        command.Parameters["@treeID"].Value = tree.ID;
                        command.Parameters["@x"].Value = tree.PositionX;
                        command.Parameters["@y"].Value = tree.PositionY;
                        await Task.Run(() => command.ExecuteNonQuery());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

                Console.WriteLine($"Write forest {forest.ID} to database - end");
        }
        public static async Task WriteMonkeyRecordsToDataBase(Monkey monkey, int forestID)
        {
            Console.WriteLine($"Write route to database forest : {forestID}, Monkey : {monkey.Naam} - start");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO dbo.MonkeyRecords (monkeyID, monkeyName, woodID, seqnr , treeID, x, y) VALUES (@monkeyID, @monkeyName, @woodID, @seqnr, @treeID, @x, @y)";
            int seqnr = 0;
            foreach (Tree tree in monkey.VisitedTrees)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        await Task.Run(() => connection.Open());
                        command.Parameters.Add(new SqlParameter("@monkeyID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@monkeyName", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@woodID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@seqnr", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@treeID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@x", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@y", SqlDbType.Int));
                        command.CommandText = query;
                        command.Parameters["@monkeyID"].Value = monkey.ID;
                        command.Parameters["@monkeyName"].Value = monkey.Naam;
                        command.Parameters["@woodID"].Value = forestID;
                        command.Parameters["@seqnr"].Value = seqnr++;
                        command.Parameters["@treeID"].Value = tree.ID;
                        command.Parameters["@x"].Value = tree.PositionX;
                        command.Parameters["@y"].Value = tree.PositionY;
                        await Task.Run(() => command.ExecuteNonQuery());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            Console.WriteLine($"Write route to database forest : {forestID}, Monkey : {monkey.Naam} - end");
            
        }
        public static async Task WriteLogsToDataBase(Forest forest)
        {
            Console.WriteLine($"forest : {forest.ID} writes log - Start");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO dbo.Logs (woodID, monkeyID, message) VALUES (@woodID, @monkeyID, @message)";
            foreach (Monkey monkey in forest.MonkeysInTheForest)
            {
                foreach(Tree tree in monkey.VisitedTrees)
                {
                    string message = $"{monkey.Naam} is now in tree {tree.ID} at location ({tree.PositionX},{tree.PositionY})";
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            await Task.Run(()=> connection.Open());
                            command.Parameters.Add(new SqlParameter("@woodID", SqlDbType.Int));
                            command.Parameters.Add(new SqlParameter("@monkeyID", SqlDbType.Int));
                            command.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar));
                            command.CommandText = query;
                            command.Parameters["@woodID"].Value = forest.ID;
                            command.Parameters["@monkeyID"].Value = monkey.ID;
                            command.Parameters["@message"].Value = message;
                            await Task.Run(()=> command.ExecuteNonQuery());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }

            Console.WriteLine($"forest : {forest.ID} writes log - End");
        }
        public static async Task WriteToTextFile(Forest f)
        {
            string path = @"D:\Hogent\Programmeren\Programmeren 4\Escape The Woods" + $"\\Forest{f.ID}.txt";
            FileInfo fi = new FileInfo(path);
            using (StreamWriter sr = fi.CreateText()) { }
            using (StreamWriter sWriter = new StreamWriter(path))
            {
                foreach(Monkey m in f.MonkeysInTheForest)
                {
                    foreach (Tree tree in m.VisitedTrees)
                    {
                        string line = $"{m.Naam} is in tree {tree.ID} at ({tree.PositionX},{tree.PositionY})";
                        await Task.Run(() => sWriter.WriteLine(line));
                    }
                }
            }
        }
    }
}
