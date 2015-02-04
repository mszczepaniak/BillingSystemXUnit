using System;
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

            var thing = new BillingDoohickey(repo.Object, charger.Object);

            thing.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Never());
            
        }

        [Fact]
        public void CustomerWithSubscriptionThatIsExpiredGetsCharged()
        {
            
        }
        // Monthly billing
        // Grace period for missed payments ("dunning" status)
        // Not all customers are necessarily subscribers
        // Idle customers should be automatically unsubscribed
    }

    public interface ICustomerRepository
    {
        
    }

    public interface ICreditCardCharger
    {
        void ChargeCustomer(Customer customer);
    }

    public class Customer
    {
        
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
        }
    }
}