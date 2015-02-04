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
            // Source of customers
            // Service for charging customers
            var repo = new Mock<ICustomerRepository>();
            var charger = new Mock<ICreditCardCharger>();
            var customer = new Customer(); // what does it mean to not have a subscription
            repo.Setup(r => r.Customers)
                .Returns(new Customer[] {customer});
            var thing = new BillingDoohickey(repo.Object, charger.Object);

            thing.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Never());
            
        }

        [Fact]
        public void CustomerWithSubscriptionThatIsExpiredGetsCharged()
        {
            var repo = new Mock<ICustomerRepository>();
            var charger = new Mock<ICreditCardCharger>();
            var customer = new Customer { Subscribed = true}; // what does it mean to not have a subscription
            repo.Setup(r => r.Customers)
                .Returns(new Customer[] {customer});
            var thing = new BillingDoohickey(repo.Object, charger.Object);
              
            thing.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Once());
        }
        // Monthly billing
        // Grace period for missed payments ("dunning" status)
        // Not all customers are necessarily subscribers
        // Idle customers should be automatically unsubscribed
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

    public class BillingDoohickey
    {
        ICreditCardCharger charger;
        ICustomerRepository repo;

        public BillingDoohickey(ICustomerRepository repo, ICreditCardCharger charger)
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
}