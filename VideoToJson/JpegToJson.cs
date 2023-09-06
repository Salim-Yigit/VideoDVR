using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using Amazon.Runtime.Documents;

namespace VideoToJson
{
    class JpegToJson
    {
        public static void ImagetoJson(string txtPath)
        {
            string imageFolderPath = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\yeni"; // Klasör yolunu ayarlayın

            // MongoDB bağlantı bilgilerini ayarlayın
            string connectionString = "mongodb://localhost:27017"; // MongoDB sunucu bağlantı adresi
            string databaseName = "LiveVideo";
            string collectionName = "Frames";
            int maxSize = 1000;

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

            // Klasördeki tüm JPEG dosyalarını al
            string[] imageFiles = File.ReadAllLines(txtPath);

            foreach (string imageFilePath in imageFiles)
            {
                // Check if the image file exists
                if (!File.Exists(imageFilePath))
                {
                    Console.WriteLine($"Image file not found: {imageFilePath}");
                    continue;
                }

                // Read the image file into a byte array
                byte[] imageData = File.ReadAllBytes(imageFilePath);

                // Encode the image data to base64
                string imageBase64 = Convert.ToBase64String(imageData);

                DateTime timestamp = DateTime.Now;

                // Create a JSON object to hold the encoded image data and timestamp
                var imageDataObj = new
                {
                    Image = imageBase64,
                    Timestamp = timestamp
                };
                // Create a JSON object to hold the encoded image data
                //var imageDataObj = new { Image = imageBase64 };

                // Serialize the JSON object to a string
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(imageDataObj);

                BsonDocument bsonDocument = BsonDocument.Parse(json);

                // BsonDocument'i MongoDB koleksiyonuna ekleyin
                if (collection.CountDocuments(FilterDefinition<BsonDocument>.Empty) >= maxSize)
                {
                    // Eğer koleksiyon belirlediğiniz limiti aşıyorsa, en eski belgeyi silin
                    var oldestDocument = collection.Find(FilterDefinition<BsonDocument>.Empty)
                        .Sort(Builders<BsonDocument>.Sort.Ascending("timestamp"))
                        .First();
                    collection.DeleteOne(oldestDocument);
                }

                // Yeni belgeyi ekleyin
                collection.InsertOne(bsonDocument);
                Console.WriteLine($"Image encoded and saved to MongoDB: {imageFilePath}");
            }

            Console.WriteLine("All images processed and saved to MongoDB.");
        }
        public static void deleteImagesFromDatabase()
        {
            //string imageFolderPath = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\yeni"; // Klasör yolunu ayarlayın

            // MongoDB bağlantı bilgilerini ayarlayın
            string connectionString = "mongodb://localhost:27017"; // MongoDB sunucu bağlantı adresi

            IMongoClient mongoClient = new MongoClient(connectionString);
            IMongoDatabase database = mongoClient.GetDatabase("LiveVideo");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Frames");

            var filter = Builders<BsonDocument>.Filter.Empty; // Boş bir filtre ile başlayın
            var sort = Builders<BsonDocument>.Sort.Ascending("timestamp"); // timestamp adlı bir alanı kullanarak sırala
            var options = new FindOptions<BsonDocument, BsonDocument>
            {
                Sort = sort
            };

            var oldestImages = collection.Find(filter)
                .Sort(sort)
                .Limit(1000)
                .ToList();
            foreach (var image in oldestImages)
            {
                var objectId = image["_id"].AsObjectId; // Belgelerin ObjectId'sini alın
                var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", objectId); // ObjectId'ye göre silme filtresi oluşturun
                collection.DeleteOne(deleteFilter); // Belgeyi sil
            }

        }

    }
    
}

