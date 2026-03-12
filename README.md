# AutoBlogSeoAi
ücretsiz google api ile otomatik post giren .net 10 uygulaması

## Auto Blog Generator (.NET 10)

Auto Blog Generator, .NET 10 kullanılarak geliştirilmiş, içerik üretimini otomatik hale getirmeyi amaçlayan bir blog yazma uygulamasıdır. Belirlenen anahtar kelimeler veya içerik stratejilerine göre otomatik olarak blog yazıları oluşturabilir ve içerik üretim sürecini büyük ölçüde hızlandırır.

Çalışma mantığı (çalıştırmak için...);

-App_DAta içerisinde ki txt doslarını kendinize göre düzenleyin.

-appsettings.json içeriğini kedni api değeriniz ile değiştirin.

-içerideki .bak dosyası ile sqlinizde db oluşturabilirsiniz  yada dbyi migration ile yaratabilirsiniz.

-sunucunuzun zamanlanmış görevine günlük olarak 
https://lacasadetaki.com/admin/automation/autorun?key=burayadagizlianahtardegeriHerkesHerkesinUygulamasinaSalcaOlmasin
kendi domainde kurulu adresinizi girin 

Bu proje özellikle SEO odaklı içerik üretimi, otomatik blog yönetimi ve dijital içerik sistemleri geliştirmek isteyen geliştiriciler için tasarlanmıştır.



* Özellikler
Otomatik blog yazısı oluşturma
Anahtar kelime (appdata içindeki topics.txt değerleri ) tabanlı içerik üretimi
SEO uyumlu başlık ve metin oluşturma
Modüler ve genişletilebilir mimari
.NET 10 ile yüksek performans
API entegrasyonlarına uygun yapı


* Amaç
Bu projenin amacı, blog içerik üretim sürecini otomatikleştirerek geliştiricilerin ve içerik üreticilerinin zamandan tasarruf etmesini sağlamaktır. Sistem, belirli konular veya anahtar kelimeler üzerinden otomatik olarak içerik oluşturabilir ve içerik üretim pipeline’ını kolaylaştırır.


* Teknolojiler
.NET 10
C#
mssql



* Kullanım
Projeyi klonlayın

git clone https://github.com/yildirim-mehmet/AutoBlogSeoAi.git

Proje dizinine gidin

cd proje-adi

Uygulamayı çalıştırın

dotnet run


* Yapılandırma
Uygulama, içerik üretimi için kullanılacak servisleri ve ayarları configuration dosyaları üzerinden yönetir. Gerekli API anahtarları ve içerik ayarları ilgili config dosyasından düzenlenebilir.

İstek Gelecek Planları

WordPress / CMS entegrasyonu

php uygulumasınıda atılan mesajlara göre yeni bir repoda sunmayı planlıyorum.


* Lisans
  
Bu proje açık kaynak olarak geliştirilmiştir. tacari kullanan arkadaşlardan yıldız ve abonelik ile destekleyebilir...

Eğer projeyi faydalı bulduysanız ⭐ vererek destek olabilirsiniz.



//////////////////////////////////////////////////////////


# AutoBlogSeoAi
A .NET 10 application that automatically generates blog posts using the free Google API.

## Auto Blog Generator (.NET 10)

Auto Blog Generator is a blog writing application developed using .NET 10, designed to automate content creation. It can automatically generate blog posts based on predefined keywords or content strategies, significantly speeding up the content production process.

## How It Works (Setup Instructions)

- Edit the `.txt` files inside the **App_Data** folder according to your needs.
- Replace the values in **appsettings.json** with your own API credentials.
- You can create the database in SQL Server using the included `.bak` file, or generate it via migrations.
- Add a **daily scheduled task** on your server and call the following endpoint:


https://yourdomain.com/admin/automation/autorun?key=your-secret-key


Replace it with your own domain and secret key.

This project is designed especially for developers who want to build **SEO-focused content generation systems, automated blog management tools, and digital publishing platforms**.

---

## Features

- Automatic blog post generation  
- Keyword-based content creation (using values from `App_Data/topics.txt`)  
- SEO-friendly title and content generation  
- Modular and extensible architecture  
- High performance with .NET 10  
- Ready for API integrations  

---

## Purpose

The purpose of this project is to automate the blog content creation process and help developers and content creators save time. The system can automatically generate content based on specific topics or keywords, simplifying the entire content production pipeline.

---

## Technologies

- .NET 10  
- C#  
- Microsoft SQL Server  

---

## Usage

Clone the repository:


git clone https://github.com/yildirim-mehmet/AutoBlogSeoAi.git


Go to the project directory:


cd project-name


Run the application:


dotnet run


---

## Configuration

The application manages the services and settings used for content generation through configuration files. Required API keys and content settings can be edited from the relevant config files.

---

## Future Plans / Feature Requests

- WordPress / CMS integration  
- I also plan to publish the PHP version of the application in a new repository based on user feedback.

---

## License

This project is developed as open source.

If you use it commercially, you can support the project by giving a ⭐ star and subscribing to the repository.

If you find this project useful, please consider supporting it by giving it a ⭐.
