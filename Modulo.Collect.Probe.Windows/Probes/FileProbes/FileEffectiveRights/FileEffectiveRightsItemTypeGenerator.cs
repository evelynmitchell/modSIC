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

using System.Collections.Generic;
using System.Linq;
using Modulo.Collect.OVAL.Definitions;
using Modulo.Collect.OVAL.Definitions.variableEvaluator;
using Modulo.Collect.OVAL.SystemCharacteristics;
using Modulo.Collect.Probe.Common;
using Modulo.Collect.Probe.Independent.Common;
using Modulo.Collect.Probe.Windows.FileEffectiveRights;
using Definitions = Modulo.Collect.OVAL.Definitions;
using Modulo.Collect.OVAL.Definitions.Windows;
using Modulo.Collect.Probe.Independent.Common.File;

namespace Modulo.Collect.Probe.Windows.File
{
    public class FileEffectiveRightsItemTypeGenerator : IItemTypeGenerator
    {
        public BaseObjectCollector SystemDataSource { get; set; }

        public BaseObjectCollector FileDataSource { get; set; }

        public IFileProvider FileProvider { get; set; }

        /// <summary>
        /// Creates a items to collect from given object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>A list of ItemType</returns>
        public virtual IEnumerable<ItemType> GetItemsToCollect(Definitions.ObjectType objectType, VariablesEvaluated variables)
        {
            var variableEvaluator = new FileEffectiveRightsVariableEvaluator(variables);
            var operationEvaluator = new FileEffectiveRightsOperationEvaluator(SystemDataSource, FileProvider);
            var fileEffectiveRights = (fileeffectiverights_object)objectType;

            if (fileEffectiveRights.HasVariableDefined())
            {
                var objects = variableEvaluator.ProcessVariables(objectType);
                return operationEvaluator.ProcessOperation(objects).ToList();
            }

            return operationEvaluator.ProcessOperation(fileEffectiveRights).ToList();
        }
    }
}
