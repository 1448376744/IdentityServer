﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public static class IdentityServerConstants
    {
        public static IEnumerable<string> SupportedSigningAlgorithms = new List<string>
        {
            SecurityAlgorithms.RsaSha256,
            SecurityAlgorithms.RsaSha384,
            SecurityAlgorithms.RsaSha512,

            SecurityAlgorithms.RsaSsaPssSha256,
            SecurityAlgorithms.RsaSsaPssSha384,
            SecurityAlgorithms.RsaSsaPssSha512,

            SecurityAlgorithms.EcdsaSha256,
            SecurityAlgorithms.EcdsaSha384,
            SecurityAlgorithms.EcdsaSha512
        };
      
        public enum RsaSigningAlgorithm
        {
            RS256,
            RS384,
            RS512,

            PS256,
            PS384,
            PS512
        }

        public enum ECDsaSigningAlgorithm
        {
            ES256,
            ES384,
            ES512
        }
    }
}