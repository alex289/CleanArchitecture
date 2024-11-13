var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CleanArchitecture_Api>("cleanarchitecture-api");

builder.Build().Run();
