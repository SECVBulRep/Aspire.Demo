var builder = DistributedApplication.CreateBuilder(args);


var api = builder.AddProject<Projects.Api>("apiservice");

var web = builder.AddProject<Projects.Web>("webservice")
    .WithReference(api);

builder.Build().Run();