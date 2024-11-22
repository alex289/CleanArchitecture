var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("Redis").WithRedisInsight();

var rabbitPasswordRessource = new ParameterResource("password", _ => "guest");
var rabbitPasswordParameter =
    builder.AddParameter("username", rabbitPasswordRessource.Value);

var rabbitMq = builder
    .AddRabbitMQ("RabbitMq", null, rabbitPasswordParameter, 5672)
    .WithManagementPlugin();

var sqlServer = builder.AddSqlServer("SqlServer");
var db = sqlServer.AddDatabase("Database", "clean-architecture");

builder.AddProject<Projects.CleanArchitecture_Api>("CleanArchitecture-Api")
    .WithOtlpExporter()
    .WithHttpHealthCheck("/health")
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithReference(db)
    .WaitFor(sqlServer);

builder.Build().Run();