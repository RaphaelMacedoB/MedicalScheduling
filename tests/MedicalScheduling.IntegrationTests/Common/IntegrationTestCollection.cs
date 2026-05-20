namespace MedicalScheduling.IntegrationTests.Common;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<MedicalSchedulingApiFactory>
{
  public const string Name = "IntegrationTests";
}
