using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using QuartzNetWebConsole.Controllers;
using System.Diagnostics;

namespace QuartzNetWebConsole.Tests
{
    [TestFixture]
    public class ControllerFactoryTests
    {
        [Test]
        public void Should_Return_Index_Controller_When_Index_Url_Is_Supplied()
        {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/index");

            Assert.That(handlerType, Is.EqualTo(typeof(IndexController)));
        }

        [Test]
        public void Should_Ignore_Extension_When_Determining_Controller()
        {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/index.ashx");

            Assert.That(handlerType, Is.EqualTo(typeof(IndexController)));
        }

        [Test]
        public void Should_Return_Index_Controller_For_Default_Route()
        {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/");

            Assert.That(handlerType, Is.EqualTo(typeof(IndexController)));
        }

        [Test]
        public void Should_Return_Scheduler_Controller_When_Scheduler_Url_Is_Supplied()
        {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/scheduler");

            Assert.That(handlerType, Is.EqualTo(typeof(SchedulerController)));
        }
    }
}
