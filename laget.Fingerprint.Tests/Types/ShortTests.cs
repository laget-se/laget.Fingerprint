﻿using laget.Fingerprint.Extensions;
using laget.Fingerprint.Tests.Models;
using System;
using System.Security.Cryptography;
using Xunit;

namespace laget.Fingerprint.Tests.Types
{
    public class ShortTests
    {
        private readonly Func<ThisUser, byte[]> _sha1;
        private readonly ThisUser _user;

        public ShortTests()
        {
            _sha1 = FingerprintBuilder<ThisUser>
                .Create(SHA1.Create().ComputeHash)
                .For(p => p.Firstname)
                .For(p => p.ShortNumber)
                .For(p => p.UShortNumber)
                .Build();

            _user = new ThisUser
            {
                Firstname = "John",
                ShortNumber = -2,
                UShortNumber = 2,
            };
        }

        [Fact]
        public void Sha1()
        {
            var hash = _sha1(_user).ToLowerHexString();
            Assert.Equal("3c6cd62c0c7ca868fabfd931a9ca4bdb39cc804f", hash);
        }

        [Fact]
        public void UserInfo_Sha1_UpdateShort_ChangeHash()
        {
            var hash0 = _sha1(_user).ToLowerHexString();
            _user.ShortNumber = 2;
            var hash1 = _sha1(_user).ToLowerHexString();

            Assert.NotEqual(hash0, hash1);
        }

        [Fact]
        public void UserInfo_Sha1_UpdateUShort_ChangeHash()
        {
            var hash0 = _sha1(_user).ToLowerHexString();
            _user.UShortNumber = 1;
            var hash1 = _sha1(_user).ToLowerHexString();

            Assert.NotEqual(hash0, hash1);
        }

        private class ThisUser : User
        {
            public short ShortNumber { get; set; }
            public ushort UShortNumber { get; set; }
        }
    }
}
