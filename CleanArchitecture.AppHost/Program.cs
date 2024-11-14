var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("Redis");

var rabbitPasswordRessource = new ParameterResource("password", _ => "guest");
var rabbitPasswordParameter = 
    builder.AddParameter("username", rabbitPasswordRessource.Value);

var rabbitMq = builder
    .AddRabbitMQ("RabbitMq", null, rabbitPasswordParameter, 5672)
    .WithManagementPlugin();

var sqlServer = builder.AddSqlServer("SqlServer");

builder.AddProject<Projects.CleanArchitecture_Api>("CleanArchitecture.Api")
    .WithHttpsEndpoint(17270)
    .WithHealthCheck("Api Health")
    .WithOtlpExporter()
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithReference(sqlServer)
    .WaitFor(sqlServer);

builder.Build().Run();
