using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SQLDatabaseSeeder
{
    internal class Program
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=realestatedb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();

        }

        private static async Task MainAsync()
        {

            SqlConnection conn = await ConnectingToDb();
            Console.WriteLine("\nPress any keys to start seeding. (WARNING: This will remove all data from the database.)");
            Console.ReadKey();
            Console.Clear();

            await SeedCountries(conn);

            await SeedCities(conn);

            await SeedRoles(conn);  

            await SeedEstateTypes(conn); 

            await ClearAuthTable(conn);
            
            await SeedUsers(conn);
            
            await SeedEstates(conn);

            Console.Read();
        }

        private static async Task<SqlConnection> ConnectingToDb()
        {
            Console.WriteLine("Getting Connection ...");
            //create instanace of database connection
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                Console.WriteLine("Openning Connection ...");

                //open connection
                conn.Open();

                Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return conn;
        }

        private static async Task SeedCountries(SqlConnection conn)
        {
            Console.WriteLine("[SEEDER] Table COUNTRY is now under manipulation.");

            ClearTable("Country",conn);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("INSERT INTO Country VALUES ");
            strBuilder.Append("('Denmark'), ");
            strBuilder.Append("('United Kingdom'),");
            strBuilder.Append("('Hungary')");
            string sqlQuery = strBuilder.ToString();
 
            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) //pass SQL query created above and connection
            {
                command.ExecuteNonQuery(); //execute the Query
                Console.WriteLine("[SEEDER] Table COUNTRY got *3* elements inserted.");
            }
        }

        private static async Task SeedCities(SqlConnection conn)
        {
            Console.WriteLine("\n[SEEDER] Table CITY is now under manipulation.");
            ClearTable("City", conn);


            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("INSERT INTO City VALUES ");
            strBuilder.Append("('Roskilde','4000',1), ");
            strBuilder.Append("('Copenhagen','1050',1), ");
            strBuilder.Append("('Lyngby','2800',1), ");

            strBuilder.Append("('London','E16AN',2), ");
            strBuilder.Append("('Manchester','M1',2), ");
            strBuilder.Append("('Brighton','BN1',2), ");

            strBuilder.Append("('Buda','1007',3), ");
            strBuilder.Append("('Szeged','6700',3), ");
            strBuilder.Append("('Kiskőrös','6200',3) ");

            string sqlQuery = strBuilder.ToString();

            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) 
            {
                command.ExecuteNonQuery(); 
                Console.WriteLine("[SEEDER] Table CITY got *9* elements inserted.");
            }
        }

        private static async Task SeedRoles (SqlConnection conn)
        {
            Console.WriteLine("\n[SEEDER] Table ROLE is now under manipulation.");
            ClearTable("Role", conn);


            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("INSERT INTO Role VALUES ");
            strBuilder.Append("('Admin'), ");
            strBuilder.Append("('Agent'),");
            strBuilder.Append("('Owner')");

            string sqlQuery = strBuilder.ToString();

            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) 
            {
                command.ExecuteNonQuery(); 
                Console.WriteLine("[SEEDER] Table ROLE got *3* elements inserted.");
            }
        }

        private static async Task SeedEstateTypes(SqlConnection conn)
        {
            Console.WriteLine("\n[SEEDER] Table ESTATETYPE is now under manipulation.");

            ClearTable("EstateType", conn);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("INSERT INTO EstateType VALUES ");
            strBuilder.Append("('Detached House'), ");
            strBuilder.Append("('Flat'),");
            strBuilder.Append("('Family House'),");
            strBuilder.Append("('Studio'),");
            strBuilder.Append("('Farm'),");
            strBuilder.Append("('Plot'),");
            strBuilder.Append("('Holiday house'),");
            strBuilder.Append("('Villa')");
            string sqlQuery = strBuilder.ToString();

            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) //pass SQL query created above and connection
            {
                command.ExecuteNonQuery(); //execute the Query
                Console.WriteLine("[SEEDER] Table ESTATETYPE got *8* elements inserted.");
            }
        }

        private static async Task ClearAuthTable(SqlConnection conn)
        {
            Console.WriteLine("\n[SEEDER] Table AUTH is now under manipulation.");

            string deleteQuery = $"DELETE FROM [Auth];";

            using (SqlCommand command = new SqlCommand(deleteQuery, conn))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"[SEEDER] Table AUTH has been cleared.");
            }

            Console.WriteLine("[SEEDER] Data into table AUTH will be auto inserted using a Trigger upon User creation.");
        }

        private static async Task SeedUsers(SqlConnection conn) {
            StringBuilder strBuilder = new StringBuilder();

            Console.WriteLine("\n[SEEDER] Table USER is now under manipulation.");


            ClearTable("User", conn);

            strBuilder.Append("INSERT INTO [User] (firstname,surname,phone_nr,address,tax_nr,ssn,city_id,role_id) VALUES");
            strBuilder.Append("('Thomas','Petersen','+4543671256','Main street 12','123123123','AB123123',2,3),");
            strBuilder.Append("('Peter','Hilsen','+4552525261','Holbaekvej 54','772233345','BC3424126',2,3),");
            strBuilder.Append("('Petra','McDonald','+4575238851','Roskilevej 111','99994563','AS234234',3,3),");

            strBuilder.Append("('Tamás','Bognár','+36703966777','Kerepesi út 123','55663377','AK123543',7,1),");
            strBuilder.Append("('Edit','Szekeres','+36209933456','Mészáros Tamás út 45','123654832','AC123123',7,2),");
            strBuilder.Append("('Tamara','Kovács','+36709234467','Petőfi Sándor utca 77A/2C','91275433','LK324345',7,2),");

            strBuilder.Append("('Adam','Black','+461236734','Oxford st. 123A/B223','93434535','MP213123',4,3)");


            string sqlQuery = strBuilder.ToString();
            using (SqlCommand command = new SqlCommand(sqlQuery, conn)) 
            {
                command.ExecuteNonQuery();
                Console.WriteLine("[SEEDER] Table USER got *8* elements inserted.");
                Console.WriteLine("[SEEDER] Table AUTH got *8* elements inserted. (Fields auto created using a Trigger)");
            }
        }

        private static async Task SeedEstates(SqlConnection conn){
            StringBuilder strBuilder = new StringBuilder();

            Console.WriteLine("\n[SEEDER] Table ESTATE is now under manipulation.");

            ClearTable("Estate", conn);


            strBuilder.Append("INSERT INTO [Estate] (city_id,address, plot_size,building_size,rooms,comment,target_price,minimum_price,estate_type_id,for_sale,for_rent,deposit,rent_price,owner_id,agent_id) VALUES");
            strBuilder.Append("(1,'Kildehusvej 32C',350.50,92.5,4,'Lovely house in the heart of Roskilde.',4200000,4100000,1,'true','false',175000,null,3,6),");
            strBuilder.Append("(2,'Nørrebrosvej 12B',125,46.3,2,'Nice flat for students..',11000000,1050000,2,'true','false',175000,null,2,7),");
            strBuilder.Append("(2,'Nordgade 54',450,114,3,'Lovely house in the heart of Copenhagen.',16500000,16000000,8,'true','false',3000000,null,1,6),");
            strBuilder.Append("(3,'Københavnvej 129E',350.50,64.5,0,'Empty plot near the woods.',1500000,1250000,6,'true','false',200000,null,2,6),");
            strBuilder.Append("(3,'Erikesvej 43C',120,43.73,1,'Studio for rent.',null,null,4,'false','true',12000,null,3,7)");


            string sqlQuery = strBuilder.ToString();
            using (SqlCommand command = new SqlCommand(sqlQuery, conn))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("[SEEDER] Table ESTATE got *5* elements inserted.");
            }


        }




        private static void ClearTable(string TableName, SqlConnection conn) 
        {
            string deleteQuery = $"DELETE FROM [{TableName}]; DBCC CHECKIDENT ('[{TableName}]', RESEED, 0);";

            using (SqlCommand command = new SqlCommand(deleteQuery, conn))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"[SEEDER] Table {TableName.ToUpper()} has been cleared.");
            }


        }


    }
}
