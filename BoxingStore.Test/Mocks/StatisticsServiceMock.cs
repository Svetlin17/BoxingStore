namespace BoxingStore.Test.Mocks
{
    using BoxingStore.Services.Statistics;
    using Moq;

    public static class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.Total())
                    .Returns(new StatisticsServiceModel
                    {
                        TotalProducts = 5,
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}