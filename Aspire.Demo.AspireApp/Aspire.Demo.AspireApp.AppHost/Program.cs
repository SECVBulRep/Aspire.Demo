var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.Aspire_Demo_Web>("web");

builder.Build().Run();