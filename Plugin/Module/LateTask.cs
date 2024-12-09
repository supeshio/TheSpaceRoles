using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheSpaceRoles;

public class LateTask


{
    // 遅延タスクを管理するクラス
    private class ScheduledTask
    {
        public double ExecutionTime; // 実行予定時間（現在時刻＋遅延時間）
        public Action Task;          // 実行するアクション
    }

    private static readonly List<ScheduledTask> tasks = new(); // 登録されたタスク

    /// <summary>
    /// 遅延タスクを登録します。
    /// </summary>
    /// <param name="delayInSeconds">遅延時間（秒、小数対応）</param>
    /// <param name="action">実行するアクション</param>
    public static void AddTask(double delayInSeconds, Action action)
    {
        if (delayInSeconds < 0)
            throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "遅延時間は0以上である必要があります。");

        if (action == null)
            throw new ArgumentNullException(nameof(action), "アクションがnullです。");

        double executionTime = CurrentTimeInSeconds() + delayInSeconds;
        tasks.Add(new ScheduledTask { ExecutionTime = executionTime, Task = action });
    }

    /// <summary>
    /// 毎フレーム呼び出される処理（遅延タスクを管理・実行）。
    /// </summary>
    public static void Update()
    {
        double currentTime = CurrentTimeInSeconds();

        // 実行予定時間を過ぎたタスクを実行
        for (int i = tasks.Count - 1; i >= 0; i--)
        {
            if (tasks[i].ExecutionTime <= currentTime)
            {
                tasks[i].Task(); // タスクを実行
                tasks.RemoveAt(i); // 実行済みのタスクをリストから削除
            }
        }
    }

    /// <summary>
    /// 現在の時間を秒単位で取得します。
    /// </summary>
    private static double CurrentTimeInSeconds()
    {
        return DateTime.Now.Ticks / (double)TimeSpan.TicksPerSecond;
    }
}
