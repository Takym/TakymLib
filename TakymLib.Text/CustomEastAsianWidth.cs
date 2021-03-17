﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TakymLib.Text
{
	/// <summary>
	///  ユーザー定義の東アジアの文字幅を扱います。
	/// </summary>
	public class CustomEastAsianWidth : EastAsianWidth
	{
		// TODO: C#コード生成処理

		/// <summary>
		///  範囲で表された東アジアの文字幅の列挙値の読み取り専用リストを取得します。
		/// </summary>
		protected IReadOnlyList<(char Start, char End, EastAsianWidthType Type)> Ranges { get; }

		/// <summary>
		///  型'<see cref="TakymLib.Text.CustomEastAsianWidth"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="ranges">範囲で表された東アジアの文字幅の列挙値の配列です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public CustomEastAsianWidth(params (char Start, char End, EastAsianWidthType Type)[] ranges)
		{
			ranges.EnsureNotNull(nameof(ranges));
			this.Ranges = new ReadOnlyCollection<(char Start, char End, EastAsianWidthType Type)>(ranges);
		}

		/// <summary>
		///  型'<see cref="TakymLib.Text.CustomEastAsianWidth"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="ranges">範囲で表された東アジアの文字幅の列挙値の読み取り専用リストです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public CustomEastAsianWidth(IReadOnlyList<(char Start, char End, EastAsianWidthType Type)> ranges)
		{
			ranges.EnsureNotNull(nameof(ranges));
			this.Ranges = ranges;
		}

#pragma warning disable TakymLib_EAWInvalid // 型またはメンバーが旧型式です
		/// <summary>
		///  指定された文字の文字幅を判定します。
		/// </summary>
		/// <param name="c">判定する文字です。</param>
		/// <returns><see cref="TakymLib.Text.EastAsianWidthType"/>列挙値を返します。</returns>
		public override EastAsianWidthType GetCharType(char c)
		{
			int count = this.Ranges.Count;
			for (int i = 0; i < count; ++i) {
				var range = this.Ranges[i];
				if (c <= range.Start && range.End <= c) {
					return range.Type;
				}
			}
			return EastAsianWidthType.Invalid;
		}
#pragma warning restore TakymLib_EAWInvalid // 型またはメンバーが旧型式です
	}
}
