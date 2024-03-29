﻿using laget.Fingerprint.Extensions;
using laget.Fingerprint.Tests.Models;
using System;
using System.Security.Cryptography;
using Xunit;

namespace laget.Fingerprint.Tests.Types
{
    public class FloatTests
    {
        private readonly Func<ThisUser, byte[]> _sha1;
        private readonly ThisUser _user;

        public FloatTests()
        {
            _sha1 = FingerprintBuilder<ThisUser>
                .Create(SHA1.Create().ComputeHash)
                .For(p => p.Firstname)
                .For(p => p.Number)
                .Build();

            _user = new ThisUser
            {
                Firstname = "John",
                Number = 2.1f
            };
        }

        [Fact]
        public void Sha1()
        {
            var hash = _sha1(_user).ToLowerHexString();

            Assert.Equal("487b230857d43f392b9d20189d972d0cc8aa7c98", hash);
        }

        [Fact]
        public void UserInfo_Sha1_UpdateBool_ChangeHash()
        {
            var hash0 = _sha1(_user).ToLowerHexString();
            _user.Number = 2.11f;
            var hash1 = _sha1(_user).ToLowerHexString();

            Assert.NotEqual(hash0, hash1);
        }

        private class ThisUser : User
        {
            public float Number { get; set; }
        }
    }
}
