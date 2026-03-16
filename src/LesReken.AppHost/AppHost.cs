var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddProject<Projects.LesReken_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
