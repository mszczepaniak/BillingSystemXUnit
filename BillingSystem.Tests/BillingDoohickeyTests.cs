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
            var customer = new Customer();

            BillingDoohickey thing = new BillingDoohickey(repo, charger);

            thing.ProcessMonth(2011, 8);

            charger.Verify(c => c.ChargeCustomer(customer), Times.Never);
            //
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
    }
}