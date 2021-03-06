﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Symbols;

namespace Microsoft.CodeAnalysis.CodeGen
{
    /// <summary>
    /// We need a CCI representation for local constants because they are emitted as locals in
    /// PDB scopes to improve the debugging experience (see LocalScopeProvider.GetConstantsInScope).
    /// </summary>
    internal sealed class LocalConstantDefinition : Cci.ILocalDefinition
    {
        private readonly string _name;
        private readonly Location _location;
        private readonly Cci.IMetadataConstant _compileTimeValue;
        private readonly bool _isDynamic;

        //Gives the synthesized dynamic attributes of the local definition
        private readonly ImmutableArray<TypedConstant> _dynamicTransformFlags;

        public LocalConstantDefinition(string name, Location location, Cci.IMetadataConstant compileTimeValue, bool isDynamic = false,
            ImmutableArray<TypedConstant> dynamicTransformFlags = default(ImmutableArray<TypedConstant>))
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(compileTimeValue != null);

            _name = name;
            _location = location;
            _compileTimeValue = compileTimeValue;
            _isDynamic = isDynamic;
            _dynamicTransformFlags = dynamicTransformFlags;
        }

        public string Name
        {
            get { return _name; }
        }

        public Location Location
        {
            get { return _location; }
        }

        public Cci.IMetadataConstant CompileTimeValue
        {
            get { return _compileTimeValue; }
        }

        public Cci.ITypeReference Type
        {
            get { return _compileTimeValue.Type; }
        }

        public bool IsConstant
        {
            get { return true; }
        }

        public ImmutableArray<Cci.ICustomModifier> CustomModifiers
        {
            get { return ImmutableArray<Cci.ICustomModifier>.Empty; }
        }

        public bool IsModified
        {
            get { return false; }
        }

        public bool IsPinned
        {
            get { return false; }
        }

        public bool IsReference
        {
            get { return false; }
        }

        public LocalSlotConstraints Constraints
        {
            get { return LocalSlotConstraints.None; }
        }

        public bool IsDynamic
        {
            get { return _isDynamic; }
        }

        public uint PdbAttributes
        {
            get { return Cci.PdbWriter.DefaultLocalAttributesValue; }
        }

        public ImmutableArray<TypedConstant> DynamicTransformFlags
        {
            get { return _dynamicTransformFlags; }
        }

        public int SlotIndex
        {
            get { return -1; }
        }

        public byte[] Signature
        {
            get { return null; }
        }

        public LocalSlotDebugInfo SlotInfo
        {
            get { return new LocalSlotDebugInfo(SynthesizedLocalKind.UserDefined, LocalDebugId.None); }
        }
    }
}
