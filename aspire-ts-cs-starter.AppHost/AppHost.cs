var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo")
    .WithDataVolume()
    .WithMongoExpress();

var mongodb = mongo.AddDatabase("auditing");

var functions = builder.AddAzureFunctionsProject<Projects.aspire_ts_cs_starter_Function>("functions")
    .WithExternalHttpEndpoints()
    .WaitFor(mongodb)
    .WithReference(mongodb);

var server = builder.AddProject<Projects.aspire_ts_cs_starter_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WaitFor(functions)
    .WithReference(functions);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
