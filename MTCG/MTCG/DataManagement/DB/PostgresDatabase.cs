using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MTCG.DataManagement.Schemas;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using WebService_Lib.Attributes;

namespace MTCG.DataManagement.DB
{
    [Component]
    public class PostgresDatabase : IDatabase
    {
        private NpgsqlConnection conn = null!;
        
        public PostgresDatabase()
        {
            CreateDatabaseIfNotExists();
        }
        
        public bool AddPackage(string admin, List<CardSchema> cards)
        {
            var transaction = BeginTransaction();
            if (transaction == null) return false;
            // Generate new package and return id
            // See: https://stackoverflow.com/a/5765441/12347616
            using var packageCmd = new NpgsqlCommand(
                "INSERT INTO packageSchema DEFAULT VALUES RETURNING id",
            conn);
            packageCmd.Parameters.Add("id", NpgsqlDbType.Integer);
            packageCmd.ExecuteNonQuery();
            int packageId;
            if (packageCmd.Parameters[0].Value is int value) packageId = value;
            else return false;

            foreach (var card in cards)
            {
                using var cardCmd = new NpgsqlCommand(
                    "INSERT INTO cardSchema (id, cardname, damage, package, deck) " + 
                    "VALUES(@p1, @p2, @p3, @p4, @p5)"
                , conn);
                cardCmd.Parameters.AddWithValue("p1", card.Id);
                cardCmd.Parameters.AddWithValue("p2", card.Name);
                cardCmd.Parameters.AddWithValue("p3", card.Damage);
                cardCmd.Parameters.AddWithValue("p4", packageId);
                cardCmd.Parameters.AddWithValue("p5", false);
                cardCmd.ExecuteNonQuery();
            }
            
            transaction.Commit();
            return true;
        }

        public bool AcquirePackage(string username)
        {
            throw new System.NotImplementedException();
        }

        public bool AddUser(UserSchema user)
        {
            throw new NotImplementedException();
        }

        public UserSchema? GetUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public StatsSchema? GetUserStats(string username)
        {
            throw new System.NotImplementedException();
        }

        public bool EditUserProfile(string username, string bio, string image)
        {
            throw new System.NotImplementedException();
        }

        public List<CardSchema> GetUserDeck(string username)
        {
            throw new System.NotImplementedException();
        }

        public List<CardSchema> GetUserCards(string username)
        {
            throw new System.NotImplementedException();
        }

        public List<StatsSchema> GetScoreboard()
        {
            throw new System.NotImplementedException();
        }

        public List<StoreSchema> GetTradingDeals()
        {
            throw new System.NotImplementedException();
        }

        public bool AddTradingDeal(string username, StoreSchema deal)
        {
            throw new System.NotImplementedException();
        }

        public bool Trade(string username, string myDeal, string otherDeal)
        {
            throw new System.NotImplementedException();
        }
        
