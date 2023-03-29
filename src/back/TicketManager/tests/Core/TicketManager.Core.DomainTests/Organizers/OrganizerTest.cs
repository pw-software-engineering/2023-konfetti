using FluentAssertions;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using Xunit;

namespace TicketManager.Core.DomainTests.Organizers;

public class OrganizerTest
{
    public class ConstructorTests
    {
        [Theory]
        [InlineData("email@email.com", "company 1", "address 1", "taxId-1", TaxIdType.Nip, "Display Name 1", "123-456-789")]
        [InlineData("email.address.email@email.com", "CoMpAnY 2", "Address 2", "TaxId-2", TaxIdType.Regon, "Display Name 2", "+48123456789")]
        [InlineData("jan.kowalski@net.edu.com", "company, 3*=", "address 3=!", "TAxId-3", TaxIdType.Krs, "Display Name 3", "000000000")]
        [InlineData("alias@loop.com", "company 4.s.4", "address 4@", "TAXId-4", TaxIdType.Pesel, "Display Name 4", "426129823")]
        [InlineData("company@company.com", "comp5", "address, 00-000 5", "TAXID-5", TaxIdType.Vatin, "Display Name 5", "+42-345-123-457")]
        public void WhenValidParametersProvided_ItShouldSetThemAsProperties(
            string email,
            string companyName,
            string address,
            string taxId,
            TaxIdType taxIdType,
            string displayName,
            string phoneNumber)
        {
            var organizer = new Organizer(email, companyName, address, taxId, taxIdType, displayName, phoneNumber);
            organizer.Email.Should().Be(email);
            organizer.CompanyName.Should().Be(companyName);
            organizer.Address.Should().Be(address);
            organizer.TaxId.Should().Be(taxId);
            organizer.TaxIdType.Should().Be(taxIdType);
            organizer.DisplayName.Should().Be(displayName);
            organizer.PhoneNumber.Should().Be(phoneNumber);
        }

        [Fact]
        public void WhenConstructorCalled_ItShouldReturnOrganizerWithUnverifiedStatus()
        {
            var organizer = new Organizer("email@email.com", "companyName", "address",
                "taxId", TaxIdType.Nip, "displayName","phoneNumber");
            organizer.VerificationStatus.Should().Be(VerificationStatus.Unverified);
        }
        
        [Fact]
        public void WhenConstructorCalledTwice_ItShouldReturnTwoDifferentIds()
        {
            
            var organizer1 = new Organizer("email@email.com", "companyName", "address",
                "taxId", TaxIdType.Nip, "displayName","phoneNumber");
            
            var organizer2 = new Organizer("email@email.com", "companyName", "address",
                "taxId", TaxIdType.Nip, "displayName","phoneNumber");

            organizer1.Id.Should().NotBe(organizer2.Id);
        }
    }

    public class GetAccountTests
    {
        [Fact]
        public void WhenPasswordHashIsProvided_ItShouldGenerateAccountWithTheSameData()
        {
            var organizer = new Organizer("email@email.com", "companyName", "address",
                "taxId", TaxIdType.Nip, "displayName","phoneNumber");

            var passwordHash = "passwordhash";

            var account = organizer.GetAccount(passwordHash);

            account.Id.Should().Be(organizer.Id);
            account.Email.Should().Be(organizer.Email);
            account.PasswordHash.Should().Be(passwordHash);
            account.Role.Should().Be(AccountRoles.Organizer);
        }
    }

    public class DecideTests
    {

        private Organizer unverifiedOrganizer;
        public DecideTests()
        {
            unverifiedOrganizer = new Organizer("email@email.com", "companyName", "address",
                "taxId", TaxIdType.Nip, "displayName","phoneNumber");
        }
        
        [Fact]
        public void WhenVerifyingPositevely_ItShouldHavePositiveVerificationStatus()
        {
            unverifiedOrganizer.Decide(true);
            unverifiedOrganizer.VerificationStatus.Should().Be(VerificationStatus.VerifiedPositively);
        }
        
        [Fact]
        public void WhenVerifyingNegatively_ItShouldHaveNegativeVerificationStatus()
        {
            unverifiedOrganizer.Decide(false);
            unverifiedOrganizer.VerificationStatus.Should().Be(VerificationStatus.VerifiedNegatively);
        }
        
        
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void WhenVerifyingForSecondTime_ItShouldNotChangeOrganizerStatus(bool decision1, bool decision2)
        {
            unverifiedOrganizer.Decide(decision1);
            var status = unverifiedOrganizer.VerificationStatus;
            unverifiedOrganizer.Decide(decision2);
            unverifiedOrganizer.VerificationStatus.Should().Be(status);
        }
    }
}
