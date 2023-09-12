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
using ZstdSharp.Unsafe;

namespace VideoToJson
{
    class JpegToJson 
    {
        public static void ImagetoJson(string ImageFolder,int maxSize)
        {
            //string imageFolderPath = "C:\\Users\\yigit\\OneDrive\\Masaüstü\\yeni"; // Klasör yolunu ayarlayın

            // MongoDB bağlantı bilgilerini ayarlayın
            string connectionString = "mongodb://localhost:27017"; // MongoDB sunucu bağlantı adresi
            string databaseName = "LiveVideo";
            string collectionName = "Frames";
            

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            string fileNamePath,tmp;
            // Klasördeki tüm JPEG dosyalarını al

            for (int i = 1; i < maxSize; i++)
            {
                tmp = "frame_" + i.ToString()+".jpg";
                fileNamePath = Path.Combine(ImageFolder,tmp);
                
                if (File.Exists(fileNamePath))
                {
                    byte[] imageData = File.ReadAllBytes(fileNamePath);

                    // Encode the image data to base64
                    string imageBase64 = Convert.ToBase64String(imageData);

                    DateTime timestamp = DateTime.Now;

                    var imageDataObj = new
                    {
                        Image = imageBase64,
                        Timestamp = timestamp
                    };

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(imageDataObj);

                    BsonDocument bsonDocument = BsonDocument.Parse(json);

                    if (collection.CountDocuments(FilterDefinition<BsonDocument>.Empty) >= maxSize)
                    {
                        // Eğer koleksiyon belirlediğiniz limiti aşıyorsa, en eski belgeyi silin
                        var oldestDocument = collection.Find(FilterDefinition<BsonDocument>.Empty)
                            .Sort(Builders<BsonDocument>.Sort.Ascending("timestamp"))
                            .First();
                        collection.DeleteOne(oldestDocument);
                    }

                    collection.InsertOne(bsonDocument);
                }
                
            }

        }
        public static void deleteImagesFromDatabase(int maxSize)
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
                .Limit(maxSize)
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

