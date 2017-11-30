using NUnit.Framework;

namespace SimpleFacade.Tests
{
    [TestFixture]
    public class DesignContraintsTests
    {
        [Test]
        public void DependenciesHaveNotChanged()
        {
            var folder = @"..\..\..\..\SimpleFacade\bin";
            var name = "SimpleFacade";

            NugetPackage.VerifyDependencies(folder, name, new string[]
            {
                "System.ComponentModel.Annotations:*",
            });
        }

        [Test]
        public void Testing_DependenciesHaveNotChanged()
        {
            var folder = @"..\..\..\..\SimpleFacade.Testing\bin";
            var name = "SimpleFacade.Testing";

            NugetPackage.VerifyDependencies(folder, name, new string[]
            {
                "SimpleFacade:*",
            });
        }
    }
}
