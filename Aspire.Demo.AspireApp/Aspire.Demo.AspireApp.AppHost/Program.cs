var builder = DistributedApplication.CreateBuilder(args);





var postgresdb = builder.AddPostgres("pg")
    .AddDatabase("postgresdb");

var cache = builder.AddRedisContainer("redis");

var api = builder.AddProject<Projects.Api>("apiservice")
    .WithReference(cache);

var web = builder.AddProject<Projects.Web>("webservice")
    .WithReference(api)
    .WithReference(postgresdb)
    .WithReference(cache);

builder.Build().Run();