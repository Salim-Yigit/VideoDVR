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

                
                string imageBase64 = Convert.ToBase64String(imageData);

                DateTime timestamp = DateTime.Now;
                

                if(currentMinute > 0)
                {
                    deleteImages(collection, i); 
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
        private static void deleteImages(IMongoCollection<BsonDocument> collection,int index)
        {
            var filter = new BsonDocument("Index", new BsonInt32(index));
            var data = collection.Find(filter).ToList();
            foreach (var path in data)
            {
                File.Delete(path["FileNamePath"].AsString);
            }
            collection.DeleteOne(filter);
        }
        
    }
    
}

