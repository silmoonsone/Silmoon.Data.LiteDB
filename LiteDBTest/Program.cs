using LiteDB;
using Silmoon.Data.LiteDB;
using Silmoon.Extension;
using System.Diagnostics;
using ObjectId = MongoDB.Bson.ObjectId;
// See https://aka.ms/new-console-template for more information


var dbPath = "test.db";

using LiteDBService liteDBService = new LiteDBService(dbPath);

List<Task> tasks = [];

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Run(() =>
    {
        TestInsert(liteDBService, 100);
    }));
}

Task.WaitAll([.. tasks]);


static void TestInsert(LiteDBService liteDBService, int count = 1000)
{
    Console.WriteLine("start insert ...");
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task.Delay(100).Wait();
    for (int i = 0; i < count; i++)
    {
        var user = new DataObject
        {
            _id = ObjectId.GenerateNewId(),
            Username = "admin" + i,
            Password = "admin" + i,
            Data1 = "1",
            Data2 = "",
            created_at = DateTime.Now.AddDays(i),
        };

        liteDBService.Add(user);
        //Console.WriteLine("insert " + i);
    }
    stopwatch.Stop();
    Console.WriteLine($"Insert {count}, time: {stopwatch.ElapsedMilliseconds - 100}ms");
}