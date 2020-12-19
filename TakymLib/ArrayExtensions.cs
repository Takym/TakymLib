﻿/****
 * TakymLib
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TakymLib
{
	/// <summary>
	///  配列の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		///  指定された配列を結合し新たな配列を作成します。
		///  この操作は時間がかかる可能性があります。
		/// </summary>
		/// <typeparam name="T">配列の要素型です。</typeparam>
		/// <param name="baseArray">基本となる配列です。</param>
		/// <param name="arrays">結合する配列です。</param>
		/// <returns>結合された新しい配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static T[] Join<T>(this T[] baseArray, params T[][] arrays)
		{
			baseArray.EnsureNotNull(nameof(baseArray));
			arrays   .EnsureNotNull(nameof(arrays));
			return baseArray.JoinCore(arrays);
		}

		/// <summary>
		///  指定されたオブジェクト配列を結合し新たなオブジェクト配列を作成します。
		///  この操作は時間がかかる可能性があります。
		/// </summary>
		/// <param name="baseArray">基本となるオブジェクト配列です。</param>
		/// <param name="arrays">結合するオブジェクト配列です。</param>
		/// <returns>結合された新しいオブジェクト配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static object?[] Join(this object?[] baseArray, params object?[][] arrays)
		{
			baseArray.EnsureNotNull(nameof(baseArray));
			arrays   .EnsureNotNull(nameof(arrays));
			return baseArray.JoinCore(arrays);
		}

		/// <summary>
		///  指定された配列を結合し新たな配列を作成します。
		/// </summary>
		/// <typeparam name="T">配列の要素型です。</typeparam>
		/// <param name="baseArray">基本となる配列です。</param>
		/// <param name="arrays">結合する配列です。</param>
		/// <returns>結合された新しい配列を含む非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task<T[]> JoinAsync<T>(this T[] baseArray, params T[][] arrays)
		{
			baseArray.EnsureNotNull(nameof(baseArray));
			arrays   .EnsureNotNull(nameof(arrays));
			return await Task.Run(() => baseArray.JoinCore(arrays)).ConfigureAwait(false);
		}

		/// <summary>
		///  指定されたオブジェクト配列を結合し新たなオブジェクト配列を作成します。
		/// </summary>
		/// <param name="baseArray">基本となるオブジェクト配列です。</param>
		/// <param name="arrays">結合するオブジェクト配列です。</param>
		/// <returns>結合された新しいオブジェクト配列を含む非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task<object?[]> JoinAsync(this object?[] baseArray, params object?[][] arrays)
		{
			baseArray.EnsureNotNull(nameof(baseArray));
			arrays   .EnsureNotNull(nameof(arrays));
			return await Task.Run(() => baseArray.JoinCore(arrays)).ConfigureAwait(false);
		}

		private static T[] JoinCore<T>(this T[] baseArray, T[][] arrays)
		{
			var result = new List<T>(baseArray);
			for (int i = 0; i < arrays.Length; ++i) {
				if (arrays[i] is not null) {
					result.AddRange(arrays[i]);
				}
			}
			return result.ToArray();
		}

		private static object?[] JoinCore(this object?[] baseArray, object?[][] arrays)
		{
			var result = new ArrayList(baseArray);
			for (int i = 0; i < arrays.Length; ++i) {
				if (arrays[i] is not null) {
					result.AddRange(arrays[i]);
				}
			}
			return result.ToArray();
		}
	}
}
