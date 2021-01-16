﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.IO
{
	/// <summary>
	///  パス文字列をキャッシュします。
	/// </summary>
	public static class PathStringPool
	{
		private static readonly Dictionary<string, PathString> _cache;

		static PathStringPool()
		{
			_cache = new();
		}

		/// <summary>
		///  現在の作業ディレクトリを指し示すパス文字列を取得します。
		/// </summary>
		/// <returns>キャッシュされたパス文字列です。</returns>
		public static PathString Get()
		{
			return Get(Environment.CurrentDirectory);
		}

		/// <summary>
		///  指定されたパスを指し示すパス文字列を取得します。
		/// </summary>
		/// <param name="path">文字列型のパス文字列です。</param>
		/// <returns>キャッシュされたパス文字列です。</returns>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="TakymLib.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public static PathString Get(string path)
		{
			path.EnsureNotNull(nameof(path));
			if (!_cache.TryGetValue(path, out var result)) {
				result = new PathString(path);
				_cache.Add(path, result);
			}
			return result;
		}

		/// <summary>
		///  キャッシュされた全てのパス文字列を削除します。
		/// </summary>
		public static void Clear()
		{
			_cache.Clear();

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}
}