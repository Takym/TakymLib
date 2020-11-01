﻿/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Xml;

namespace Exrecodel.InternalImplementations
{
	internal sealed class XrcdlRootImplementation : XrcdlRoot
	{
		private readonly XrcdlDocumentImplementation   _doc;
		private readonly XmlElement                    _elem;
		private readonly List<XrcdlNode>               _list;
		private readonly List<XmlElement>              _elem_cache;

		public override string Name
		{
			get => this.GetElement(Constants.Name).Value ?? string.Empty;
			set => this.GetElement(Constants.Name).Value = value;
		}

		public override IReadOnlyList<XrcdlNode> Children { get; }

		public XrcdlRootImplementation(XrcdlDocumentImplementation document) : base(document)
		{
			_doc          = document;
			_elem         = _doc.GetElement(Constants.Root);
			_list         = new List<XrcdlNode>();
			_elem_cache   = new List<XmlElement>();
			this.Children = _list.AsReadOnly();
		}

		private XmlElement GetElement(params string[] names)
		{
			return Utils.GetElement(_doc.CreateElement, _elem, names);
		}
	}
}
