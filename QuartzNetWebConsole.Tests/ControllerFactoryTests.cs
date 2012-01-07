using NUnit.Framework;
using QuartzNetWebConsole.Controllers;

namespace QuartzNetWebConsole.Tests {
    public static class ControllerFactoryTests {
        public static void Should_Return_Index_Controller_When_Index_Url_Is_Supplied() {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/index");

            Assert.That(handlerType, Is.EqualTo(typeof (IndexController)));
        }

        public static void Should_Ignore_Extension_When_Determining_Controller() {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/index.ashx");

            Assert.That(handlerType, Is.EqualTo(typeof (IndexController)));
        }

        public static void Should_Return_Index_Controller_For_Default_Route() {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/");

            Assert.That(handlerType, Is.EqualTo(typeof (IndexController)));
        }

        public static void Should_Return_Scheduler_Controller_When_Scheduler_Url_Is_Supplied() {
            var factory = new ControllerFactory();

            var handlerType = factory.GetHandlerType("http://localhost/quartz/scheduler");

            Assert.That(handlerType, Is.EqualTo(typeof (SchedulerController)));
        }
    }
}