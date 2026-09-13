namespace SimpleDB.Tests;

using System.Linq;

public class SimpleDBIntegrationTests
{
    [Fact]
    public void StoredRecord_CanBeReadAgain()
    {
        string testFilePath =
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.csv");

        try
        {
            IDatabaseRepository<TestRecord> csvDatabase =
           new IDatabaseRepositoryImpl<TestRecord>(testFilePath);

            TestRecord reading = new TestRecord("testUser", "Dog", "123456789");

            csvDatabase.Store(reading);

            var records = csvDatabase.Read();
            var result = records.First();

            Assert.Equal(reading.Author, result.Author);
            Assert.Equal(reading.Observation, result.Observation);
            Assert.Equal(reading.Timestamp, result.Timestamp);
        }

        finally
        {
            File.Delete(testFilePath);
        }

    }
    [Fact]
    public void StoredRecord_AreReadInCorrectOrder()
    {
        string testFilePath =
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.csv");

        try
        {
            IDatabaseRepository<TestRecord> csvDatabase =
           new IDatabaseRepositoryImpl<TestRecord>(testFilePath);

            TestRecord reading1 = new TestRecord("testUser1", "Dog", "111111111");
            TestRecord reading2 = new TestRecord("testUser2", "Cat", "222222222");
            TestRecord reading3 = new TestRecord("testUser3", "Horse", "333333333");
            TestRecord reading4 = new TestRecord("testUser4", "Bird", "444444444");

            csvDatabase.Store(reading1);
            csvDatabase.Store(reading2);
            csvDatabase.Store(reading3);
            csvDatabase.Store(reading4);

            var records = csvDatabase.Read();
            var results = records.ToList();

            Assert.Equal(4, results.Count);

            Assert.Equal(reading1.Author, results[0].Author);
            Assert.Equal(reading1.Observation, results[0].Observation);
            Assert.Equal(reading1.Timestamp, results[0].Timestamp);

            Assert.Equal(reading2.Author, results[1].Author);
            Assert.Equal(reading2.Observation, results[1].Observation);
            Assert.Equal(reading2.Timestamp, results[1].Timestamp);

            Assert.Equal(reading3.Author, results[2].Author);
            Assert.Equal(reading3.Observation, results[2].Observation);
            Assert.Equal(reading3.Timestamp, results[2].Timestamp);

            Assert.Equal(reading4.Author, results[3].Author);
            Assert.Equal(reading4.Observation, results[3].Observation);
            Assert.Equal(reading4.Timestamp, results[3].Timestamp);
        }

        finally
        {
            File.Delete(testFilePath);
        }

    }
    [Fact]
    public void Read_WithLimit_ReturnsCorrectNumberOfRecords()
    {
        string testFilePath =
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.csv");

        try
        {
            IDatabaseRepository<TestRecord> csvDatabase =
           new IDatabaseRepositoryImpl<TestRecord>(testFilePath);

            TestRecord reading1 = new TestRecord("testUser1", "Dog", "111111111");
            TestRecord reading2 = new TestRecord("testUser2", "Cat", "222222222");
            TestRecord reading3 = new TestRecord("testUser3", "Horse", "333333333");
            TestRecord reading4 = new TestRecord("testUser4", "Bird", "444444444");

            csvDatabase.Store(reading1);
            csvDatabase.Store(reading2);
            csvDatabase.Store(reading3);
            csvDatabase.Store(reading4);

            var records = csvDatabase.Read(2);
            var results = records.ToList();

            Assert.Equal(2, results.Count);

            Assert.Equal(reading1.Author, results[0].Author);
            Assert.Equal(reading1.Observation, results[0].Observation);
            Assert.Equal(reading1.Timestamp, results[0].Timestamp);

            Assert.Equal(reading2.Author, results[1].Author);
            Assert.Equal(reading2.Observation, results[1].Observation);
            Assert.Equal(reading2.Timestamp, results[1].Timestamp);

        }

        finally
        {
            File.Delete(testFilePath);
        }

    }

    public class TestRecord
    {
        public string Author { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;

        public TestRecord() {}
        public TestRecord(string author, string observation, string timestamp)
        {
            Author = author;
            Observation = observation;
            Timestamp = timestamp; 
        }
    }

}
