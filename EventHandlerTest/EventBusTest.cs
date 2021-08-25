using System;
using EventHandler;
using EventHandler.Interfaces;
using NUnit.Framework;

namespace EventBusTest
{
    public class EventBusImplTest
    {
        private bool _isInvoke;
        private string _order;

        [SetUp]
        public void SetUp()
        {
            _isInvoke = false;
            _order = "";
        }

        [Test]
        public void Invoke_Specific_Action_After_Publish()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestSpecificInvoke);
            // Act
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsTrue(_isInvoke);
        }

        [Test]
        public void Invoke_General_Action_After_publish()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestGeneralInvoke);
            // Act
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsTrue(_isInvoke);
        }
        
        [Test]
        public void Invoke_General_Action_Subscribed_In_Run_Time_After_Publish()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe(typeof(OnTest1), TestGeneralInvoke);
            // Act
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsTrue(_isInvoke);
        }

        [Test]
        public void Throw_Exception_If_Subscribed_Type_Is_Not_Derived_From_IEventBase()
        {
            // Arrange
            var eventBus = new EventBus();
            // Act
            void Action() => eventBus.Subscribe(typeof(OnNotIEventBase), TestGeneralInvoke);
            //Assert
            Assert.Throws(typeof(ArgumentException), Action);
        }

        [Test]
        public void Not_Invoke_Specific_Action_After_Unsubscribe()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestSpecificInvoke);
            // Act
            eventBus.Unsubscribe<OnTest1>(TestSpecificInvoke);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsFalse(_isInvoke);
        }

        [Test]
        public void Not_Invoke_General_Action_After_Unsubscribe()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestGeneralInvoke);
            // Act
            eventBus.Unsubscribe<OnTest1>(TestGeneralInvoke);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsFalse(_isInvoke);
        }
        
        [Test]
        public void Throw_Exception_If_Unsubscribed_Type_Is_Not_Derived_From_IEventBase()
        {
            // Arrange
            var eventBus = new EventBus();
            // Act
            void Action() => eventBus.Unsubscribe(typeof(OnNotIEventBase), TestGeneralInvoke);
            //Assert
            Assert.Throws(typeof(ArgumentException), Action);
        }

        [Test]
        public void Not_Invoke_General_Action_Subscribed_In_Run_Time_After_Unsubscribe()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe(typeof(OnTest1), TestGeneralInvoke);
            // Act
            eventBus.Unsubscribe(typeof(OnTest1), TestGeneralInvoke);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.IsFalse(_isInvoke);
        }

        [Test]
        public void Only_Invoke_Not_Unsubscribe_Specific_Action()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestSpecificOrder1);
            eventBus.Subscribe<OnTest1>(TestSpecificOrder2);
            // Act
            eventBus.Unsubscribe<OnTest1>(TestSpecificOrder1);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.AreEqual("2", _order); 
        }
        
        [Test]
        public void Only_Invoke_Not_Unsubscribe_General_Action()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestGeneralOrder1);
            eventBus.Subscribe<OnTest1>(TestGeneralOrder2);
            // Act
            eventBus.Unsubscribe<OnTest1>(TestGeneralOrder1);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.AreEqual("2", _order);
        }

        [Test]
        public void Only_Invoke_Not_Unsubscribe_General_Action_Subscribed_In_Run_Time()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe(typeof(OnTest1), TestGeneralOrder1);
            eventBus.Subscribe(typeof(OnTest1), TestGeneralOrder2);
            // Act
            eventBus.Unsubscribe(typeof(OnTest1), TestGeneralOrder1);
            eventBus.Publish(new OnTest1());
            //Assert
            Assert.AreEqual("2", _order); 
        }

        [Test]
        public void Multiple_Subscribed_Action_Should_Invoke_Multiple_Times()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestGeneralOrder1);
            eventBus.Subscribe<OnTest2>(TestGeneralOrder1);
            // Act
            eventBus.Publish(new OnTest1());
            eventBus.Publish(new OnTest2());
            //Assert
            Assert.AreEqual("11", _order);
        }

        [Test]
        public void Multiple_Action_Subscribed_In_Run_Time_Should_Invoke_Multiple_Times()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe(typeof(OnTest1), TestGeneralOrder1);
            eventBus.Subscribe(typeof(OnTest2), TestGeneralOrder1);
            // Act
            eventBus.Publish(new OnTest1());
            eventBus.Publish(new OnTest2());
            //Assert
            Assert.AreEqual("11", _order);
        }
        
        [Test]
        public void Multiple_Subscribed_Action_Only_Invoke_Which_Not_Unsubscribe()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe<OnTest1>(TestGeneralOrder1);
            eventBus.Subscribe<OnTest2>(TestGeneralOrder1);
            // Act
            eventBus.Unsubscribe<OnTest1>(TestGeneralOrder1);
            eventBus.Publish(new OnTest1());
            eventBus.Publish(new OnTest2());
            //Assert
            Assert.AreEqual("1", _order);
        }

        [Test]
        public void Multiple_Action_Subscribed_In_Run_Time_Only_Invoke_Which_Not_Unsubscribe()
        {
            // Arrange
            var eventBus = new EventBus();
            eventBus.Subscribe(typeof(OnTest1), TestGeneralOrder1);
            eventBus.Subscribe(typeof(OnTest2), TestGeneralOrder1);
            // Act
            eventBus.Unsubscribe(typeof(OnTest1), TestGeneralOrder1);
            eventBus.Publish(new OnTest1());
            eventBus.Publish(new OnTest2());
            //Assert
            Assert.AreEqual("1", _order);
        }
        
        private void TestSpecificOrder1(OnTest1 obj)
        {
            _order += "1";
        }
        
        private void TestSpecificOrder2(OnTest1 obj)
        {
            _order += "2";
        }
        
        private void TestGeneralOrder1(IEventBase obj)
        {
            _order += "1";
        }
        
        private void TestGeneralOrder2(IEventBase obj)
        {
            _order += "2";
        }

        private void TestGeneralInvoke(IEventBase obj)
        {
            _isInvoke = true;
        }

        private void TestSpecificInvoke(OnTest1 obj)
        {
            _isInvoke = true;
        }
    }
}

internal class OnTest1 : IEventBase
{
}

internal class OnTest2 : IEventBase
{
}

internal class OnNotIEventBase
{
}