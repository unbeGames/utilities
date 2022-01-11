using System;
using System.Collections.Generic;
using UnityEngine;

//
// Logging of messages
//
// There are three different types of messages:
//
// Debug.Log/Warn/Error coming from unity (or code, e.g. packages, not using GameDebug)
//    These get caught here and sent onto the console and into our log file
// GameDebug.Log/Warn/Error coming from game
//    These gets sent onto the console and into our log file
//    *IF* we are in editor, they are also sent to Debug.* so they show up in editor Console window
// Console.Write
//    Only used for things that should not be logged. Typically reponses to user commands. Only shown on Console.
//



namespace Unbegames.Services {
	public interface IWritable {
		void Write(string msg);
	}

	public static class GameDebug {
		private static System.IO.StreamWriter logFile = null;
		private static bool forwardToDebug = true;
		private static Queue<string> logQueue = new Queue<string>();
		private static bool isWriting = false;
		private static IWritable writable;

		public static void Init(IWritable wr, System.IO.StreamWriter lf) {
			writable = wr;
			forwardToDebug = Application.isEditor;
			Application.logMessageReceived += LogCallback;
			logFile = lf;
		}

		public static void Shutdown() {
			Application.logMessageReceived -= LogCallback;
			if (logFile != null)
				logFile.Close();
			logFile = null;
		}

		static void LogCallback(string message, string stack, LogType logtype) {
			switch (logtype) {
				default:
				case LogType.Log:
					_Log(message);
					break;
				case LogType.Warning:
					_LogWarning(message);
					break;
				case LogType.Error:
					_LogError(message);
					break;
				case LogType.Exception:
					_LogException(message, stack);
					break;
			}
		}

		public static void Log(string message) {
			if (forwardToDebug)
				Debug.Log(message);
			else
				_Log(message);
		}

		public static void TickLateUpdate() {
			if (logFile != null && !isWriting && logQueue.Count > 0) {
				StartWriting();
			}
		}

		static async void StartWriting() {
			isWriting = true;
			while (logQueue.Count > 0) {
				var logline = logQueue.Dequeue();
				await logFile.WriteLineAsync(logline);
			}
			isWriting = false;
		}

		static void _Log(string message) {
			writable.Write($"{Time.frameCount}: {message}");
			if (logFile != null)
				SheduleLogWrite($"{Time.frameCount}: {message}");
		}

		public static void LogError(string message, UnityEngine.Object context = null) {
			if (forwardToDebug)
				Debug.LogError(message, context);
			else
				_LogError(message);
		}

		static void _LogError(string message) {
			writable.Write($"{Time.frameCount}: <color=orange>[ERR]</color> {message}");
			if (logFile != null)
				SheduleLogWrite($"{Time.frameCount}: [ERR] {message}");
		}

		public static void LogException(string message, Exception e) {
			if (forwardToDebug)
				Debug.LogException(e);
			else
				_LogException(message, e.StackTrace);
		}

		static void _LogException(string message, string stack) {
			writable.Write($"{Time.frameCount}: <color=red>[EXC]</color> {message}");
			writable.Write(stack);
			if (logFile != null) {
				SheduleLogWrite($"{Time.frameCount}: [EXC] {message}");
				SheduleLogWrite(stack);
			}
		}

		public static void LogWarning(string message) {
			if (forwardToDebug)
				Debug.LogWarning(message);
			else
				_LogWarning(message);
		}

		static void _LogWarning(string message) {
			writable.Write($"{Time.frameCount}: <color=yellow>[WARN]</color> {message}");
			if (logFile != null)
				SheduleLogWrite($"{Time.frameCount}: [WARN] {message}");
		}

		static void SheduleLogWrite(string message) {
			logQueue.Enqueue(message);
		}

		public static void Assert(bool condition) {
			if (!condition)
				throw new ApplicationException("GAME ASSERT FAILED");
		}

		public static void Assert(bool condition, string msg) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {msg}");
		}

		public static void Assert<T>(bool condition, string format, T arg1) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {string.Format(format, arg1)}");
		}

		public static void Assert<T1, T2>(bool condition, string format, T1 arg1, T2 arg2) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {string.Format(format, arg1, arg2)}");
		}

		public static void Assert<T1, T2, T3>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {string.Format(format, arg1, arg2, arg3)}");
		}

		public static void Assert<T1, T2, T3, T4>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {string.Format(format, arg1, arg2, arg3, arg4)}");
		}

		public static void Assert<T1, T2, T3, T4, T5>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			if (!condition)
				throw new ApplicationException($"GAME ASSERT FAILED : {string.Format(format, arg1, arg2, arg3, arg4, arg5)}");
		}
	}
}
