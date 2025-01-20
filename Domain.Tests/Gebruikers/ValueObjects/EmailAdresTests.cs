using System;
using Domain.Common.Exceptions;
using Domain.Gebruikers.ValueObjects;
using Xunit;

namespace Domain.Tests.Gebruikers.ValueObjects
{
    public class EmailAdresTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.com")]
        [InlineData("user+tag@domain.com")]
        [InlineData("email@subdomain.domain.com")]
        [InlineData("123@domain.com")]
        [InlineData("email@domain.co.uk")]
        public void Moet_GeldigeEmail_Accepteren(string emailAdres)
        {
            // Act
            var email = new EmailAdres(emailAdres);

            // Assert
            Assert.Equal(emailAdres.ToLower(), email.ToString());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("invalid@")]
        [InlineData("@domain.com")]
        [InlineData("email@domain")]
        [InlineData("email@.com")]
        [InlineData("email@domain..com")]
        public void Moet_OngeldigeEmail_Weigeren(string ongeldigeEmail)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainValidationException>(() => new EmailAdres(ongeldigeEmail));
            Assert.True(ex.HeeftFoutVoor("Email"));
            Assert.Contains("Ongeldig email formaat", ex.HaalFoutenOp("Email"));
        }

        [Fact]
        public void Moet_EmailsGelijkzijnOngeachtHoofdletters()
        {
            // Arrange
            var email1 = new EmailAdres("Test@Example.com");
            var email2 = new EmailAdres("test@example.com");

            // Act & Assert
            Assert.Equal(email1, email2);
            Assert.True(email1.Equals(email2));
        }

        [Fact]
        public void Moet_ConsistentHashCode_GenererenVoorZelfdeEmail()
        {
            // Arrange
            var email1 = new EmailAdres("Test@Example.com");
            var email2 = new EmailAdres("test@example.com");

            // Act & Assert
            Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
        }

        [Fact]
        public void Moet_ImplicieteConversie_NaarString_Werken()
        {
            // Arrange
            var emailAdres = "test@example.com";
            var email = new EmailAdres(emailAdres);

            // Act
            string resultaat = email;

            // Assert
            Assert.Equal(emailAdres, resultaat);
        }

        [Fact]
        public void Moet_False_Teruggeven_BijVergelijkingMetNull()
        {
            // Arrange
            var email = new EmailAdres("test@example.com");

            // Act & Assert
            Assert.False(email.Equals(null));
        }

        [Fact]
        public void Moet_False_Teruggeven_BijVergelijkingMetAnderType()
        {
            // Arrange
            var email = new EmailAdres("test@example.com");
            var anderObject = "test@example.com";

            // Act & Assert
            Assert.False(email.Equals(anderObject));
        }
    }
}