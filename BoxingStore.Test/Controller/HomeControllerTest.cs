namespace BoxingStore.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using BoxingStore.Controllers;
    using BoxingStore.Services.Products;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.Products;
    using static WebConstants.Cache;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        => MyController<HomeController>
            .Instance(controller => controller
                .WithData(TenPublicProducts))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
                .ContainingEntry(entry => entry
                    .WithKey(LatestProductsCacheKey)
                    .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                    .WithValueOfType<List<LatestProductServiceModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<LatestProductServiceModel>>()
                .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}