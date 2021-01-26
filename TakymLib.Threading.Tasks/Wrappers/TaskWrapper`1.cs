﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading.Tasks.Wrappers
{
	/// <summary>
	///  <see cref="System.Threading.Tasks.Task{TResult}"/>を<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult{TResult}"/>として扱える様にします。
	/// </summary>
	public readonly struct TaskWrapper<TResult> : IAsyncMethodResult<TResult>
	{
		private readonly Task<TResult> _task;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.AsyncState"/>の値を取得します。
		/// </summary>
		public object? AsyncState => _task.AsyncState;

		/// <summary>
		///  現在の<see cref="System.Threading.Tasks.Task"/>から<see cref="System.IAsyncResult.AsyncState"/>の値を取得します。
		/// </summary>
		public WaitHandle AsyncWaitHandle => ((IAsyncResult)(_task)).AsyncWaitHandle;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.Exception"/>の値を取得します。
		/// </summary>
		public Exception? Exception => _task.Exception;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCompleted"/>の値を取得します。
		/// </summary>
		public bool IsCompleted => _task.IsCompleted;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCompletedSuccessfully"/>の値を取得します。
		/// </summary>
		public bool IsCompletedSuccessfully => _task.IsCompletedSuccessfully;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsFaulted"/>の値を取得します。
		/// </summary>
		public bool IsFailed => _task.IsFaulted;

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task.IsCanceled"/>の値を取得します。
		/// </summary>
		public bool IsCancelled => _task.IsCanceled;

		/// <summary>
		///  現在の<see cref="System.Threading.Tasks.Task{TResult}"/>から<see cref="System.IAsyncResult.CompletedSynchronously"/>の値を取得します。
		/// </summary>
		public bool CompletedSynchronously => ((IAsyncResult)(_task)).CompletedSynchronously;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Tasks.Wrappers.TaskWrapper{TResult}"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		public TaskWrapper(Task<TResult> task)
		{
			task.EnsureNotNull(nameof(task));
			_task = task;
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task{TResult}.GetAwaiter"/>を呼び出します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.TaskAwaiterWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiterWrapper<TResult> GetAwaiter()
		{
			return new(_task.GetAwaiter());
		}

		/// <summary>
		///  <see cref="System.Threading.Tasks.Task{TResult}.ConfigureAwait(bool)"/>を呼び出します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元に実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </param>
		/// <returns><see cref="TakymLib.Threading.Tasks.Wrappers.ConfiguredTaskAwaitableWrapper{TResult}"/>オブジェクトです。</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredTaskAwaitableWrapper<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task.ConfigureAwait(continueOnCapturedContext));
		}

		IAwaiter<TResult> IAwaitable<TResult>.GetAwaiter()
		{
			return this.GetAwaiter();
		}

		IAwaitable<TResult> IAsyncMethodResult<TResult>.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
	}
}