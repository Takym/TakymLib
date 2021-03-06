﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Distributed.Internals
{
	internal sealed class ExecutionContextManagerInternal : ExecutionContextManager
	{
		private readonly ReaderWriterLockSlim              _rwlock;
		private readonly Dictionary<int, ExecutionContext> _dict;

		internal ExecutionContextManagerInternal()
		{
			_rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			_dict   = new Dictionary<int, ExecutionContext>();
		}

		protected override ExecutionContext GetClientContextCore()
		{
			return this.GetExecutionContext(Thread.CurrentThread);
		}

		protected override ExecutionContext GetServerContextCore(Thread thread)
		{
			return this.GetExecutionContext(thread);
		}

		private ExecutionContext GetExecutionContext(Thread thread)
		{
			try {
				_rwlock.EnterReadLock();
				int id = thread.ManagedThreadId;
				if (_dict.ContainsKey(id)) {
					var result = _dict[id];
					if (result.IsDisposing || result.IsDisposed) {
						result = new ExecutionContext();
						AddContext(id, result);
					}
					return result;
				} else {
					var result = new ExecutionContext();
					AddContext(id, result);
					return result;
				}
			} finally {
				if (_rwlock.IsReadLockHeld) {
					_rwlock.ExitReadLock();
				}
			}
			void AddContext(int id, ExecutionContext context)
			{
				try {
					_rwlock.EnterWriteLock();
					_dict.Add(id, context);
				} finally {
					if (_rwlock.IsWriteLockHeld) {
						_rwlock.ExitWriteLock();
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				try {
					_rwlock.EnterWriteLock();
					foreach (var item in _dict.Values) {
						item.Dispose();
					}
				} finally {
					if (_rwlock.IsWriteLockHeld) {
						_rwlock.ExitWriteLock();
					}
				}
				_rwlock.Dispose();
			}
			_dict.Clear();
			base.Dispose(disposing);
		}

		protected override async ValueTask DisposeAsyncCore()
		{
			try {
				_rwlock.EnterWriteLock();
				foreach (var item in _dict.Values) {
					await item.ConfigureAwait(true).DisposeAsync();
				}
			} finally {
				if (_rwlock.IsWriteLockHeld) {
					_rwlock.ExitWriteLock();
				}
			}
			if (_rwlock is IAsyncDisposable asyncDisposable) {
				await asyncDisposable.ConfigureAwait(false).DisposeAsync();
			} else {
				_rwlock.Dispose();
			}
			await base.DisposeAsyncCore();
		}
	}
}
