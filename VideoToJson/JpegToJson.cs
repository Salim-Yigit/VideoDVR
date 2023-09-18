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
using System.Security.Cryptography;
using System.Threading;

namespace VideoToJson
{
    class JpegToJson 
    {
        public static void ImagetoJson(string ImageFolder,int maxSize,int currentMinute)
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
            
            for (int i = 1; i < maxSize; i++)
            {
                tmp = "frame_" + i.ToString()+".jpg";
                fileNamePath = Path.Combine(ImageFolder,tmp);

                while(!File.Exists(fileNamePath))
                {
                     
                }
                Thread.Sleep(15);
                byte[] imageData = File.ReadAllBytes(fileNamePath);

                // Encode the image data to base64
                string imageBase64 = Convert.ToBase64String(imageData);

                DateTime timestamp = DateTime.Now;
                

                if(currentMinute > 0)
                {
                    var filter = new BsonDocument("Index", new BsonInt32(i));
                    var data = collection.Find(filter).ToList();
                    foreach (var path in data)
                    {
                        File.Delete(path["FileNamePath"].AsString);
                    }
                    collection.DeleteOne(filter);
                }

                var imageDataObj = new
                 {
                        Image = imageBase64,
                        Index = i,
                        Timestamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss"), 
                        FileNamePath = fileNamePath
                 };

                 string json = Newtonsoft.Json.JsonConvert.SerializeObject(imageDataObj);

                 BsonDocument bsonDocument = BsonDocument.Parse(json);
                           
                 collection.InsertOne(bsonDocument);
                 
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

