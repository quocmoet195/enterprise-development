var builder = DistributedApplication.CreateBuilder(args);
var password = builder.AddParameter("DatabasePassword");
var mysql = builder.AddMySql("mysql")
                .WithEnvironment("Password", password);

var hospitalDb = mysql.AddDatabase("HospitalDb"); 

var api = builder.AddProject<Projects.Hospital_Api_Host>("api")
            .WithReference(hospitalDb)
            .WaitFor(hospitalDb);

builder.Build().Run();
