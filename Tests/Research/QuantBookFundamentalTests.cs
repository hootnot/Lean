﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using NUnit.Framework;
using Python.Runtime;
using System;
using System.Collections.Generic;
using QuantConnect.Research;

namespace QuantConnect.Tests.Research
{

    [TestFixture]
    public class QuantBookFundamentalTests
    {
        private dynamic _module;
        private DateTime _startDate;
        private DateTime _endDate;

        [SetUp]
        public void Setup()
        {
            _startDate = new DateTime(2014, 1, 1);
            _endDate = new DateTime(2014, 12, 31);

            using (Py.GIL())
            {
                _module = Py.Import("Test_QuantBookFundamental");
            }
        }

        //All the accepted types of input for fundamental data request
        private static readonly object[] _fundamentalTestCases =
        {
            new object[] {new List<string> {"AAPL", "GOOG", "SPY"}},
            new object[] {"AAPL"},
            new object[] {Symbol.Create("AAPL", SecurityType.Equity, Market.USA)},
            new object[] {new List<Symbol> { Symbol.Create("AAPL", SecurityType.Equity, Market.USA),
                Symbol.Create("GOOG", SecurityType.Equity, Market.USA),
                Symbol.Create("SPY", SecurityType.Equity, Market.USA)}},
        };

        [TestCaseSource(nameof(_fundamentalTestCases))]
        public void PyFundamentalQuantBookHistory(dynamic input)
        {
            using (Py.GIL())
            {
                var PyFundamentalData = _module.FundamentalHistoryTest(input, _startDate, _endDate);
                var data = PyFundamentalData.getData();
            }
        }

        [TestCaseSource(nameof(_fundamentalTestCases))]
        public void CSharpFundamentalQuantBookHistory(dynamic input)
        {
            var qb = new QuantBook();
            qb.AddEquity("AAPL");
            qb.AddEquity("GOOG");
            qb.AddEquity("SPY");
            var data = qb.GetFundamental(input, "ValuationRatios.PERatio", _startDate, _endDate);

        }
    }
}
