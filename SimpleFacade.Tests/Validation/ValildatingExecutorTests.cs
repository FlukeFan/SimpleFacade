using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using SimpleFacade.Exceptions;
using SimpleFacade.Execution;
using SimpleFacade.Validation;

namespace SimpleFacade.Tests.Validation
{
    [TestFixture]
    public class ValildatingExecutorTests
    {
        [Test]
        public void Execute_ValidatesDto()
        {
            Action act = () =>
            {
                var executor = (IExecutor)new ValidatingExecutor(new Executor());
                executor.Execute(new Dto { Name = null });
            };

            act.ShouldThrow<FacadeException>();
        }

        public class Dto
        {
            [Required]
            public string Name { get; set; }
        }
    }
}