        public void CreateDatabaseIfNotExists()
        {
            // Get DB config and read it
            // See: https://stackoverflow.com/a/41021476/12347616
            // And: https://docs.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-5.0
            string user, password, ip;
            long port;

            try
            {
                string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
                string configPath =
                    $"{Path.GetFullPath(Path.Combine(runningPath!, @"..\..\..\"))}Resources\\dbConfig.json";
                if (!File.Exists(configPath)) throw new FileNotFoundException("dbConfig.json not found in Resources folder");
                
                string config = File.ReadAllText(configPath);
                // Deserialize with JSON.NET
                // See: https://www.newtonsoft.com/json/help/html/DeserializeDictionary.htm
                var parsedConfig = JsonConvert.DeserializeObject<Dictionary<string, object>>(config);
                
                if (parsedConfig["user"] == null || parsedConfig["password"] == null || parsedConfig["ip"] == null ||
                    parsedConfig["port"] == null) throw new ArgumentException("Malformed dbConfig.json");
                user = (string) parsedConfig["user"];
                password = (string) parsedConfig["password"];
                ip = (string) parsedConfig["ip"];
                port = (long) parsedConfig["port"];
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Something went wrong reading database config:");
                Console.Error.WriteLine(ex.ToString());
                Console.Error.WriteLine("Using default config.");
                user = "postgres";
                password = "postgres";
                ip = "localhost";
                port = 5432;
            }
            
            // Check if database exists
            // See: https://stackoverflow.com/a/20032567/12347616
            // And: https://www.npgsql.org/doc/index.html
            string connString = $"Server={ip};Port={port};User Id={user};Password={password};";
            conn = new NpgsqlConnection(connString);
            // Open General connection
            conn.Open();
            
            using var cmdChek = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname='mtcg'", conn);
            var dbExists = cmdChek.ExecuteScalar() != null;
            
            if (dbExists) return;
            
            // Database does not exist; Create database and tables 
            // See: https://stackoverflow.com/a/17840078/12347616
            using (var cmd = new NpgsqlCommand(@"
    CREATE DATABASE mtcg
        WITH OWNER = postgres
        ENCODING = 'UTF8'
        CONNECTION LIMIT = -1;
    ", conn))
            {
                cmd.ExecuteNonQuery();
            }
            // Close general connection and build new one to database
            conn.Close();
            connString += ";Database=mtcg";
            conn = new NpgsqlConnection(connString);
            conn.Open();
            
            // Build tables
            using (var cmd = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS userSchema(
        username VARCHAR(256),
        password VARCHAR(256),
        roleStr VARCHAR(256),
        PRIMARY KEY(username)
    )", conn))
            {
                cmd.ExecuteNonQuery();
            }
            
            // Autoincrement 
            // See: https://stackoverflow.com/a/787774/12347616
            using (var cmd = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS packageSchema(
        id SERIAL,
        PRIMARY KEY(id)
    )", conn))
            {
                cmd.ExecuteNonQuery();
            }
            
            // Foreign Keys
            // See: https://www.postgresqltutorial.com/postgresql-foreign-key/
            using (var cmd = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS cardSchema(
        id VARCHAR(256),
        cardname VARCHAR(256),
        damage INTEGER,
        package INTEGER,
        username VARCHAR(256),
        deck BOOLEAN,
        store VARCHAR(256),
        PRIMARY KEY(id),
        CONSTRAINT fk_package
            FOREIGN KEY(package)
                REFERENCES packageSchema(id)
                ON DELETE SET NULL,
        CONSTRAINT fk_user
            FOREIGN KEY(username)
                REFERENCES userSchema(username)
    )", conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS storeSchema(
        id VARCHAR(256),
        trade VARCHAR(256),
        wanted VARCHAR(256),
        damage INTEGER,
        PRIMARY KEY(id),
        CONSTRAINT fk_trade
            FOREIGN KEY(trade)
                REFERENCES cardSchema(id)
    )", conn))
            {
                cmd.ExecuteNonQuery();
            }
            
            // Add Constraint later on
            // See: https://kb.objectrocket.com/postgresql/alter-table-add-constraint-how-to-use-constraints-sql-621
            using (var cmd = new NpgsqlCommand(@"
    ALTER TABLE cardSchema ADD
    CONSTRAINT fk_store
        FOREIGN KEY(store)
            REFERENCES storeSchema(id)
            ON DELETE SET NULL
    ", conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS statsSchema(
        username VARCHAR(256),
        elo INTEGER,
        wins INTEGER,
        looses INTEGER,
        coins INTEGER,
        bio VARCHAR(256),
        image VARCHAR(256),
        PRIMARY KEY(username),
        CONSTRAINT fk_user
            FOREIGN KEY(username)
                REFERENCES userSchema(username)
    )", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Try to start a new transaction.
        /// </summary>
        /// <returns>
        /// Returns a <c>NpgsqlTransaction</c> when a new transaction can be started or
        /// null when not.
        /// </returns>
        private NpgsqlTransaction? BeginTransaction()
        {
            // Try to start new transaction
            for (var i = 0; i < 15; i++)
            {
                try
                {
                    var transaction = conn.BeginTransaction();
                    return transaction;
                }
                catch (InvalidOperationException ex)
                {
                    // Try again later...
                    Thread.Sleep(50);
                }
            }
            return null;
        }
    }
}