using System.Collections.Generic;
using MTCG.DataManagement.Schemas;
using Npgsql;
using WebService_Lib.Attributes;

namespace MTCG.DataManagement.DB
{
    [Component]
    public class PostgresDatabase : IDatabase
    {
        public PostgresDatabase()
        {
            // TODO connect using Resources
            CreateDatabaseIfNotExists();
        }
        
        public void CreateDatabaseIfNotExists()
        {
            // Check if database exists
            // See: https://stackoverflow.com/a/20032567/12347616
            string connStr = "Server=localhost;Port=5432;User Id=postgres;Password=postgres;";
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            string cmdText = "SELECT 1 FROM pg_database WHERE datname='mtcg'";
            using var cmd = new NpgsqlCommand(cmdText, conn);
            var dbExists = cmd.ExecuteScalar() != null;
            
            
            if (dbExists) return;
            // Create database and tables (if not already exists)
            // See: https://stackoverflow.com/a/17840078/12347616

            var m_createdb_cmd = new NpgsqlCommand(@"
    CREATE DATABASE mtcg
        WITH OWNER = postgres
        ENCODING = 'UTF8'
        CONNECTION LIMIT = -1;
    ", conn);
             
            m_createdb_cmd.ExecuteNonQuery();
            
            connStr = "Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=mtcg";
            conn = new NpgsqlConnection(connStr);
            conn.Open();
            
            var createUserSchema = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS userSchema(
        username VARCHAR(256),
        password VARCHAR(256),
        roleStr VARCHAR(256),
        PRIMARY KEY(username)
    )", conn);
            // Autoincrement 
            // See: https://stackoverflow.com/a/787774/12347616
            var createPackageSchema = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS packageSchema(
        id SERIAL,
        PRIMARY KEY(id)
    )", conn);
            
            
            // Foreign Keys
            // See: https://www.postgresqltutorial.com/postgresql-foreign-key/
            var createCardSchema = new NpgsqlCommand(@"
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
    )", conn);
            
            var createStoreSchema = new NpgsqlCommand(@"
    CREATE TABLE IF NOT EXISTS storeSchema(
        id VARCHAR(256),
        trade VARCHAR(256),
        wanted VARCHAR(256),
        damage INTEGER,
        PRIMARY KEY(id),
        CONSTRAINT fk_trade
            FOREIGN KEY(trade)
                REFERENCES cardSchema(id)
    )", conn);
            
            
            
            // Add Constraint later on
            // See: https://kb.objectrocket.com/postgresql/alter-table-add-constraint-how-to-use-constraints-sql-621
            var alterCardSchema = new NpgsqlCommand(@"
    ALTER TABLE cardSchema ADD
    CONSTRAINT fk_store
        FOREIGN KEY(store)
            REFERENCES storeSchema(id)
            ON DELETE SET NULL
    ", conn);
            
            var createStatsSchema = new NpgsqlCommand(@"
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
    )", conn);

            createUserSchema.ExecuteNonQuery();
            createPackageSchema.ExecuteNonQuery();
            createCardSchema.ExecuteNonQuery();
            createStoreSchema.ExecuteNonQuery();
            alterCardSchema.ExecuteNonQuery();
            createStatsSchema.ExecuteNonQuery();
            conn.Close();
        }

        public bool AddPackage(string admin, List<CardSchema> cards)
        {
            throw new System.NotImplementedException();
        }

        public bool AcquirePackage(string username)
        {
            throw new System.NotImplementedException();
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
    }
}