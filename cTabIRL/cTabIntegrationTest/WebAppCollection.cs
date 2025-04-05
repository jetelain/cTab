[assembly: CollectionBehavior(DisableTestParallelization = true)] // Cannot parallelize because extension supports only one connection

namespace cTabIntegrationTest
{
    [CollectionDefinition("cTabWebApp")]
    public class WebAppCollection : ICollectionFixture<WebApp>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
