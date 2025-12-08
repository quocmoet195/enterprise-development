var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("DatabasePassword");
var mysql = builder.AddMySql("mysql")
                 .WithImage("mysql", "9.4")
                 .WithEnvironment("Password", password);

var hospitalDb = mysql.AddDatabase("HospitalDb");

var nats = builder.AddNats("hospital-nats").WithImage("nats", "2.11");

var generator = builder.AddProject<Projects.Hospital_Generator_Nats_Host>("generator")
                     .WithReference(nats);

var api = builder.AddProject<Projects.Hospital_Api_Host>("api")
            .WithReference(hospitalDb)
            .WithReference(nats)
            .WaitFor(hospitalDb)
            .WaitFor(nats);

builder.Build().Run();