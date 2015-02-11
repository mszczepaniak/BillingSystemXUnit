using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Moq;
using Xunit;

namespace BillingSystem.Tests
{
    public class BillingDoohickeyTests
    {
        [Fact]
        public void CustomerWhoDoesNotHaveSubscriptionDoesNotGetCharged()
        {
            var customer = new Customer(); // what does it mean to not have a subscription
            var processor = CreateBillingProcessor(customer);

            processor.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Never());

            // Source of customers
            // Service for charging customers
        }

        

        [Fact]
        public void CustomerWithSubscriptionThatIsExpiredGetsCharged()
        {

            var repo = new Mock<ICustomerRepository>();
            var charger = new Mock<ICreditCardCharger>();
            var customer = new Customer { Subscribed = true };
            repo.Setup(r => r.Customers)
                .Returns(new Customer[] {customer});
            var processor = new BillingProcessor(repo.Object, charger.Object);
              
            processor.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Once());
        }

        [Fact]
        public void CustomerWithSubscriptionThatIsCurrentDoesNotGetCharged()
        { 
            
        }
        // Monthly billing
        // Grace period for missed payments ("dunning" status)
        // Not all customers are necessarily subscribers
        // Idle customers should be automatically unsubscribed
        private TestableBillingProcessor CreateBillingProcessor(Customer customer)
        {

            var repo = new Mock<ICustomerRepository>();
            var charger   = new Mock<ICreditCardCharger>();
            repo.Setup(r => r.Customers)
                .Returns(new Customer[] { customer });
            var processor = new BillingProcessor(repo.Object, charger.Object);

            return processor;
        }
    }

    public interface ICustomerRepository
    {
        IEnumerable<Customer> Customers { get; }
    }

    public interface ICreditCardCharger
    {
        void ChargeCustomer(Customer customer);
    }

    public class Customer
    {
        public bool Subscribed { get; set; }
    }

    public class BillingProcessor
    {
        ICreditCardCharger charger;
        ICustomerRepository repo;

        public BillingProcessor(ICustomerRepository repo, ICreditCardCharger charger)
        {
            this.repo = repo;
            this.charger = charger;
        }

        public void ProcessMonth(int year, int month)
        {
            var customer = repo.Customers.Single();

            if (customer.Subscribed)
            {
                charger.ChargeCustomer(customer);
            }
        }
    }

    public class TestableBillingProcessor : BillingProcessor
    {
        public Mock<ICreditCardCharger> Charger;
        public Mock<ICustomerRepository> Repository;
        public TestableBillingProcessor()
        {
            Charger = new Mock<ICreditCardCharger>();
            Repository = new Mock<ICustomerRepository>();
        }
    }
}