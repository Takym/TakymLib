
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
#pragma warning disable CA2012 // ValueTask �𐳂����g�p����K�v������܂�
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
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch(this AsyncEventHandler? handler, object? sender, EventArgs e)
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1��̂ݎ��s��
				}
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
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch<TEventArgs>(this AsyncEventHandler<TEventArgs>? handler, object? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1��̂ݎ��s��
				}
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
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs>? handler, TSender? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TSender, TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1��̂ݎ��s��
				}
			}
		}
#pragma warning restore CA2012 // ValueTask �𐳂����g�p����K�v������܂�
	}
}
