var builder = DistributedApplication.CreateBuilder(args);

// Cấu hình MySQL
var password = builder.AddParameter("DatabasePassword");
var mysql = builder.AddMySql("mysql")
                 .WithImage("mysql", "9.4")
                 .WithEnvironment("Password", password);

var hospitalDb = mysql.AddDatabase("HospitalDb");

// Cấu hình NATS
var natsUser = builder.AddParameter("NatsLogin");
var natsPass = builder.AddParameter("NatsPassword");
//var nats = builder
//    .AddNats("hospital-nats", userName: natsUser, password: natsPass)
//    .WithJetStream();
var nats = builder.AddNats("hospital-nats").WithImage("nats", "2.11");

// 1. THÊM GENERATOR SERVICE
// Generator Service tạo contracts và gửi qua NATS, nên nó cần tham chiếu NATS.
var generator = builder.AddProject<Projects.Hospital_Generator_Nats_Host>("generator")
                     .WithReference(nats);

// 2. Cấu hình API Host (Server)
var api = builder.AddProject<Projects.Hospital_Api_Host>("api")
            .WithReference(hospitalDb)
            .WithReference(nats)
            .WaitFor(hospitalDb)
            .WaitFor(nats);

builder.Build().Run();