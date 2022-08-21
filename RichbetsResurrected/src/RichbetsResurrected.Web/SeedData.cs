using Microsoft.EntityFrameworkCore;
using RichbetsResurrected.Entities.DatabaseEntities.Shop;
using RichbetsResurrected.Identity.Contexts;

namespace RichbetsResurrected.Web;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = serviceProvider.GetRequiredService<AppDbContext>())
        {
            var stylingCategory = new Category
            {
                Name = "User Styling", Description = "Styling for your user", ImageUrl = "123"
            };
            
            dbContext.Categories.Add(stylingCategory);
            dbContext.SaveChanges();
            
            var effectSubCategory = new SubCategory
            {
                Name = "Effects", Description = "Effects for your user", CategoryId = stylingCategory.Id
            };
            
            var patternSubCategory = new SubCategory
            {
                Name = "Patterns", Description = "Patterns for your user", CategoryId = stylingCategory.Id
            };
            
            var bannerSubCategory = new SubCategory
            {
                Name = "Banners", Description = "Banners for your user", CategoryId = stylingCategory.Id
            };
            
            var animatedSubCategory = new SubCategory
            {
                Name = "Animated", Description = "Animated for your user", CategoryId = stylingCategory.Id
            };
            
            dbContext.SubCategories.AddRange(effectSubCategory, patternSubCategory, bannerSubCategory, animatedSubCategory);
            dbContext.SaveChanges();
            
            
            // Pattern items
            var pinkLeopard = new Item
            {
                Name = "Pink Leopard", Description = "pink_leopard", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.pinimg.com/736x/b4/ab/5d/b4ab5dec71a33a5e81198915dd517718.jpg"
            };
            
            var tiger = new Item
            {
                Name = "Tiger", Description = "tiger", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.postimg.cc/Bvw94dgD/tiger.jpg"
            };
            
            var giraffe = new Item
            {
                Name = "Giraffe", Description = "giraffe", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://murals-weblinc.netdna-ssl.com/product_images/giraffe-print-10263661/5ec82fa4bd89dd0018f779d1/product_large_image.jpg?c=1590177700"
            };

            var poland = new Item
            {
                Name = "Poland", Description = "poland2", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/1/12/Flag_of_Poland.svg/1200px-Flag_of_Poland.svg.png"
            };
            
            var fade = new Item
            {
                Name = "Fade", Description = "fade", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://ih1.redbubble.net/image.213300789.4460/flat,750x1000,075,f.u2.jpg"
            };
            
            var marbleFade = new Item
            {
                Name = "Marble Fade", Description = "marble-fade", Price = 100, SubCategoryId = patternSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.imgur.com/Dik8l2g.png"
            };
            
            
            
            // Baner items
            var strawberries = new Item
            {
                Name = "Strawberries", Description = "strawberries", Price = 100, SubCategoryId = bannerSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://static.vecteezy.com/system/resources/previews/002/859/881/large_2x/a-pattern-background-with-small-and-cute-strawberries-and-flowers-and-leaves-arranged-randomly-simple-pattern-design-template-free-vector.jpg"
            };
            
            var cash = new Item
            {
                Name = "Cash", Description = "cash", Price = 100, SubCategoryId = bannerSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://thumbs.dreamstime.com/b/money-cash-pattern-28000619.jpg"
            };
            
            var polishFlag = new Item
            {
                Name = "Polish Flag", Description = "poland", Price = 100, SubCategoryId = bannerSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/1/12/Flag_of_Poland.svg/1200px-Flag_of_Poland.svg.png"
            };
            
            var fastfood = new Item
            {
                Name = "Fastfood", Description = "fastfood", Price = 100, SubCategoryId = bannerSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://www.creativefabrica.com/wp-content/uploads/2020/11/01/Fast-Food-Seamless-Pattern-1-Graphics-6399037-1.jpg"
            };
            
            var redline = new Item
            {
                Name = "Redline", Description = "redline", Price = 100, SubCategoryId = bannerSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.postimg.cc/90MQb6TD/redline.jpg"
            };
            
            
            // Animated items
            var fire = new Item
            {
                Name = "Fire", Description = "anim-fire", Price = 100, SubCategoryId = animatedSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/22/Animated_fire_by_nevit.gif"
            };
            
            var epilepsy = new Item
            {
                Name = "Epilepsy", Description = "anim-epilepsy", Price = 100, SubCategoryId = animatedSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.pinimg.com/originals/92/b8/19/92b819465492e2f9f1a385cf7916d3a0.gif"
            };
            
            var colorWave = new Item
            {
                Name = "Color Wave", Description = "anim-color-wave", Price = 100, SubCategoryId = animatedSubCategory.Id, AvailableQuantity = -1, 
                IsAvailable = true, ImageUrl = "https://i.pinimg.com/originals/84/d8/7e/84d87eb7e536135161c55887d878d44b.gif"
            };
            
            
            
            // Seed items
            dbContext.Items.AddRange(pinkLeopard, tiger, giraffe, poland, fade, marbleFade, strawberries, cash, polishFlag, fastfood, redline, 
                fire, epilepsy, colorWave);
            dbContext.SaveChanges();
            
            var patternItemTypes = new List<ItemType>
            {
                new() { ItemId = pinkLeopard.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
                new() { ItemId = tiger.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
                new() { ItemId = giraffe.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
                new() { ItemId = poland.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
                new() { ItemId = fade.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
                new() { ItemId = marbleFade.Id, IsEquippable = true, IsNicknamePattern = true, IsUnique = true },
            };
            
            var banerItemTypes = new List<ItemType>
            {
                new() { ItemId = strawberries.Id, IsEquippable = true, IsNicknameBanner = true, IsUnique = true },
                new() { ItemId = cash.Id, IsEquippable = true, IsNicknameBanner = true, IsUnique = true },
                new() { ItemId = polishFlag.Id, IsEquippable = true, IsNicknameBanner = true, IsUnique = true },
                new() { ItemId = fastfood.Id, IsEquippable = true, IsNicknameBanner = true, IsUnique = true },
                new() { ItemId = redline.Id, IsEquippable = true, IsNicknameBanner = true, IsUnique = true },
            };
            
            var animatedItemTypes = new List<ItemType>
            {
                new() { ItemId = fire.Id, IsEquippable = true, IsNicknameAnimation = true, IsUnique = true },
                new() { ItemId = epilepsy.Id, IsEquippable = true, IsNicknameAnimation = true, IsUnique = true },
                new() { ItemId = colorWave.Id, IsEquippable = true, IsNicknameAnimation = true, IsUnique = true },
            };
            
            dbContext.ItemTypes.AddRange(patternItemTypes);
            dbContext.ItemTypes.AddRange(banerItemTypes);
            dbContext.ItemTypes.AddRange(animatedItemTypes);
            dbContext.SaveChanges();
            
        }
    }
}