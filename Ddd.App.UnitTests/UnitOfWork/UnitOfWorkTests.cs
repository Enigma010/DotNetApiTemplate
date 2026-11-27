using Ddd.App.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ddd.App.UnitTests.UnitOfWork
{
    public class UnitOfWorkTests
    {
        private readonly List<Mock<IUnitOfWork>> _unitOfWorkMocks = new List<Mock<IUnitOfWork>>();
        private Mock<ILogger> _logger;
        private readonly UnitOfWorks _unitOfWorks;
        public UnitOfWorkTests()
        {
            _logger = new Mock<ILogger>();
            var unitOfWork1 = new Mock<IUnitOfWork>();
            var unitOfWork2 = new Mock<IUnitOfWork>();
            _unitOfWorkMocks.Add(unitOfWork1);
            _unitOfWorkMocks.Add(unitOfWork2);
            _unitOfWorks = new UnitOfWorks(_unitOfWorkMocks.Select(m => m.Object), _logger.Object);
        }

        [Fact]
        public async Task RunAction_Success()
        {
            Action action = () => { /* Do nothing */ };
            await _unitOfWorks.RunAsync(action);
            foreach(var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Never);
            }
        }

        [Fact]
        public async Task RunAction_Failure()
        {
            Action action = () => { throw new InvalidOperationException();  };
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _unitOfWorks.RunAsync(action));
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RunAction_RollbackThrows(bool useScopedTransactions)
        {
            Action action = () => { throw new InvalidOperationException(); };
            _unitOfWorkMocks.ForEach(unitOfWorkMock => unitOfWorkMock.SetupGet(uow => uow.UseScopedTransactions).Returns(useScopedTransactions));
            _unitOfWorkMocks[1].Setup(uow => uow.Rollback()).Throws(new InvalidOperationException());
            await Assert.ThrowsAsync<AggregateException>(async () => await _unitOfWorks.RunAsync(action));
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
            }
        }

        [Fact]
        public async Task RunIntFunc_Success()
        {
            int expectedResult = 42;
            Func<Task<int>> func = async () => { await Task.Delay(10); return expectedResult; };
            var result = await _unitOfWorks.RunAsync(func);
            Assert.Equal(expectedResult, result);
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Never);
            }
        }

        [Fact]
        public async Task RunIntFunc_Failure()
        {
            Func<Task<int>> func = async () => { await Task.Delay(10); throw new InvalidOperationException(); };
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _unitOfWorks.RunAsync(func));
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RunIntFunc_RollbackThrows(bool useScopedTransactions)
        {
            Func<Task<int>> func = async () => { throw new InvalidOperationException(); };
            _unitOfWorkMocks.ForEach(unitOfWorkMock => unitOfWorkMock.SetupGet(uow => uow.UseScopedTransactions).Returns(useScopedTransactions));
            _unitOfWorkMocks[1].Setup(uow => uow.Rollback()).Throws(new InvalidOperationException());
            await Assert.ThrowsAsync<AggregateException>(async () => await _unitOfWorks.RunAsync(func));
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RunTaskFunc_RollbackThrows(bool useScopedTransactions)
        {
            Func<Task> func = async () => { throw new InvalidOperationException(); };
            _unitOfWorkMocks.ForEach(unitOfWorkMock => unitOfWorkMock.SetupGet(uow => uow.UseScopedTransactions).Returns(useScopedTransactions));
            _unitOfWorkMocks[1].Setup(uow => uow.Rollback()).Throws(new InvalidOperationException());
            await Assert.ThrowsAsync<AggregateException>(async () => await _unitOfWorks.RunAsync(func));
            foreach (var unitOfWorkMock in _unitOfWorkMocks)
            {
                unitOfWorkMock.Verify(uow => uow.Begin(), Times.Once);
                unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
                unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
            }
        }
    }
}
