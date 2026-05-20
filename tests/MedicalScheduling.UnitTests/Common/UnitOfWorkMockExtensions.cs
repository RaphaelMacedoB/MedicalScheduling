using MedicalScheduling.Domain.Repositories;
using Moq;

namespace MedicalScheduling.UnitTests.Common;

public static class UnitOfWorkMockExtensions
{
  public static Mock<IUnitOfWork> CreateUnitOfWorkMock()
  {
    var mock = new Mock<IUnitOfWork>();
    mock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);
    return mock;
  }
}
