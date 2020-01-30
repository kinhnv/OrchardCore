using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Media",
    Author = "The BDS Team",
    Website = "https://batdongsan.com.vn",
    Version = "2.0.0"
)]

[assembly: Feature(
    Id = "CMS_BDS.Media",
    Name = "Media",
    Description = "The media module adds media management support.",
    Dependencies = new[]
    {
        "OrchardCore.ContentTypes"
    },
    Category = "batdongsan.com.vn"
)]

[assembly: Feature(
    Id = "CMS_BDS.Media.Cache",
    Name = "Media Cache",
    Description = "The media cache module adds remote file store cache support.",
    Dependencies = new[]
    {
        "CMS_BDS.Media"
    },
    Category = "batdongsan.com.vn"
)]
