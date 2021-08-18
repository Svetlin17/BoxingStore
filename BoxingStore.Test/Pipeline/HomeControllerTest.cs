namespace BoxingStore.Test.Pipeline
{
    using System.Collections.Generic;
    using BoxingStore.Controllers;
    using BoxingStore.Services.Products;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.Products;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(TenPublicProducts))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<LatestProductServiceModel>>()
                    .Passing(m => m.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}