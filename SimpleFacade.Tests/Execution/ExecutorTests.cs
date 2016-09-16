using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SimpleFacade.Exceptions;
using SimpleFacade.Execution;

namespace SimpleFacade.Tests.Execution
{
    [TestFixture]
    public class ExecutorTests
    {
        [Test]
        public void ExecutesCommand()
        {
            var executor = new Executor() as IExecutor;

            var result = (string)executor.Execute(new SimpleCommand());

            result.Should().Be("SimpleCommand");
        }

        [Test]
        public void ExecutesVoidCommand()
        {
            var executor = new Executor() as IExecutor;

            var e = Assert.Throws<Exception>(() => executor.Execute(new SimpleVoidCommand()));

            e.Message.Should().Be("SimpleVoidCommand.Execute()");
        }

        [Test]
        public void ExecutesQuery()
        {
            var executor = new Executor() as IExecutor;

            var result = (int)executor.Execute(new SimpleQuery());

            result.Should().Be(3);
        }

        [Test]
        public void Execute_UnwrapsTargetInvocationException()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var customException = Assert.Throws<CustomException>(() => executor.Execute(new ExceptingCommand()));

            customException.Message.Should().Be("Thrown from ExceptingCommand");
        }

        [Test]
        public void ExecutesVoidCommandHandler()
        {
            VoidCommand.WasRun = false;
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            executor.Execute(new VoidCommand());

            VoidCommand.WasRun.Should().BeTrue();
        }

        [Test]
        public void ExecutesCommandHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (int)executor.Execute(new Square { Value = 3 });

            result.Should().Be(9);
        }

        [Test]
        public void ExecutesQuerySingleHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (int)executor.Execute(new Multiply { Value1 = 3, Value2 = 2 });

            result.Should().Be(6);
        }

        [Test]
        public void ExecutesQueryListHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (IList<int>)executor.Execute(new List { Start = 4 });

            result.Should().ContainInOrder(4, 5, 6);
        }

        public class SimpleVoidCommand : Command
        {
            public override void Execute()
            {
                throw new Exception("SimpleVoidCommand.Execute()");
            }
        }

        public class SimpleCommand : Command<string>
        {
            public override string Execute()
            {
                return "SimpleCommand";
            }
        }

        public class SimpleQuery : Query<int>
        {
            public override int Find()
            {
                return 3;
            }
        }

        public class ExceptingCommand : ICommand<int> { }

        public class ExceptingCommandHandler : IHandleCommand<ExceptingCommand, int>
        {
            public int Execute(ExceptingCommand cmd)
            {
                throw new CustomException("Thrown from ExceptingCommand");
            }
        }

        public class CustomException : FacadeException
        {
            public CustomException(string message) : base(message) { }
        }

        public class VoidCommand : ICommand
        {
            public static bool WasRun = false;
        }

        public class VoidCommandHandler : IHandleVoidCommand<VoidCommand>
        {
            public void Execute(VoidCommand cmd)
            {
                VoidCommand.WasRun = true;
            }
        }

        public class Square : ICommand<int>
        {
            public int Value;
        }

        public class SquareHandler : IHandleCommand<Square, int>
        {
            public int Execute(Square cmd)
            {
                return cmd.Value * cmd.Value;
            }
        }

        public class Multiply : IQuery<int>
        {
            public int Value1;
            public int Value2;
        }

        public class MultiplyHandler : IHandleQuery<Multiply, int>
        {
            public int Find(Multiply query)
            {
                return query.Value1 * query.Value2;
            }
        }

        public class List : IQuery<IList<int>>
        {
            public int Start;
        }

        public class ListHandler : IHandleQuery<List, IList<int>>
        {
            public IList<int> Find(List query)
            {
                return new List<int>
                {
                    query.Start,
                    query.Start + 1,
                    query.Start + 2,
                };
            }
        }

        public abstract class AbstractHandler<TCmd, TReturn> : IHandleCommand<TCmd, TReturn>
            where TCmd : ICommand<TReturn>
        {
            public abstract TReturn Execute(TCmd cmd);
        }
    }
}
