using System;
using HandyCrab.Business.Tests;
using HandyCrab.Business.Tests.TestBusiness;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ScopedTestMethodAttribute : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            using (TestScope.GetScope())
            {
                return base.Execute(testMethod);
            }
        }
    }
}