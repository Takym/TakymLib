﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading;
using TakymLib.Threading.Distributed.Internals;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  実行文脈情報を管理します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class ExecutionContextManager : DisposableBase
	{
		private readonly static object                   _manager_lock = new();
		private          static ExecutionContextManager? _manager;

		/// <summary>
		///  既定の<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトを取得します。
		/// </summary>
		/// <remarks>
		///  このインスタンスを破棄(<see cref="System.IDisposable.Dispose"/>の呼び出し)しないでください。
		///  アプリケーション終了時のみ破棄できます。
		/// </remarks>
		/// <returns>有効な<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトです。</returns>
		public static ExecutionContextManager GetDefault()
		{
			lock (_manager_lock) {
				if (_manager is null || _manager.IsDisposing || _manager.IsDisposed) {
					_manager = Create();
				}
				return _manager;
			}
		}

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <returns>有効な<see cref="TakymLib.Threading.Distributed.ExecutionContextManager"/>オブジェクトです。</returns>
		public static ExecutionContextManager Create()
		{
			return new ExecutionContextManagerInternal();
		}

		/// <summary>
		///  現在のスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		public ExecutionContext Client
		{
			get
			{
				this.EnsureNotDisposed();
				return this.GetClientContextCore();
			}
		}

		/// <summary>
		///  上書きされた場合、現在のスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		protected abstract ExecutionContext GetClientContextCore();

		/// <summary>
		///  指定されたスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <param name="thread">スレッドです。</param>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public ExecutionContext GetServerContext(Thread thread)
		{
			thread.EnsureNotNull(nameof(thread));
			this.EnsureNotDisposed();
			return this.GetServerContextCore(thread);
		}

		/// <summary>
		///  上書きされた場合、指定されたスレッドの実行文脈情報を取得します。
		/// </summary>
		/// <param name="thread">スレッドです。</param>
		/// <returns><see cref="TakymLib.Threading.Distributed.ExecutionContext"/>オブジェクトです。</returns>
		protected abstract ExecutionContext GetServerContextCore(Thread thread);

		/// <summary>
		///  指定されたスレッドへ接続します。
		/// </summary>
		/// <param name="thread">接続先のスレッドです。</param>
		/// <returns>接続情報を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ObjectDisposedException"/>
		public ConnectedContext Connect(Thread thread)
		{
			thread.EnsureNotNull(nameof(thread));
			this.EnsureNotDisposed();
			return new ConnectedContext(
				this.GetServerContextCore(thread),
				this.GetClientContextCore(),
				true
			);
		}
	}
}
