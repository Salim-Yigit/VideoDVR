using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

public class ImageDisplay
{
    public static void DisplayImagesFromMongoDB()
    {
        var connectionString = "mongodb://localhost:27017";
        string databaseName = "LiveVideo";
        string collectionName = "Frames";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

        // Tüm dokümanları almak için boş bir filtre oluşturun
        var filter = new BsonDocument();

        // Dokümanları koleksiyondan alın
        var documents = collection.Find(filter).ToList();

        foreach (var document in documents)
        {
            // Base64 kodlanmış resmi alın
            var base64Image = document["Image"].AsString;

            // Base64'ü çözerek byte dizisine dönüştürün
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Byte dizisini bir MemoryStream'e yükleyin
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                // MemoryStream'den bir Image nesnesi oluşturun
                Image image = Image.FromStream(ms);

                // Görüntüyü ekranda gösterin
                ShowImage(image);
            }
        }
    }

    public static void ShowImage(Image image)
    {
        // Görüntüyü ekranda göstermek için bir pencere oluşturun
        Form displayForm = new Form();
        displayForm.Text = "MongoDB Image";

        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = image;
        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

        displayForm.Controls.Add(pictureBox);

        // Pencereyi gösterin
        Application.Run(displayForm);
    }
}

