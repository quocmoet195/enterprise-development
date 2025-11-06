var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql")
                   .WithEnvironment("Password", "quocmoet195");

var hospitalDb = mysql.AddDatabase("HospitalDb"); 

var api = builder.AddProject<Projects.Hospital_Api_Host>("api")
                 .WithReference(hospitalDb);

builder.Build().Run();
