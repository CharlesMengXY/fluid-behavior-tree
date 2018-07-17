﻿using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class DecoratorTest {
        public class DecoratorExample : DecoratorBase {
            public TaskStatus status;

            protected override TaskStatus OnUpdate () {
                return status;
            }
        }

        public class EnabledProperty {
            [Test]
            public void Returns_true_if_child_is_set () {
                var decorator = new DecoratorExample { child = A.TaskStub().Build() };

                Assert.IsTrue(decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_not_set () {
                var decorator = new DecoratorExample();

                Assert.IsFalse(decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_disabled () {
                var decorator = new DecoratorExample { child = A.TaskStub().Build() };
                decorator.child.Enabled.Returns(false);

                Assert.IsFalse(decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false () {
                var decorator = new DecoratorExample { child = A.TaskStub().Build() };
                decorator.Enabled = false;

                Assert.IsFalse(decorator.Enabled);
            }
        }

        public class UpdateMethod {
            [Test]
            public void Sets_LastUpdate_to_returned_status_value () {
                var decorator = new DecoratorExample();
                decorator.status = TaskStatus.Failure;

                decorator.Update();

                Assert.AreEqual(TaskStatus.Failure, decorator.LastStatus);
            }
        }

        public class EndMethod {
            [Test]
            public void Calls_end_on_child () {
                var task = A.TaskStub().Build();
                var decorator = new DecoratorExample { child = task };

                decorator.End();

                task.Received(1).End();
            }
        }

        public class ResetMethod {
            [Test]
            public void Runs_reset_on_child () {
                var task = A.TaskStub().Build();
                var decorator = new DecoratorExample { child = task };

                decorator.Reset();

                task.Received(1).Reset();
            }
        }
    }
}