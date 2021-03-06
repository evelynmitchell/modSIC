﻿/*
 * Modulo Open Distributed SCAP Infrastructure Collector (modSIC)
 * 
 * Copyright (c) 2011-2014, Modulo Solutions for GRC.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 *   
 * - Redistributions in binary form must reproduce the above copyright 
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *   
 * - Neither the name of Modulo Security, LLC nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific  prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 * */
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulo.Collect.OVAL.Definitions;
using Modulo.Collect.OVAL.Tests.helpers;
using Modulo.Collect.OVAL.Definitions.VariableEvaluators.Evaluators.LocalVariableComponents;
using Modulo.Collect.OVAL.SystemCharacteristics;
using Modulo.Collect.OVAL.Definitions.variableEvaluator.exceptions;

namespace Modulo.Collect.OVAL.Tests.variables.localVariableComponents
{
    /// <summary>
    /// Summary description for LocalVariableVariablesComponentTest
    /// </summary>
    [TestClass]
    public class LocalVariableVariablesComponentTest
    {
        public LocalVariableVariablesComponentTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_get_value_to_variableComponent_by_the_constantVariable()
        {
            oval_definitions definitions = new OvalDocumentLoader().GetFakeOvalDefinitions("definitionsWithLocalVariable.xml");
            Assert.IsNotNull(definitions);
            VariableComponentType variableComponent = new VariableComponentType() { var_ref = "oval:org.mitre.oval:var:3000" };          
            VariablesTypeVariableLocal_variable localVariable = new VariablesTypeVariableLocal_variable() { id = "oval:org.mitre.oval:var:5000" };

            LocalVariableVariablesComponent variableComponentEvaluator = new LocalVariableVariablesComponent(localVariable,variableComponent,definitions.variables,null);
            IEnumerable<string> values = variableComponentEvaluator.GetValue();
            Assert.IsTrue(values.Count() > 0, "the quantity of values is not expected.");
            Assert.IsTrue(values.ElementAt(0) == "Multiprocessor Free", "the value is not expected");

        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_get_value_to_variableComponent_by_the_LocalVariable()
        {
            OvalDocumentLoader ovalDocuments = new OvalDocumentLoader();

            oval_definitions definitions = ovalDocuments.GetFakeOvalDefinitions("definitionsWithLocalVariable.xml");
            Assert.IsNotNull(definitions);
            oval_system_characteristics systemSystemCharacteristics = ovalDocuments.GetFakeOvalSystemCharacteristics("system_characteristics_with_local_variable.xml");
            Assert.IsNotNull(systemSystemCharacteristics);
            VariablesTypeVariableLocal_variable localVariable = new VariablesTypeVariableLocal_variable() { id = "oval:org.mitre.oval:var:5000" };

            VariableComponentType variableComponent = new VariableComponentType() { var_ref = "oval:org.mitre.oval:var:4000" };
            LocalVariableVariablesComponent variableComponentEvaluator = new LocalVariableVariablesComponent(localVariable,variableComponent, definitions.variables, systemSystemCharacteristics);
            IEnumerable<string> values = variableComponentEvaluator.GetValue();
            Assert.IsTrue(values.Count() > 0, "the quantity of value is not expected");
            Assert.IsTrue(values.ElementAt(0) == @"Software\Microsoft\Windows NT\CurrentVersion", "the value is not expected");

        }

        [TestMethod, Owner("lcosta")]
        [ExpectedException(typeof(VariableCircularReferenceException))]
        public void Should_not_be_possible_to_get_value_to_variableComponent_if_this_variable_refer_his_localVariable()
        {
            OvalDocumentLoader ovalDocuments = new OvalDocumentLoader();

            oval_definitions definitions = ovalDocuments.GetFakeOvalDefinitions("definitionsWithLocalVariable.xml");
            Assert.IsNotNull(definitions);
            oval_system_characteristics systemSystemCharacteristics = ovalDocuments.GetFakeOvalSystemCharacteristics("system_characteristics_with_local_variable.xml");
            Assert.IsNotNull(systemSystemCharacteristics);

            VariablesTypeVariableLocal_variable localVariable = new VariablesTypeVariableLocal_variable() { id = "oval:org.mitre.oval:var:4000" };

            VariableComponentType variableComponent = new VariableComponentType() { var_ref = "oval:org.mitre.oval:var:4000" };
            LocalVariableVariablesComponent variableComponentEvaluator = new LocalVariableVariablesComponent(localVariable,variableComponent, definitions.variables, systemSystemCharacteristics);
            IEnumerable<string> values = variableComponentEvaluator.GetValue();
            Assert.IsTrue(values.Count() == 0);
        }        
    }
}

