using L = Modding.Logger;

namespace EnemyHPBar;

internal static class Logger {
	private const string prefix = $"[{nameof(EnemyHPBar)}] - ";

	internal static void LogFine(string message) => L.LogFine(prefix + message);

	internal static void LogDebug(string message) => L.LogDebug(prefix + message);

	internal static void Log(string message) => L.Log(prefix + message);

	internal static void LogWarn(string message) => L.LogWarn(prefix + message);

	internal static void LogError(string message) => L.LogError(prefix + message);
}
