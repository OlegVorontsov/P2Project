using FilesService.Core.Models;
using MongoDB.Driver;

namespace FilesService.Infrastructure.MongoDb;

public class MongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("mongodb");

    public IMongoCollection<FileData> Files
        => _database.GetCollection<FileData>("files");
}