namespace TheSpaceRoles;

using System;
using System.Collections.Generic;

public class LateTask
{
    // 遅延タスクを管理するクラス
    private class ScheduledTask
    {
        public float ExecutionTime; // 実行予定時間（現在時刻＋遅延時間）
        public Action Task;          // 実行するアクション
        public float Interval;      // 実行間隔（繰り返し処理用）
        public float EndTime;       // 処理終了時間（繰り返し処理用）
        public bool IsRepeating;     // 繰り返し処理かどうか
    }

    private static readonly List<ScheduledTask> tasks = new(); // 登録されたタスク

    /// <summary>
    /// 遅延タスクを登録します。
    /// </summary>
    /// <param name="delayInSeconds">遅延時間（秒、小数対応）</param>
    /// <param name="action">実行するアクション</param>
    public static void AddTask(float delayInSeconds, Action action)
    {
        if (delayInSeconds < 0)
            throw new ArgumentOutOfRangeException(nameof(delayInSeconds), "遅延時間は0以上である必要があります。");

        if (action == null)
            throw new ArgumentNullException(nameof(action), "アクションがnullです。");

        float executionTime = CurrentTimeInSeconds() + delayInSeconds;
        tasks.Add(new ScheduledTask { ExecutionTime = executionTime, Task = action, IsRepeating = false });
    }

    /// <summary>
    /// 指定時間にわたって一定間隔で処理を実行するタスクを登録します。
    /// </summary>
    /// <param name="action">実行するアクション</param>
    /// <param name="intervalSeconds">実行間隔（秒、小数対応）</param>
    /// <param name="durationSeconds">処理を継続する時間（秒、小数対応）</param>
    public static void AddRepeatedTask(float intervalSeconds, float durationSeconds, Action<float> action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action), "アクションがnullです。");

        float startTime = CurrentTimeInSeconds();
        float endTime = startTime + durationSeconds;
        float nextExecutionTime = startTime;

        // 最初のタスクを登録
        tasks.Add(new ScheduledTask
        {
            ExecutionTime = nextExecutionTime,
            Task = () =>
            {
                float elapsed = CurrentTimeInSeconds() - startTime;
                action(elapsed); // 経過時間を引数に処理を実行
            },
            Interval = intervalSeconds,
            EndTime = endTime,
            IsRepeating = true
        });
    }

    /// <summary>
    /// 毎フレーム呼び出される処理（遅延タスクを管理・実行）。
    /// </summary>
    public static void Update()
    {
        float currentTime = CurrentTimeInSeconds();

        for (int i = tasks.Count - 1; i >= 0; i--)
        {
            var task = tasks[i];

            if (task.ExecutionTime <= currentTime)
            {
                task.Task(); // タスクを実行

                if (task.IsRepeating)
                {
                    // 次回実行時間を設定
                    task.ExecutionTime += task.Interval;
                    if (task.ExecutionTime > task.EndTime)
                    {
                        tasks.RemoveAt(i); // 繰り返し処理の終了
                    }
                }
                else
                {
                    tasks.RemoveAt(i); // 単発タスクは削除
                }
            }
        }
    }

    /// <summary>
    /// 現在の時間を秒単位で取得します。
    /// </summary>
    private static float CurrentTimeInSeconds()
    {
        return DateTime.Now.Ticks / (float)TimeSpan.TicksPerSecond;
    }
}

