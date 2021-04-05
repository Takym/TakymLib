/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Security.Properties;

namespace TakymLib.Security
{
	/// <summary>
	///  メモリの検証を定期的に行います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class MemoryValidation : DisposableBase
	{
		/// <summary>
		///  メモリ検証スレッドを新たに開始します。
		/// </summary>
		/// <returns>メモリ検証スレッドを監視するオブジェクトです。</returns>
		public static MemoryValidation Start()
		{
			var mv = new MemoryValidation(1000);
			mv.StartCore();
			return mv;
		}

		private readonly Thread                        _thread;
		private readonly CancellationTokenSource       _cts_sub;
		private readonly CancellationTokenSource       _cts_main;
		private readonly CancellationTokenRegistration _ctr_main;
		private readonly int                           _interval;
		private readonly RandomNumberGenerator         _rng;
		private          long                          _value;
		private          long                          _mask;
		private          long                          _masked_and;
		private          long                          _masked_ior;
		private          long                          _masked_xor;

		private MemoryValidation(int interval)
		{
			_thread              = new Thread(this.BackgroundThread);
			_thread.IsBackground = true;
			_cts_sub             = new CancellationTokenSource();
			_cts_main            = new CancellationTokenSource();
			_ctr_main            = _cts_main.Token.Register(this.ThrowSecurityException, true);
			_interval            = interval;
			_rng                 = RandomNumberGenerator.Create();
		}

		private void StartCore()
		{
			_thread.Start();
		}

		private void BackgroundThread()
		{
			var token = _cts_sub.Token;
			try {
#if NET48
				byte[] buf = new byte[8];
#else
				Span<byte> buf = stackalloc byte[8];
#endif
				while (!token.IsCancellationRequested) {
					lock (this) {
						_rng.GetNonZeroBytes(buf);
						_value = BitConverter.ToInt64(buf
#if NET48
							, 0
#endif
						);
						_rng.GetNonZeroBytes(buf);
						_mask = BitConverter.ToInt64(buf
#if NET48
							, 0
#endif
						);
						_masked_and = _value & _mask;
						_masked_ior = _value | _mask;
						_masked_xor = _value ^ _mask;
					}
					Thread.Sleep(_interval);
					lock (this) {
						if (((_value & _mask) != _masked_and) ||
							((_value | _mask) != _masked_ior) ||
							((_value ^ _mask) != _masked_xor)) {
							_cts_main.Cancel();
						}
					}
				}
			} catch (OperationCanceledException oce) when (oce.CancellationToken == token) {
				return;
			}
		}

		private void ThrowSecurityException()
		{
			throw new SecurityException(Resources.MemoryValidation_SecurityException, this.GetType());
		}

		/// <summary>
		///  直ちにメモリ検証スレッドを停止させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.AggregateException"/>
		public void StopImmediately()
		{
			this.EnsureNotDisposed();
			_cts_sub.Cancel(true);
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (disposing) {
				_cts_sub .Dispose();
				_ctr_main.Dispose();
				_cts_main.Dispose();
				_rng     .Dispose();
				_thread.ExecutionContext?.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <inheritdoc/>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			if (_cts_sub is IAsyncDisposable asyncDisposable0) {
				await asyncDisposable0.ConfigureAwait(false).DisposeAsync();
			} else {
				_cts_sub.Dispose();
			}
#if NET48
			_ctr_main.Dispose();
#else
			await _ctr_main.ConfigureAwait(false).DisposeAsync();
#endif
			if (_cts_main is IAsyncDisposable asyncDisposable1) {
				await asyncDisposable1.ConfigureAwait(false).DisposeAsync();
			} else {
				_cts_main.Dispose();
			}
			if (_rng is IAsyncDisposable asyncDisposable2) {
				await asyncDisposable2.ConfigureAwait(false).DisposeAsync();
			} else {
				_rng.Dispose();
			}
			_thread.ExecutionContext?.Dispose();
			await base.DisposeAsyncCore();
		}
	}
}
