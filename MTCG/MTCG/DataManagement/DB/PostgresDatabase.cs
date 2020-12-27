using System;
using System.Collections.Generic;
using System.Data;
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
        
        public bool AddPackage(List<CardSchema> cards)
        {
            var transaction = BeginTransaction();
            if (transaction == null) return false;
            try
            {
                // Generate new package and return id
                // See: https://stackoverflow.com/a/5765441/12347616
                using var packageCmd = new NpgsqlCommand(
                    "INSERT INTO packageSchema DEFAULT VALUES RETURNING id",
                    conn);
                packageCmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer)
                    {Direction = ParameterDirection.Output});
                packageCmd.ExecuteNonQuery();
                int packageId;
                if (packageCmd.Parameters[0].Value is int value) packageId = value;
                else
                {
                    transaction.Rollback();
                    return false;
                }

                foreach (var card in cards)
                {
                    using var cardCmd = new NpgsqlCommand(
                        "INSERT INTO cardSchema (id, cardname, damage, package, deck) " +
                        "VALUES(@p1, @p2, @p3, @p4, @p5)"
                        , conn);
                    cardCmd.Parameters.AddWithValue("p1", card.Id);
                    cardCmd.Parameters[0].NpgsqlDbType = NpgsqlDbType.Varchar;
                    cardCmd.Parameters.AddWithValue("p2", card.Name);
                    cardCmd.Parameters[1].NpgsqlDbType = NpgsqlDbType.Varchar;
                    cardCmd.Parameters.AddWithValue("p3", card.Damage);
                    cardCmd.Parameters[2].NpgsqlDbType = NpgsqlDbType.Double;
                    cardCmd.Parameters.AddWithValue("p4", packageId);
                    cardCmd.Parameters[3].NpgsqlDbType = NpgsqlDbType.Integer;
                    cardCmd.Parameters.AddWithValue("p5", false);
                    cardCmd.Parameters[4].NpgsqlDbType = NpgsqlDbType.Boolean;
                    cardCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool AcquirePackage(string username)
        {
            throw new System.NotImplementedException();
        }

        public bool AddUser(UserSchema user)
        {
            var transaction = BeginTransaction();
            if (transaction == null) return false;
            try
            {
                using var userSchemaCmd = new NpgsqlCommand(
                    "INSERT INTO userSchema (username, password, roleStr) " +
                    "VALUES(@p1, @p2, @p3)",
                    conn);

                userSchemaCmd.Parameters.AddWithValue("p1", user.Username);
                userSchemaCmd.Parameters[0].NpgsqlDbType = NpgsqlDbType.Varchar;
                userSchemaCmd.Parameters.AddWithValue("p2", user.Password);
                userSchemaCmd.Parameters[1].NpgsqlDbType = NpgsqlDbType.Varchar;
                userSchemaCmd.Parameters.AddWithValue("p3", user.Role.ToString());
                userSchemaCmd.Parameters[2].NpgsqlDbType = NpgsqlDbType.Varchar;

                var stats = new StatsSchema(user.Username);
                using var statsSchemaCmd = new NpgsqlCommand(
                    "INSERT INTO statsSchema " +
                    "VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7)",
                    conn);

                statsSchemaCmd.Parameters.AddWithValue("p1", stats.Username);
                statsSchemaCmd.Parameters[0].NpgsqlDbType = NpgsqlDbType.Varchar;
                statsSchemaCmd.Parameters.AddWithValue("p2", stats.Elo);
                statsSchemaCmd.Parameters[1].NpgsqlDbType = NpgsqlDbType.Integer;
                statsSchemaCmd.Parameters.AddWithValue("p3", stats.Wins);
                statsSchemaCmd.Parameters[2].NpgsqlDbType = NpgsqlDbType.Integer;
                statsSchemaCmd.Parameters.AddWithValue("p4", stats.Looses);
                statsSchemaCmd.Parameters[3].NpgsqlDbType = NpgsqlDbType.Integer;
                statsSchemaCmd.Parameters.AddWithValue("p5", stats.Coins);
                statsSchemaCmd.Parameters[4].NpgsqlDbType = NpgsqlDbType.Integer;
                statsSchemaCmd.Parameters.AddWithValue("p6", stats.Bio);
                statsSchemaCmd.Parameters[5].NpgsqlDbType = NpgsqlDbType.Varchar;
                statsSchemaCmd.Parameters.AddWithValue("p7", stats.Image);
                statsSchemaCmd.Parameters[6].NpgsqlDbType = NpgsqlDbType.Varchar;

                userSchemaCmd.ExecuteNonQuery();
                statsSchemaCmd.ExecuteNonQuery();
                transaction.Commit();

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public UserSchema? GetUser(string username)
        {
            try
            {
                using var cmd = new NpgsqlCommand(
                    "SELECT * FROM userSchema WHERE username=@p1",
                    conn);

                cmd.Parameters.AddWithValue("p1", username);
                cmd.Parameters[0].NpgsqlDbType = NpgsqlDbType.Varchar;

                cmd.Parameters.Add(new NpgsqlParameter("username", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});
                cmd.Parameters.Add(new NpgsqlParameter("password", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});
                cmd.Parameters.Add(new NpgsqlParameter("roleStr", NpgsqlDbType.Varchar)
                    {Direction = ParameterDirection.Output});

                cmd.ExecuteNonQuery();
                if (cmd.Parameters[1].Value != null &&
                    cmd.Parameters[2].Value != null &&
                    cmd.Parameters[3].Value != null)
                {
                    return new UserSchema(
                        (string) cmd.Parameters[1].Value!,
                        (string) cmd.Parameters[2].Value!,
                        (string) cmd.Parameters[3].Value!);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
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
            
            if (dbExists)
            {
                // Close general connection and build new one to database
                conn.Close();
                connString += ";Database=mtcg";
                conn = new NpgsqlConnection(connString);
                conn.Open();
                return;
            };
            
            // Database does not exist; Create database and tables 
            // See: https://stackoverflow.com/a/17840078/12347616
            using (var cmd = new NpgsqlCommand(@"
    CREATE DATABASE mtcg
        WITH OWNER = postgres
        ENCODING = 'UTF8'
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
        damage DOUBLE PRECISION ,
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

        // TODO Remove for debug only
        public void DropMTCG()
        {
            string connString = $"Server={conn.Host};Port={conn.Port};User Id=postgres;Password=postgres;";
            conn.Close();
            conn = new NpgsqlConnection(connString);
            // Open General connection
            conn.Open();
            using var reset = new NpgsqlCommand("DROP DATABASE mtcg", conn);
            reset.ExecuteNonQuery();
            Environment.Exit(1);
        }
    }
}