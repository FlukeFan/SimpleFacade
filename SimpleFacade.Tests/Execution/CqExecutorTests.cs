using System;
using FluentAssertions;
using NUnit.Framework;
using SimpleFacade.Execution;

namespace SimpleFacade.Tests.Execution
{
    [TestFixture]
    public class CqExecutorTests
    {
        [Test]
        public void VoidCommand()
        {
            var executor = new CqExecutor(new Executor());

            var e = Assert.Throws<Exception>(() => executor.Execute(new CqVoidCommand()));

            e.Message.Should().Be("CqVoidCommand.Execute()");
        }

        [Test]
        public void Command()
        {
            var executor = new CqExecutor(new Executor());

            var result = executor.Execute(new CqCommand());

            result.Should().Be("CqCommand");
        }

        [Test]
        public void Query()
        {
            var executor = new CqExecutor(new Executor());

            var result = executor.Execute(new CqQuery());

            result.Should().Be(3);
        }

        public class CqVoidCommand : Command
        {
            public override void Execute()
            {
                throw new Exception("CqVoidCommand.Execute()");
            }
        }

        public class CqCommand : Command<string>
        {
            public override string Execute()
            {
                return "CqCommand";
            }
        }

        public class CqQuery : Query<int>
        {
            public override int Find()
            {
                return 3;
            }
        }
    }
}
