﻿/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;

namespace Exrecodel.Extensions
{
	/// <summary>
	///  型'<see cref="Exrecodel.XrcdlDocument"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class XrcdlDocumentExtensions
	{
		/// <summary>
		///  指定された文字列をXML文書として読み込みます。
		/// </summary>
		/// <param name="document">読み込み先の<see cref="Exrecodel"/>文書オブジェクトです。</param>
		/// <param name="xml">読み込み元のXML文書を表す文字列です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Xml.XmlException"/>
		public static void FromXml(this XrcdlDocument document, string? xml)
		{
			if (document == null) {
				throw new ArgumentNullException(nameof(document));
			}
			xml ??= string.Empty;
			using (var sr = new StringReader(xml)) {
				document.Load(sr);
			}
		}

		/// <summary>
		///  指定された<see cref="Exrecodel"/>文書をXML文字列へ変換します。
		/// </summary>
		/// <param name="document">変換前の<see cref="Exrecodel"/>文書オブジェクトです。</param>
		/// <returns>変換結果の<see cref="Exrecodel"/>文書を表すXML文字列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.Xml.XmlException"/>
		public static string ToXml(this XrcdlDocument document)
		{
			if (document == null) {
				throw new ArgumentNullException(nameof(document));
			}
			using (var sw = new StringWriter()) {
				document.Save(sw);
				return sw.ToString();
			}
		}
	}
}
