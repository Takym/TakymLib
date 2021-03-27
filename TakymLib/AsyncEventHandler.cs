/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  �C�x���g����������񓯊��֐���\���܂��B
	/// </summary>
	/// <param name="sender">�C�x���g�̔������ł��B</param>
	/// <param name="e">��̃C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
	/// <returns>���̏����̔񓯊�����ł��B</returns>
	public delegate ValueTask AsyncEventHandler(object? sender, EventArgs e);

	/// <summary>
	///  �C�x���g����������񓯊��֐���\���܂��B
	/// </summary>
	/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
	/// <param name="sender">�C�x���g�̔������ł��B</param>
	/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
	/// <returns>���̏����̔񓯊�����ł��B</returns>
	public delegate ValueTask AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  �C�x���g����������񓯊��֐���\���܂��B
	/// </summary>
	/// <typeparam name="TSender">�������̎�ނł��B</typeparam>
	/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
	/// <param name="sender">�C�x���g�̔������ł��B</param>
	/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
	/// <returns>���̏����̔񓯊�����ł��B</returns>
	public delegate ValueTask AsyncEventHandler<TSender, TEventArgs>(TSender? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  �^'<see cref="TakymLib.AsyncEventHandler"/>'�̋@�\���g�����܂��B
	///  ���̃N���X�͐ÓI�N���X�ł��B
	/// </summary>
	public static class AsyncEventHandlerExtensions
	{
		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">��̃C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch(this AsyncEventHandler? handler, object? sender, EventArgs e)
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch<TEventArgs>(this AsyncEventHandler<TEventArgs>? handler, object? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <typeparam name="TSender">�������̎�ނł��B</typeparam>
		/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		public static ValueTask Dispatch<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs>? handler, TSender? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <typeparam name="TDelegate">�f���Q�[�g�̎�ނł��B</typeparam>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">��̃C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("����� AsyncEventHandler �𗘗p���Ă��������B"
#if NET5_0_OR_GREATER
			, DiagnosticId = "TakymLib_Dispatch"
#endif
		)]
		public static ValueTask Dispatch<TDelegate>(this TDelegate handler, object? sender, EventArgs e)
			where TDelegate: Delegate
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <typeparam name="TDelegate">�f���Q�[�g�̎�ނł��B</typeparam>
		/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("����� AsyncEventHandler �𗘗p���Ă��������B"
#if NET5_0_OR_GREATER
			, DiagnosticId = "TakymLib_Dispatch"
#endif
		)]
		public static ValueTask Dispatch<TDelegate, TEventArgs>(this TDelegate handler, object? sender, TEventArgs e)
			where TDelegate : Delegate
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

		/// <summary>
		///  �w�肳�ꂽ�C�x���g�𔭉΂����܂��B
		/// </summary>
		/// <remarks>
		///  ���s�����͕ۏ؂���܂���B
		/// </remarks>
		/// <typeparam name="TDelegate">�f���Q�[�g�̎�ނł��B</typeparam>
		/// <typeparam name="TSender">�������̎�ނł��B</typeparam>
		/// <typeparam name="TEventArgs">�C�x���g�f�[�^�̎�ނł��B</typeparam>
		/// <param name="handler">���s����C�x���g�n���h���ł��B<see langword="null"/>�̏ꍇ�͎��s����܂���B</param>
		/// <param name="sender">�C�x���g�̔������ł��B</param>
		/// <param name="e">�C�x���g�f�[�^���i�[���Ă���I�u�W�F�N�g�ł��B</param>
		/// <returns>���̏����̔񓯊�����ł��B</returns>
		/// <exception cref="System.ArgumentException"/>
		/// <exception cref="System.MemberAccessException"/>
		[Obsolete("����� AsyncEventHandler �𗘗p���Ă��������B"
#if NET5_0_OR_GREATER
			, DiagnosticId = "TakymLib_Dispatch"
#endif
		)]
		public static ValueTask Dispatch<TDelegate, TSender, TEventArgs>(this TDelegate handler, TSender? sender, TEventArgs e)
			where TDelegate : Delegate
			where TEventArgs: EventArgs
		{
			if (handler is null) {
				return default;
			} else {
				return DispatchCore(handler, sender, e);
			}
		}

#if NET5_0_OR_GREATER
#pragma warning disable CA2012 // ValueTask �𐳂����g�p����K�v������܂�
#endif
		private static async ValueTask DispatchCore<TSender, TEventArgs>(Delegate handler, TSender? sender, TEventArgs e)
			where TEventArgs : EventArgs
		{
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = WrapHandler<TSender, TEventArgs>(handlers[i])(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1��̂ݎ��s��
				}
			}
		}
#if NET5_0_OR_GREATER
#pragma warning restore CA2012 // ValueTask �𐳂����g�p����K�v������܂�
#endif

		private static AsyncEventHandler<TSender, TEventArgs> WrapHandler<TSender, TEventArgs>(Delegate handler)
			where TEventArgs : EventArgs
		{
			return handler switch {
				AsyncEventHandler<TSender, TEventArgs> h => h,
				AsyncEventHandler<TEventArgs>          h => (sender, e) =>   h(sender, e),
				AsyncEventHandler                      h => (sender, e) =>   h(sender, e),
				Func<object?,  EventArgs,  ValueTask>  h => (sender, e) =>   h(sender, e),
				Func<object?,  TEventArgs, ValueTask>  h => (sender, e) =>   h(sender, e),
				Func<TSender?, TEventArgs, ValueTask>  h => (sender, e) =>   h(sender, e),
				EventHandler<TEventArgs>               h => (sender, e) => { h(sender, e); return default; },
				EventHandler                           h => (sender, e) => { h(sender, e); return default; },
				Action<object?,  EventArgs>            h => (sender, e) => { h(sender, e); return default; },
				Action<object?,  TEventArgs>           h => (sender, e) => { h(sender, e); return default; },
				Action<TSender?, TEventArgs>           h => (sender, e) => { h(sender, e); return default; },
				_ => throw new ArgumentException(Resources.AsyncEventHandlerExtensions_WrapHandler, nameof(handler))
			};
		}
	}
}
