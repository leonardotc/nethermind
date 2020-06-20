//  Copyright (c) 2018 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.

using System;
using Nethermind.Core;
using Nethermind.Core.Specs;

namespace Nethermind.Evm.Precompiles.Mcl.Bls
{
    /// <summary>
    /// https://eips.ethereum.org/EIPS/eip-2537
    /// </summary>
    public class G2MultiExpPrecompile : IPrecompile
    {
        public static IPrecompile Instance = new G2MultiExpPrecompile();

        private G2MultiExpPrecompile()
        {
        }

        public Address Address { get; } = Address.FromNumber(15);

        public long BaseGasCost(IReleaseSpec releaseSpec)
        {
            return 0L;
        }

        public long DataGasCost(byte[] inputData, IReleaseSpec releaseSpec)
        {
            int k = inputData.Length / 290;
            return 55000L * k * Discount.For[k] / 1000;;
        }

        public (byte[], bool) Run(byte[] inputData)
        {  
            throw new NotImplementedException();
        }
    }
}